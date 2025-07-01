using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Enums;
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
        Exiled.Events.Handlers.Player.Hurting += this.OnHurting;
    }
    public override void Unregister()
    {
        Exiled.Events.Handlers.Player.TogglingNoClip -= this.OnTogglingNoClip;
        Exiled.Events.Handlers.Player.Hurting -= this.OnHurting;
    }

    private void OnTogglingNoClip(TogglingNoClipEventArgs ev)
    {
        if (CustomRole.Get(typeof(Scp066Role)) is Scp066Role scp066Role && scp066Role.Check(ev.Player))
        {
            ev.IsAllowed = false;
            OnKeyPressed(ev.Player);
        }
    }

    private void OnHurting(HurtingEventArgs ev)
    {
        if (CustomRole.Get(typeof(Scp066Role)) is Scp066Role scp066Role && scp066Role.Check(ev.Attacker))
        {
            if (ev.DamageHandler.Type is DamageType.CardiacArrest)
            {
                ev.Amount = Plugin.Singleton.Config.Damage;
            }
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
        float distance = Plugin.Singleton.Config.Distance;
        
        // While the symphony is running
        while (audioPlayer.ClipsById.Count > 0)
        {
            // SCP-066 can break the windows
            foreach (var window in Window.List)
            {
                if (Vector3.Distance(scp066.Position, window.Position) <= distance && !window.IsBroken)
                {
                    window.BreakWindow();
                }
            }
            
            // Deal damage to players near SCP-066
            foreach (Player player in Player.List)
            {
                if (player == scp066)
                    continue;

                if (player.IsScp)
                    continue;
                
                if (Vector3.Distance(scp066.Position, player.Position) <= distance)
                {
                    /*// idk it works?
                    if (player.TryGetEffect(out CardiacArrest effect))
                    {
                        effect._attacker = scp066.Footprint;
                        effect._duration += 0.5f;
                    }
                    */
                    player.EnableEffect<CardiacArrest>(0.5f);
                }
            }
            
            yield return Timing.WaitForSeconds(0.5f);
        }
    }
}