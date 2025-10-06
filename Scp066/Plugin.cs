using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using RoleAPI;
using Scp066.Features;

namespace Scp066;

public class Scp066 : Plugin<Config>
{
    private const bool PreRelease = false;
    public override string Name => "Scp066";
    public override string Description => "Adds SCP-066, the noise maker, as a custom role with unique abilities and features.";
    public override string Author => "RisottoMan, LabApi version: MedveMarci";
    public override Version Version => new(1, 0, 0);
    public override Version RequiredApiVersion { get; } = new(LabApiProperties.CompiledVersion);
    public static Scp066 Instance { get; private set; }
    private Scp066Role Role { get; set; }

    public override void Enable()
    {
        Instance = this;
        Startup.SetupAPI(Name);
        Startup.RegisterCustomRole(Role);
        LabApi.Events.Handlers.Scp0492Events.StartingConsumingCorpse += EventHandler.OnStartingConsumingCorpse;
        _ = CheckForUpdatesAsync(Version);
    }
    
    public override void LoadConfigs()
    {
        base.LoadConfigs();
        Role = Config.Scp066Role;
    }

    public override void Disable()
    {
        Instance = null;
        Startup.UnRegisterCustomRole(Role);
        LabApi.Events.Handlers.Scp0492Events.StartingConsumingCorpse -= EventHandler.OnStartingConsumingCorpse;
    }
    
    internal static async Task CheckForUpdatesAsync(Version currentVersion)
    {
        try
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd($"{Instance.Name}/{currentVersion}");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");

            var repo = $"MedveMarci/{Instance.Name}";
            var latestStableJson = await client.GetStringAsync($"https://api.github.com/repos/{repo}/releases/latest")
                .ConfigureAwait(false);
            var allReleasesJson = await client
                .GetStringAsync($"https://api.github.com/repos/{repo}/releases?per_page=20").ConfigureAwait(false);

            using var latestStableDoc = JsonDocument.Parse(latestStableJson);
            using var allReleasesDoc = JsonDocument.Parse(allReleasesJson);

            var latestStableRoot = latestStableDoc.RootElement;
            string stableTag = null;
            if (latestStableRoot.TryGetProperty("tag_name", out var tagProp))
                stableTag = tagProp.GetString();
            var stableVer = ParseVersion(stableTag);

            JsonElement? latestPre = null;
            Version preVer = null;
            string preTag = null;

            if (allReleasesDoc.RootElement.ValueKind == JsonValueKind.Array)
            {
                DateTime? bestPublishedAt = null;
                foreach (var rel in allReleasesDoc.RootElement.EnumerateArray().Where(rel => rel.ValueKind == JsonValueKind.Object))
                {
                    var draft = rel.TryGetProperty("draft", out var draftProp) &&
                                draftProp.ValueKind == JsonValueKind.True;
                    if (draft) continue;

                    var prerelease = rel.TryGetProperty("prerelease", out var preProp) &&
                                     preProp.ValueKind == JsonValueKind.True;
                    if (!prerelease) continue;

                    DateTime? publishedAt = null;
                    if (rel.TryGetProperty("published_at", out var pubProp))
                    {
                        var s = pubProp.GetString();
                        if (!string.IsNullOrWhiteSpace(s) && DateTime.TryParse(s, out var dt))
                            publishedAt = dt;
                    }

                    if (latestPre != null && (!publishedAt.HasValue ||
                                              (bestPublishedAt.HasValue && publishedAt.Value <= bestPublishedAt.Value)))
                        continue;
                    latestPre = rel;
                    bestPublishedAt = publishedAt;
                }
            }

            if (latestPre.HasValue)
            {
                if (latestPre.Value.TryGetProperty("tag_name", out var preTagProp))
                    preTag = preTagProp.GetString();
                preVer = ParseVersion(preTag);
            }

            var outdatedStable = stableVer != null && stableVer > currentVersion;
            var prereleaseNewer = preVer != null && preVer > currentVersion && !outdatedStable;

            if (outdatedStable)
                LogManager.Info(
                    $"A new {Instance.Name} version is available: {stableTag} (current {currentVersion}). Download: https://github.com/MedveMarci/{Instance.Name}/releases/latest",
                    ConsoleColor.DarkRed);
            else if (prereleaseNewer)
                LogManager.Info(
                    $"A newer pre-release is available: {preTag} (current {currentVersion}). Download: https://github.com/MedveMarci/{Instance.Name}/releases/tag/{preTag}",
                    ConsoleColor.DarkYellow);
            else
                LogManager.Info(
                    $"Thanks for using {Instance.Name} v{currentVersion}. To get support and latest news, join to my Discord Server: https://discord.gg/KmpA8cfaSA",
                    ConsoleColor.Blue);
            if (PreRelease)
                LogManager.Info(
                    "This is a pre-release version. There might be bugs, if you find one, please report it on GitHub or Discord.",
                    ConsoleColor.DarkYellow);
        }
        catch (Exception e)
        {
            LogManager.Error($"Version check failed.\n{e}");
        }
    }

    private static Version ParseVersion(string tag)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(tag)) return null;
            var t = tag.Trim();
            if (t.StartsWith("v", StringComparison.OrdinalIgnoreCase))
                t = t.Substring(1);

            var cut = t.IndexOfAny(['-', '+']);
            if (cut >= 0)
                t = t.Substring(0, cut);

            return Version.TryParse(t, out var v) ? v : null;
        }
        catch
        {
            return null;
        }
    }
    
}