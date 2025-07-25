using System.ComponentModel;
using Exiled.API.Interfaces;
using Scp066.Features;

namespace Scp066.Configs;

public class Config : IConfig
{
    [Description("Whether or not is the plugin enabled?")]
    public bool IsEnabled { get; set; } = true;

    [Description("Whether or not is the plugin is in debug mode?")]
    public bool Debug { get; set; } = false;
    
    [Description("Maximum range of SCP-066 abilities")]
    public float Distance { get; set; } = 5;
    
    [Description("How much damage should SCP-066 do?")]
    public float Damage { get; set; } = 8;

    [Description("Can SCP-066 destroy windows with its Noise ability?")]
    public bool IsBreakableWindows { get; set; } = true;

    [Description("Configs for the SCP-066 role players turn into")]
    public Scp066Role Scp066RoleConfig { get; set; } = new();
}