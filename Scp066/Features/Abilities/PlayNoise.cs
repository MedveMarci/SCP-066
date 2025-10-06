using System.Collections.Generic;
using System.Linq;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
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
        
        manager.AudioPlayer.AddClip("Beethoven", 0.5f);
        Timing.RunCoroutine(CheckEndOfPlayback(player, manager));
    }
    
    private static IEnumerator<float> CheckEndOfPlayback(Player scp066, ObjectManager manager)
    {
        if (Scp066.Instance.Config == null) yield break;
        var distance = Scp066.Instance.Config.Scp066Role.AudioConfig.MaxDistance;
        var damage = Scp066.Instance.Config.Damage;
        var damageText = Scp066.Instance.Config.CustomDeathText;

        if (distance <= 0 || damage <= 0)
            yield break;

        const float maxWaitForStart = 2f;
        var waited = 0f;
        
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
            // Deal damage to players near SCP-066
            foreach (var player in Player.ReadyList.Where(player => Vector3.Distance(scp066.Position, player.Position) <= distance && !player.IsSCP && player.IsAlive && player.Team != Team.OtherAlive))
            {
                player.Damage(new CustomReasonDamageHandler(damageText, damage));
            }
            
            yield return Timing.WaitForSeconds(0.5f);
        }
    }
}