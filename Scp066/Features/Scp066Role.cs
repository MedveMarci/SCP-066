using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Spawn;
using PlayerRoles;
using RoleAPI.API;
using RoleAPI.API.Configs;
using UnityEngine;

namespace Scp066.Features;
public class Scp066Role : ExtendedRole
{
    public override string Name { get; set; } = "SCP-066";
    public override string Description { get; set; } = "Eric's Toy";
    public override string CustomInfo { get; set; } = "SCP-066";
    public override uint Id { get; set; } = 660;
    public override int MaxHealth { get; set; } = 3500;
    public override SpawnProperties SpawnProperties { get; set; } = new()
    {
        Limit = 1,
        DynamicSpawnPoints = [new DynamicSpawnPoint { Chance = 100, Location = SpawnLocationType.Inside173Gate }]
    };
    
    public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;
    
    public override Exiled.API.Features.Broadcast Broadcast { get; set; } = new()
    {
        Show = true,
        Content = 
            "<color=red>\ud83c\udfb5 You are SCP-066 - Eric's Toy \ud83c\udfb5\n" +
            "Play sounds to kill humans\n" +
            "Use abilities by clicking on the buttons</color>",
        Duration = 15
    };
    
    public override string ConsoleMessage { get; set; } =
        "You are SCP-066 - Eric's Toy!\n" +
        "Play sounds to kill humans\n" +
        "Configure your buttons in the settings. Remove the stars.";

    public override string CustomDeathText { get; set; } = "<color=red>The subject expired after exposure to a loud sound by SCP-066</color>";
    
    public override SpawnConfig SpawnConfig { get; set; } = new()
    {
        MinPlayers = 10,
        SpawnChance = 50
    };
    
    public override SchematicConfig SchematicConfig { get; set; } = new()
    {
        SchematicName = "Scp066",
        Offset = new Vector3(0f, -1f, 0f),
    };
    
    public override TextToyConfig TextToyConfig { get; set; } = new()
    {
        Text = "<color=red>SCP-066</color>",
        Offset = new Vector3(0, 1, 0),
        Rotation = new Vector3(0, 180, 0),
        Scale = new Vector3(0.2f, 0.2f, 0.2f),
    };

    public override HintConfig HintConfig { get; set; } = new()
    {
        Text = "<align=right><size=50><color=red><b>SCP-066</b></color></size>\n" +
               "<size=30><color=red>Eric's Toy play sounds</color></size>\n\n" + 
               "Abilities:\n" + 
               "<color=%color%>\ud83c\udfb5 Eric? {0}</color>\n" + 
               "<color=%color%>\ud83c\udfb6 Note {1}</color>\n" + 
               "<color=%color%>\ud83c\udfba Noise {2}</color>\n" + 
               "\n<size=18>if you cant use abilities\nremove \u2b50 in settings</size></align>",
        AvailableAbilityColor = "red",
        UnavailableAbilityColor = "#880000"
    };
    
    public override AudioConfig AudioConfig { get; set; } = new()
    {
        Name = "scp066",
        Volume = 100,
        IsSpatial = true,
        MinDistance = 5f,
        MaxDistance = 15f
    };
    
    public override List<EffectConfig> Effects { get; set; } =
    [
        new EffectConfig()
        {
            EffectType = EffectType.Disabled,
        },

        new EffectConfig()
        {
            EffectType = EffectType.Slowness,
            Intensity = 50
        },
        
        new EffectConfig()
        {
            EffectType = EffectType.SilentWalk,
            Intensity = 50
        }
    ];
    
    public override bool IsPlayerInvisible { get; set; } = true;
    public override bool IsShowPlayerNickname { get; set; } = true;
}