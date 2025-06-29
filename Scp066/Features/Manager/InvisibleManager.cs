using Exiled.API.Features;
using Exiled.API.Features.Roles;

namespace Scp066.Features.Manager;
public static class InvisibleManager
{
    /// <summary>
    /// Make a specific SCP-066 invisible to all players
    /// </summary>
    /// <param name="scp066">A player with the role of SCP-066</param>
    public static void MakeInvisible(Player scp066)
    {
        foreach (Player other in Player.List)
        {
            if (scp066 == other)
                continue;
            
            if (scp066.Role.Is(out FpcRole fpc))
            {
                fpc.IsInvisibleFor.Add(other);
            }
        }
    }
    
    /// <summary>
    /// Make a specific SCP-066 invisible for a specific player
    /// </summary>
    /// <param name="scp066">A player with the role of SCP-066</param>
    /// <param name="player">The player who shouldn't see SCP-066</param>
    public static void MakeInvisibleForPlayer(Player scp066, Player player)
    {
        if (scp066.Role.Is(out FpcRole fpc))
        {
            fpc.IsInvisibleFor.Add(player);
        }
    }

    /// <summary>
    /// Remove the invisibility of a specific SCP-066 for all players
    /// </summary>
    /// <param name="scp066">A player with the role of SCP-066</param>
    public static void RemoveInvisible(Player scp066)
    {
        if (scp066.Role.Is(out FpcRole fpc))
        {
            foreach (Player player in fpc.IsInvisibleFor)
            {
                fpc.IsInvisibleFor.Remove(player);
            }
        }
    }
    
    /// <summary>
    /// Remove the invisibility of a specific SCP-066 for a specific player
    /// </summary>
    /// <param name="scp066">A player with the role of SCP-066</param>
    /// <param name="player">The player who should see SCP-066</param>
    public static void RemoveInvisibleForPlayer(Player scp066, Player player)
    {
        if (scp066.Role.Is(out FpcRole fpc))
        {
            fpc.IsInvisibleFor.Remove(player);
        }
    }
}