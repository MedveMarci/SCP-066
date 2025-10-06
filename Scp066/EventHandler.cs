using LabApi.Events.Arguments.Scp0492Events;
using UncomplicatedCustomRoles.Extensions;

namespace Scp066;

public static class EventHandler
{
    public static void OnStartingConsumingCorpse(Scp0492StartingConsumingCorpseEventArgs ev)
    {
       if (ev.Player.TryGetSummonedInstance(out var role) && role.Role.Id == 066)
           ev.IsAllowed = false;
    }
}