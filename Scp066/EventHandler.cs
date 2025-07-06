using System;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Scp330;
using Exiled.Events.EventArgs.Warhead;
using MEC;
using Scp066.Features;
using Scp066.Features.Manager;
using Random = UnityEngine.Random;

namespace Scp066;
public class EventHandler
{
    private readonly Plugin _plugin;
    private Scp066Role _scp066role;
    public EventHandler(Plugin plugin)
    {
        _plugin = plugin;
    
        Exiled.Events.Handlers.Server.RoundStarted += this.OnRoundStarted;
        Exiled.Events.Handlers.Warhead.Starting += this.OnWarheadStart;
        Exiled.Events.Handlers.Warhead.Stopping += this.OnWarheadStop;
        Exiled.Events.Handlers.Scp096.AddingTarget += this.OnAddingTarget;
        Exiled.Events.Handlers.Player.SpawningRagdoll += this.OnSpawningRagdoll;
        Exiled.Events.Handlers.Player.EnteringPocketDimension += this.OnEnteringPocketDimension;
        Exiled.Events.Handlers.Player.SearchingPickup += this.OnSearchingPickup;
        Exiled.Events.Handlers.Player.DroppingItem += this.OnDroppingItem;
        Exiled.Events.Handlers.Player.Hurting += this.OnPlayerHurting;
        Exiled.Events.Handlers.Player.UsingItem += this.OnUsingItem;
        Exiled.Events.Handlers.Player.UsingItem += this.OnUsingItem;
        Exiled.Events.Handlers.Player.Dying += this.OnPlayerDying;
        Exiled.Events.Handlers.Scp330.InteractingScp330 += this.OnInteractingScp330;
        Exiled.Events.Handlers.Player.Verified += this.OnVerified;
        Exiled.Events.Handlers.Player.ChangingSpectatedPlayer += this.OnChangingSpectatedPlayer;
        Exiled.Events.Handlers.Player.InteractingDoor += this.OnInteractionDoor;
    }
    
