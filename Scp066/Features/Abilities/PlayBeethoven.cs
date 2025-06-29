using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using Scp066.Interfaces;
using UnityEngine;

namespace Scp066.Features.Abilities;
public class PlayBeethoven : Ability
{
    public override string Name => "Beethoven";
    public override string Description => "Plays Beethoven's Symphony No. 2, which kills players near SCP-066";
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
            this.OnKeyPressed(ev.Player);
        }
    }    
    
    protected override void ActivateAbility(Player player, AudioPlayer audioPlayer)
    {
        audioPlayer?.AddClip($"Beethoven");
        //Timing.RunCoroutine(this.CheckEndOfPlayback(player, audioPlayer));
    }
    
    private IEnumerator<float> CheckEndOfPlayback(Player scp066, AudioPlayer audioPlayer)
    {
        yield return Timing.WaitForSeconds(0.1f);
        while (true)
        {
            //Пока звук не прекратится
            
            foreach (Player player in Player.List)
            {
                if (player == scp066)
                    continue;

                if (player.IsScp)
                    continue;
                
                if (Vector3.Distance(scp066.Position, player.Position) < 10f)
                {
                    player.Health -= 10; //todo
                }
            }
            
            yield return Timing.WaitForSeconds(0.5f);
        }
    }
}