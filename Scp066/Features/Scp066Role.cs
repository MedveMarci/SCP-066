using System.Collections.Generic;
using PlayerRoles;
using RoleAPI.API;
using RoleAPI.API.Configs;
using Scp066.Features.Abilities;
using UncomplicatedCustomRoles.API.Enums;
using UncomplicatedCustomRoles.API.Features;
using UncomplicatedCustomRoles.API.Features.Behaviour;
using UncomplicatedCustomRoles.API.Features.CustomModules;
using UncomplicatedCustomRoles.Manager;
using UnityEngine;

namespace Scp066.Features;

public class Scp066Role : ExtendedRole
{
    public override int Id { get; set; } = 066;
    public override string Name { get; set; } = "SCP-066";
    public override bool OverrideRoleName { get; set; } = true;
    public override string Nickname { get; set; } = null;
    public override string CustomInfo { get; set; } = "";
    public override string BadgeName { get; set; } = "";
    public override string BadgeColor { get; set; } = "";
    public override string SpawnHint { get; set; } = "";
    public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;
    public override RoleTypeId RoleAppearance { get; set; } = RoleTypeId.Scp0492;
    public override List<Team> IsFriendOf { get; set; } = [];

    public override HealthBehaviour Health { get; set; } = new()
    {
        Amount = 2500,
        Maximum = 2500
    };

    public override HumeShieldBehaviour HumeShield { get; set; } = new()
    {
        Amount = 1000,
        Maximum = 1000
    };

    public override List<Effect> Effects { get; set; } =
    [
        new()
        {
            EffectType = "Fade",
            Duration = -1,
            Intensity = 255,
            Removable = false
        },
        new()
        {
            EffectType = "Slowness",
            Duration = -1,
            Intensity = 25,
            Removable = false
        },
        new()
        {
            EffectType = "SilentWalk",
            Duration = -1,
            Intensity = 50,
            Removable = false
        }
    ];

    public override bool CanEscape { get; set; } = false;

    public override string SpawnBroadcast { get; set; } =
        "<color=red>\ud83c\udfb5 You are SCP-066 - Eric's Toy \ud83c\udfb5\n" +
        "Play sounds to kill humans\n" +
        "Use abilities by clicking on the buttons</color>";

    public override ushort SpawnBroadcastDuration { get; set; } = 15;
    public override List<ItemType> Inventory { get; set; } = [];
    public override Dictionary<ItemType, ushort> Ammo { get; set; } = [];
    public override float DamageMultiplier { get; set; } = 0;

    public override SpawnBehaviour SpawnSettings { get; set; } = new()
    {
        CanReplaceRoles =
        [
            RoleTypeId.Scp049, RoleTypeId.Scp079, RoleTypeId.Scp096, RoleTypeId.Scp106, RoleTypeId.Scp173,
            RoleTypeId.Scp939
        ],
        MinPlayers = 5,
        MaxPlayers = 1,
        SpawnChance = 10,
        Spawn = SpawnType.RoleSpawn,
        SpawnRoles = [RoleTypeId.Scp173]
    };

    public override SchematicConfig SchematicConfig { get; set; } = new()
    {
        SchematicName = "Scp066",
        Offset = new Vector3(0f, -1f, 0f),
        Rotation = new Vector3(0f, 90f, 0f)
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
        MaxDistance = 5f
    };

    public override AbilityConfig AbilityConfig { get; set; } = new()
    {
        AbilityTypes =
        [
            typeof(PlayEric),
            typeof(PlayNotes),
            typeof(PlayNoise)
        ]
    };

    public override void OnSpawned(SummonedCustomRole role)
    {
        role.AddModule(
            typeof(CustomScpAnnouncer),
            new Dictionary<string, object> { { "name", "SCP066" } }
        );
        base.OnSpawned(role);
    }
    
}