    ~EventHandler()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= this.OnRoundStarted;
        Exiled.Events.Handlers.Warhead.Starting -= this.OnWarheadStart;
        Exiled.Events.Handlers.Warhead.Stopping -= this.OnWarheadStop;
        Exiled.Events.Handlers.Scp096.AddingTarget -= this.OnAddingTarget;
        Exiled.Events.Handlers.Player.SpawningRagdoll -= this.OnSpawningRagdoll;
        Exiled.Events.Handlers.Player.EnteringPocketDimension -= this.OnEnteringPocketDimension;
        Exiled.Events.Handlers.Player.SearchingPickup -= this.OnSearchingPickup;
        Exiled.Events.Handlers.Player.DroppingItem -= this.OnDroppingItem;
        Exiled.Events.Handlers.Player.Hurting -= this.OnPlayerHurting;
        Exiled.Events.Handlers.Player.UsingItem -= this.OnUsingItem;
        Exiled.Events.Handlers.Player.UsingItem -= this.OnUsingItem;
        Exiled.Events.Handlers.Player.Dying -= this.OnPlayerDying;
        Exiled.Events.Handlers.Scp330.InteractingScp330 -= this.OnInteractingScp330;
        Exiled.Events.Handlers.Player.Verified -= this.OnVerified;
        Exiled.Events.Handlers.Player.ChangingSpectatedPlayer -= this.OnChangingSpectatedPlayer;
        Exiled.Events.Handlers.Player.InteractingDoor -= this.OnInteractionDoor;
    }
    
    /// <summary>
    /// Logic of choosing SCP-066 if the round is started
    /// </summary>
    private void OnRoundStarted()
    {
        _scp066role = CustomRole.Get(typeof(Scp066Role)) as Scp066Role;
        if (_scp066role is null)
        {
            Log.Error("Custom role SCP-066 role not found or not registered");
            return;
        }

        // Minimum and maximum number of Players for the chance of SCP-066 appearing
        float min = _plugin.Config.MinimumPlayers - 1;
        float max = _plugin.Config.MaximumPlayers;

        if (min < 0 || max < 0)
        {
            Log.Error("Set the number of players to normal values in config");
            return;
        }
        
        // Add SCP-066 if no in the game
        if (_scp066role!.TrackedPlayers.Count >= _scp066role.SpawnProperties.Limit)
            return;
        
        for (int i = 0; i < _scp066role.SpawnProperties.Limit; i++)
        {
            // List of people who could potentially become SCP-066
            var players = Player.List.Where(r => r.IsHuman && !r.IsNPC && r.CustomInfo == null).ToList();
            // A minimum of players is required
            if (players.Count < min || players.Count == 0)
                return;
        
            // The formula for the chance of SCP-066 appearing in a round depends on count of players
            float value = Math.Max(min, Math.Min(max, Player.List.Count));
            float chance = (value - min) / (max - min);
            
            // Checking the chance to spawn in current round
            float randomValue = Random.value;

            Log.Debug($"[OnRoundStarted] Spawn chance {randomValue} >= {chance}");
            
            if (randomValue >= chance)
                return;
            
            // Choosing a random player
            Player randomPlayer = players.RandomItem();

            Timing.CallDelayed(0.05f, () =>
            {
                _scp066role.AddRole(randomPlayer);
            });
        }
    }

    /// <summary>
    /// Allow the use of abilities for SCP-066
    /// </summary>
    private void OnUsingItem(UsingItemEventArgs ev)
    {
        if (_scp066role != null && _scp066role.Check(ev.Player))
        {
            ev.IsAllowed = false;
        }
    }

    /// <summary>
    /// Block any damage from players
    /// </summary>
    private void OnPlayerHurting(HurtingEventArgs ev)
    {
        if (_scp066role is null)
            return;
        
        if (_scp066role.Check(ev.Player))
        {
            // Disable damage from car
            if (ev.DamageHandler.Type == DamageType.Crushed && 
                ev.Player.CurrentRoom.Type == RoomType.Surface)
            {
                ev.IsAllowed = false;
            }
        
            // Disable damage from tesla
            if (ev.DamageHandler.Type == DamageType.Tesla)
            {
                ev.IsAllowed = false;
            }
        
            // Disable fall damage
            if (ev.DamageHandler.Type == DamageType.Falldown)
            {
                ev.IsAllowed = false;
            }
            
            // Increase damage from decontamination
            if (ev.DamageHandler.Type == DamageType.Decontamination)
            {
                ev.Amount = 300;
            }
        }

        if (_scp066role.Check(ev.Attacker))
        {
            ev.Amount = 0;
        }
    }
    
    /// <summary>
    /// Does not allow SCP-066 to pick up items
    /// </summary>
    private void OnSearchingPickup(SearchingPickupEventArgs ev)
    {
        if (_scp066role != null && _scp066role.Check(ev.Player))
        {
            ev.IsAllowed = false;
        }
    }

    /// <summary>
    /// Does not allow SCP-066 to drop items
    /// </summary>
    private void OnDroppingItem(DroppingItemEventArgs ev)
    {
        if (_scp066role != null && _scp066role.Check(ev.Player))
        {
            ev.IsAllowed = false;
        }
    }
    
    /// <summary>
    /// Clearing the inventory if the SCP-066 dies
    /// </summary>
    private void OnPlayerDying(DyingEventArgs ev)
    {
        if (_scp066role != null && _scp066role.Check(ev.Player))
        {
            ev.Player.ClearInventory();
        }
    }
    
    /// <summary>
    /// Does not allow SCP-066 to turn on the warhead
    /// </summary>
    private void OnWarheadStart(StartingEventArgs ev)
    {
        if (_scp066role != null && _scp066role.Check(ev.Player))
        {
            ev.IsAllowed = false;
        }
    }
    
    /// <summary>
    /// Does not allow SCP-066 to turn off the warhead
    /// </summary>
    private void OnWarheadStop(StoppingEventArgs ev)
    {
        if (_scp066role != null && _scp066role.Check(ev.Player))
        {
            ev.IsAllowed = false;
        }
    }
    
    /// <summary>
    /// Does not add SCP-066 for SCP-096 to targets
    /// </summary>
    private void OnAddingTarget(AddingTargetEventArgs ev)
    {
        if (_scp066role != null && _scp066role.Check(ev.Player))
        {
            ev.IsAllowed = false;
        }
    }
    
    /// <summary>
    /// If the SCP-066 dies, then his original body should not appear
    /// </summary>
    private void OnSpawningRagdoll(SpawningRagdollEventArgs ev)
    {
        if (_scp066role != null && _scp066role.Check(ev.Player))
        {
            ev.IsAllowed = false;
        }
    }
    
    /// <summary>
    /// Does not allow SCP-106 to teleport SCP-066 to a pocket dimension
    /// </summary>
    private void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
    {
        if (_scp066role != null && _scp066role.Check(ev.Player))
        {
            ev.IsAllowed = false;
        }
    }

    /// <summary>
    /// Does not allow SCP-066 to take candies
    /// </summary>
    private void OnInteractingScp330(InteractingScp330EventArgs ev)
    {
        if (_scp066role != null && _scp066role.Check(ev.Player))
        {
            ev.IsAllowed = false;
        }
    }

    /// <summary>
    /// Update size from SCP-066 to new player
    /// </summary>
    /// <param name="ev"></param>
    private void OnVerified(VerifiedEventArgs ev)
    {
        if (ev.Player is null)
            return;
        
        if (_scp066role is null)
            return;
        
        foreach (Player scp066 in _scp066role.TrackedPlayers)
        {
            InvisibleManager.MakeInvisibleForPlayer(scp066, ev.Player);
        }
    }
    
    /// <summary>
    /// Spectators should see SCP-066 in the first person, unlike other players
    /// It works with a delay from the server
    /// </summary>
    private void OnChangingSpectatedPlayer(ChangingSpectatedPlayerEventArgs ev)
    {
        if (_scp066role is null)
            return;

        if (_scp066role.Check(ev.NewTarget))
        {
            InvisibleManager.RemoveInvisibleForPlayer(ev.NewTarget, ev.Player);
        }
        
        if (_scp066role.Check(ev.OldTarget))
        {
            InvisibleManager.MakeInvisibleForPlayer(ev.OldTarget, ev.Player);
        }
    }

    /// <summary>
    /// Allow SCP-066 to open checkpoints as SCP
    /// </summary>
    private void OnInteractionDoor(InteractingDoorEventArgs ev)
    {
        if (_scp066role is null)
            return;

        if (ev.Door.Type is DoorType.CheckpointLczA or DoorType.CheckpointLczB)
        {
            ev.Door.IsOpen = !ev.Door.IsOpen;
        }
    }
}