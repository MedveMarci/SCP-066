using System;
using System.Linq;
using System.Text;
using Exiled.API.Features;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Utilities;
using Scp066.Features.Controller;
using Scp066.Interfaces;
using Hint = HintServiceMeow.Core.Models.Hints.Hint;

namespace Scp066.Features.Manager;
public static class HintManager
{
    public static void AddHint(Player player)
    {
        try
        {
            var abilityList = AbilityManager.GetAbilities.OrderBy(r => r.KeyId);
        
            Hint hint = new Hint
            {
                Id = "066",
                AutoText = arg =>
                {
                    var controller = player.GameObject.GetComponent<CooldownController>();
                    StringBuilder stringBuilder = new StringBuilder();
                
                    stringBuilder.Append("<size=50><color=red><b>SCP-066</b></color></size>\n");
                    stringBuilder.Append("<size=30><color=red>Eric's Toy play sounds</color></size>\n\n");
                    stringBuilder.Append("Abilities:\n");
                
                    foreach (IAbility ability in abilityList)
                    {
                        string color = "red";
                        if (!controller.IsAbilityAvailable(ability.Name))
                        {
                            color = "#880000";
                        }
                    
                        stringBuilder.Append($"<color={color}>{ability.Name}  [{ability.KeyCode}]</color>\n");
                    }
                
                    stringBuilder.Append($"\n<size=18><color=red>Kill the players with Noise</color></size>");
                    return stringBuilder.ToString();
                },
                FontSize = 35,
                YCoordinate = 500,
                Alignment = HintAlignment.Right,
                SyncSpeed = HintSyncSpeed.Normal,
            };

            PlayerDisplay playerDisplay = PlayerDisplay.Get(player);
            playerDisplay.AddHint(hint);
        }
        catch (Exception ex)
        {
            Log.Error($"An error occurred when adding hint to player: {ex}");
        }
    }

    public static void RemoveHint(Player player)
    {
        PlayerDisplay playerDisplay = PlayerDisplay.Get(player);
        playerDisplay.RemoveHint("066");
    }
}