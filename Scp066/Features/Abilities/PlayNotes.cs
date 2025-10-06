using LabApi.Features.Wrappers;
using RoleAPI.API.Interfaces;
using RoleAPI.API.Managers;
using UnityEngine;

namespace Scp066.Features.Abilities;
public class PlayNotes : Ability
{
    public override string Name => "\ud83c\udfb6 Note";
    public override string Description => "Play back random creepy notes";
    public override int KeyId => 661;
    public override KeyCode KeyCode => KeyCode.R;
    public override float Cooldown => 10f;
    protected override void ActivateAbility(Player player, ObjectManager manager)
    {
        var value = Random.Range(0, 6) + 1;
        if (!AudioClipStorage.AudioClips.ContainsKey($"Notes{value}"))
        {
            LogManager.Error($"[Scp066] The audio file 'Notes{value}.ogg' was not found for playback. Please ensure the file is placed in the correct directory.");
            return;
        }
        manager.AudioPlayer.AddClip($"Notes{value}");
    }
}