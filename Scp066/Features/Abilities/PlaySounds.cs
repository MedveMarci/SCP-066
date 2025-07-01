using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Scp066.Interfaces;
using UnityEngine;

namespace Scp066.Features.Abilities;
public class PlaySounds : Ability
{
    public override string Name => "\ud83c\udfb5 Eric?";
    public override string Description => "Play back random creepy notes and 'eric'";
    public override int KeyId => 660;
    public override string KeyCode => "Q";
    public override float Cooldown => 10f;
    public override void Register()
    {
        Exiled.Events.Handlers.Player.VoiceChatting += this.OnVoiceChatting;
    }
    public override void Unregister()
    {
        Exiled.Events.Handlers.Player.VoiceChatting -= this.OnVoiceChatting;
    }

    private void OnVoiceChatting(VoiceChattingEventArgs ev)
    {
        var scp066Role = CustomRole.Get(typeof(Scp066Role)) as Scp066Role;
        if (scp066Role != null && scp066Role.Check(ev.Player))
        {
            ev.IsAllowed = false;
            OnKeyPressed(ev.Player);
        }
    }
    
    protected override void ActivateAbility(Player player, AudioPlayer audioPlayer)
    {
        int value = Random.Range(0, 4) + 1;
        string clip = $"Eric{value}";

        if (value == 4)
        {
            value = Random.Range(0, 6) + 1;
            clip = $"Notes{value}";
        }
        
        audioPlayer?.AddClip(clip);
    }
}