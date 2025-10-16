using LabApi.Events.Arguments.Scp0492Events;
using LabApi.Events.CustomHandlers;
using UncomplicatedCustomRoles.Extensions;

namespace Scp066;

public class EventHandler : CustomEventsHandler
{
    public override void OnScp0492StartingConsumingCorpse(Scp0492StartingConsumingCorpseEventArgs ev)
    {
       if (ev.Player.TryGetSummonedInstance(out var role) && role.Role.Id == 066)
           ev.IsAllowed = false;
    }
}