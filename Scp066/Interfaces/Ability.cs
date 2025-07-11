using System.Linq;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using Scp066.Features;
using Scp066.Features.Controller;
using UnityEngine;
using UserSettings.ServerSpecific;

namespace Scp066.Interfaces;
public abstract class Ability : IAbility
{
    public virtual string Name { get; }
    public virtual string Description { get; }
    public virtual int KeyId { get; }
    public virtual KeyCode KeyCode { get; }
    public virtual float Cooldown { get; }
    public virtual void Register()
    {
        ServerSpecificSettingsSync.ServerOnSettingValueReceived += OnKeybindActivateAbility;
    }

    public virtual void Unregister()
    {
        ServerSpecificSettingsSync.ServerOnSettingValueReceived -= OnKeybindActivateAbility;
    }

    private void OnKeybindActivateAbility(ReferenceHub referenceHub, ServerSpecificSettingBase settingBase)
    {
        // Check keybind settings
        if (settingBase is not SSKeybindSetting keybindSetting || keybindSetting.SettingId != this.KeyId || !keybindSetting.SyncIsPressed)
            return;
        
        // Check player
        if (!Player.TryGet(referenceHub, out Player player))
            return;

        if (CustomRole.Get(typeof(Scp066Role)) is not Scp066Role scp066Role)
            return;

        if (!scp066Role.Check(player))
            return;

        PlayerController controller = player.GameObject.GetComponent<PlayerController>();
        
        // Check current audio
        AudioPlayer audioPlayer = controller.GetCurrentAudioPlayer;
        if (audioPlayer is not null && audioPlayer.ClipsById.Any())
            return;
        
        // Check cooldown for the ability
        CooldownController cooldown = player.GameObject.GetComponent<CooldownController>();
        if (!cooldown.IsAbilityAvailable(this.Name))
            return;
        
        // Set a cooldown for the ability
        cooldown.SetCooldownForAbility(this.Name, this.Cooldown);
        
        // Activate the ability
        this.ActivateAbility(player, audioPlayer);
        Log.Debug($"[Ability] Activating the {this.Name.ToLower()} ability");
    }

    protected abstract void ActivateAbility(Player player, AudioPlayer audioPlayer);
}