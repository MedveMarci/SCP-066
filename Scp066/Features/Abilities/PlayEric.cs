using LabApi.Features.Wrappers;
using RoleAPI.API.Interfaces;
using RoleAPI.API.Managers;
using UnityEngine;

namespace Scp066.Features.Abilities;
public class PlayEric : Ability
{
    public override string Name => "\ud83c\udfb5 Eric?";
    public override string Description => "Play back random sound 'eric?'";
    public override int KeyId => 660;
    public override KeyCode KeyCode => KeyCode.Q;
    public override float Cooldown => 10f;
    protected override void ActivateAbility(Player player, ObjectManager manager)
    {
        var value = Random.Range(0, 3) + 1;
        if (!AudioClipStorage.AudioClips.ContainsKey($"Eric{value}"))
        {
            LogManager.Error($"[Scp066] The audio file 'Eric{value}.ogg' was not found for playback. Please ensure the file is placed in the correct directory.");
            return;
        }
        manager.AudioPlayer?.AddClip($"Eric{value}");
    }
}