using System.ComponentModel;
using Exiled.API.Interfaces;
using Scp066.Features;
using UnityEngine;

namespace Scp066.Configs;

public class Config : IConfig
{
    [Description("Whether or not is the plugin enabled?")]
    public bool IsEnabled { get; set; } = true;

    [Description("Whether or not is the plugin is in debug mode?")]
    public bool Debug { get; set; } = false;
    
    [Description("The volume of all the audio files.")]
    public byte Volume { get; set; } = 100;
    
    [Description("Maximum range of SCP-999 abilities")]
    public float MaxDistance { get; set; } = 10;
    
    [Description("The minimum players required to spawn SCP-999")]
    public int MinimumPlayers { get; set; } = 5;
    
    [Description("The maximum players required to spawn SCP-999")]
    public int MaximumPlayers { get; set; } = 15;
    
    [Description("The name of the schematic that will be used as a game model")]
    public string SchematicName { get; set; } = "Scp066";
    
    [Description("Offset the position of the schematic relative to the player")]
    public Vector3 SchematicOffset { get; set; } = new Vector3(0f, -0.71f, 0f);

    [Description("Configs for the SCP-066 role players turn into")]
    public Scp066Role Scp066RoleConfig { get; set; } = new();
}