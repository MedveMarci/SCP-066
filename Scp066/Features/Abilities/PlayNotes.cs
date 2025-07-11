using Exiled.API.Features;
using Scp066.Interfaces;
using UnityEngine;

namespace Scp066.Features.Abilities;
public class PlayNotes : Ability
{
    public override string Name => "\ud83c\udfb6 Note";
    public override string Description => "Play back random creepy notes";
    public override int KeyId => 661;
    public override KeyCode KeyCode => KeyCode.R;
    public override float Cooldown => 10f;
    protected override void ActivateAbility(Player player, AudioPlayer audioPlayer)
    {
        int value = Random.Range(0, 6) + 1;
        audioPlayer?.AddClip($"Notes{value}");
    }
}