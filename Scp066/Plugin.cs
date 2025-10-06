using System;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using RoleAPI;
using Scp066.Features;

namespace Scp066;

public class Scp066 : Plugin<Config>
{
    public override string Name => "Scp066";

    public override string Description => "Adds SCP-066, the noise maker, as a custom role with unique abilities and features.";

    public override string Author => "MedveMarci";
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
    
}