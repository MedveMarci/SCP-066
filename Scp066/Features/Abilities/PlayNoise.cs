using System.Collections.Generic;
using System.Linq;
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
        
        manager.AudioPlayer.AddClip("Beethoven");
        Timing.RunCoroutine(CheckEndOfPlayback(player, manager));
    }
    
    private IEnumerator<float> CheckEndOfPlayback(Player scp066, ObjectManager manager)
    {
        float distance = Plugin.Singleton.Config.Distance;
        float damage = Plugin.Singleton.Config.Damage;
        string damageText = Plugin.Singleton.Config.Scp066RoleConfig.CustomDeathText;
        bool isBreakableWindows = Plugin.Singleton.Config.IsBreakableWindows;

        if (distance <= 0 || damage <= 0)
            yield break;

        float maxWaitForStart = 2f;
        float waited = 0f;
        
        // This is a test cycle in case the sound doesn't work.
        while (manager.AudioPlayer.ClipsById.Values.Any(clip => clip.Clip != "Beethoven"))
        {
            if (waited > maxWaitForStart)
            {
                yield break;
            }
            
            yield return Timing.WaitForSeconds(0.1f);
            waited += 0.1f;
        }
        
        
        // While the symphony is running
        while (manager.AudioPlayer.ClipsById.Values.Any(clip => clip.Clip == "Beethoven"))
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
                if (player == scp066 || player.IsDead || player.IsScp)
                    continue;
                
                if (Vector3.Distance(scp066.Position, player.Position) <= distance)
                {
                    player.Hurt(new CustomReasonDamageHandler(damageText, damage));
                }
            }
            
            yield return Timing.WaitForSeconds(0.1f);
        }
    }
}