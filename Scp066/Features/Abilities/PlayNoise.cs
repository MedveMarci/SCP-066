using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using PlayerStatsSystem;
using RoleAPI.API.Interfaces;
using RoleAPI.API.Managers;
using UnityEngine;

namespace Scp066.Features.Abilities;
public class PlayNoise : Ability
{
    public override string Name => "\ud83c\udfba Noise";
    public override string Description => "Plays Symphony, which kills players";
    public override int KeyId => 662;
    public override KeyCode KeyCode => KeyCode.F;
    public override float Cooldown => 40f;
    protected override void ActivateAbility(Player player, ObjectManager manager)
    {
        if (manager.AudioPlayer is null)
            return;
        
        int value = Random.Range(0, 3) + 1;
        manager.AudioPlayer.AddClip($"Beethoven");
        Timing.RunCoroutine(CheckEndOfPlayback(player, manager));
    }
    
    private IEnumerator<float> CheckEndOfPlayback(Player scp066, ObjectManager manager)
    {
        float distance = Plugin.Singleton.Config.Distance;
        float damage = Plugin.Singleton.Config.Damage;
        bool isBreakableWindows = Plugin.Singleton.Config.IsBreakableWindows;

        if (distance <= 0)
            yield break;
        
        // While the symphony is running
        while (manager.AudioPlayer.ClipsById.Count > 0)
        {
            // SCP-066 can break the windows
            if (isBreakableWindows is true)
            {
                foreach (var window in Window.List)
                {
                    if (Vector3.Distance(scp066.Position, window.Position) <= distance && !window.IsBroken)
                    {
                        window.BreakWindow();
                    }
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
                    //player.EnableEffect<CardiacArrest>(0.5f);
                    player.Hurt(new CustomReasonDamageHandler("Dead by SCP-066", damage));
                }
            }
            
            yield return Timing.WaitForSeconds(0.5f);
        }
    }
}