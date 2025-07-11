using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exiled.API.Features;
using Hints;
using Scp066.Features.Manager;
using Scp066.Interfaces;
using UnityEngine;

namespace Scp066.Features.Controller;
public class HintController : MonoBehaviour
{
    public void Init(Player player)
    {
        _player = player;
        _abilities = AbilityManager.GetAbilities.OrderBy(r => r.KeyId).ToList();
        _controller = player.GameObject.GetComponent<CooldownController>();
        InvokeRepeating(nameof(CheckHint), 0f, 0.5f);
        Log.Debug($"[CooldownController] Invoke the hint cycle");
    }
    
    void CheckHint()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<align=right>");
        stringBuilder.Append("<size=50><color=red><b>SCP-066</b></color></size>\n");
        stringBuilder.Append("<size=30><color=red>Eric's Toy play sounds</color></size>\n\n");
        stringBuilder.Append("Abilities:\n");

        int i = 0;
        foreach (var ability in _abilities)
        {
            string color = "red";
            if (!_controller.IsAbilityAvailable(ability.Name))
            {
                color = "#880000";
            }
                    
            stringBuilder.Append($"<color={color}>{ability.Name}  {{{i}}}</color>\n");
            i++;
        }
                
        stringBuilder.Append($"\n<size=18>if you cant use abilities\nremove \u2b50 in settings</size>");
        stringBuilder.Append("</align>\n\n\n\n\n\n\n\n");
        
        _player.HintDisplay.Show(new TextHint(stringBuilder.ToString(), [
            new SSKeybindHintParameter(660),
            new SSKeybindHintParameter(661),
            new SSKeybindHintParameter(662),
        ], durationScalar: 1f));
    }
    
    void OnDestroy()
    {
        CancelInvoke(nameof(CheckHint));
        Log.Debug($"[CooldownController] Cancel the hint cycle");
    }

    private Player _player;
    private List<IAbility> _abilities;
    private CooldownController _controller;
}