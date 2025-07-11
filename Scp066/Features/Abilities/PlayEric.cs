using Exiled.API.Features;
using Scp066.Interfaces;
using UnityEngine;

namespace Scp066.Features.Abilities;
public class PlayEric : Ability
{
    public override string Name => "\ud83c\udfb5 Eric?";
    public override string Description => "Play back random sound 'eric?'";
    public override int KeyId => 660;
    public override KeyCode KeyCode => KeyCode.Q;
    public override float Cooldown => 10f;
    protected override void ActivateAbility(Player player, AudioPlayer audioPlayer)
    {
        int value = Random.Range(0, 3) + 1;
        audioPlayer?.AddClip($"Eric{value}");
    }
}