using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using Scp066.Interfaces;
using UnityEngine;

namespace Scp066.Features.Abilities;
public class PlayNoise : Ability
{
    public override string Name => "\ud83c\udfba Noise";
    public override string Description => "Plays Beethoven's Symphony No. 2, which kills players near SCP-066";
    public override int KeyId => 661;
    public override string KeyCode => "Alt";
    public override float Cooldown => 40f;
    public override void Register()
    {
        Exiled.Events.Handlers.Player.TogglingNoClip += this.OnTogglingNoClip;
    }
    public override void Unregister()
    {
        Exiled.Events.Handlers.Player.TogglingNoClip -= this.OnTogglingNoClip;
    }

    private void OnTogglingNoClip(TogglingNoClipEventArgs ev)
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
        if (audioPlayer is null)
            return;
        
        audioPlayer.AddClip($"Beethoven");
        Timing.RunCoroutine(CheckEndOfPlayback(player, audioPlayer));
    }
    
    private IEnumerator<float> CheckEndOfPlayback(Player scp066, AudioPlayer audioPlayer)
    {
        while (audioPlayer.ClipsById.Count > 0)
        {
            foreach (Player player in Player.List)
            {
                if (player == scp066)
                    continue;

                if (player.IsScp)
                    continue;
                
                if (Vector3.Distance(scp066.Position, player.Position) <= 5f)
                {
                    player.EnableEffect<CardiacArrest>(0.5f);
                }
            }
            
            yield return Timing.WaitForSeconds(0.5f);
        }
    }
}