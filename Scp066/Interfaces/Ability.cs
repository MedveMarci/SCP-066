using Exiled.API.Features;
using Scp066.Features.Controller;

namespace Scp066.Interfaces;
public abstract class Ability : IAbility
{
    public virtual string Name { get; }
    public virtual string Description { get; }
    public virtual float Cooldown { get; }
    public virtual void Register() {}
    public virtual void Unregister() {}
    
    public void OnKeyPressed(Player player)
    {
        if (player is null)
            return;

        PlayerController controller = player.GameObject.GetComponent<PlayerController>();
        
        // Check current audio
        AudioPlayer audioPlayer = controller.GetCurrentAudioPlayer;
        /*
        if (audioPlayer is not null)
        {
            if (audioPlayer.TryGetClip(1, out AudioClipPlayback clip) && clip.Progress > 0) //has sound?
                return;
        }*/
        
        // Check cooldown for the ability
        CooldownController cooldown = player.GameObject.GetComponent<CooldownController>();
        if (!cooldown.IsAbilityAvailable(this.Name))
            return;
        
        // Set a cooldown for the ability
        cooldown.SetCooldownForAbility(this.Name, this.Cooldown);
        
        // Activate the ability
        this.ActivateAbility(player, audioPlayer);
        Log.Debug($"[Ability] Activating the {this.Name.ToLower()} ability");
    }
    
    protected abstract void ActivateAbility(Player player, AudioPlayer audioPlayer);
}