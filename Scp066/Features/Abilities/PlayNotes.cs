using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Scp066.Interfaces;
using UnityEngine;

namespace Scp066.Features.Abilities;
public class PlayNotes : Ability
{
    public override string Name => "Notes";
    public override string Description => "Play back random creepy notes. Its safe for players, but the sounds are annoying";
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
            this.OnKeyPressed(ev.Player);
        }
    }
    
    protected override void ActivateAbility(Player player, AudioPlayer audioPlayer)
    {
        int value = Random.Range(0, 6) + 1;
        //todo add eric
        audioPlayer?.AddClip($"Notes{value}");
    }
}