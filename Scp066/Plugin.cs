using System;
using Exiled.API.Features;
using Exiled.CustomRoles.API;
using Scp066.Configs;

namespace Scp066;
public class Plugin : Plugin<Config>
{
    public override string Name => "Scp066";
    public override string Author => "RisottoMan";
    public override Version Version => new(1, 3, 0);
    public override Version RequiredExiledVersion => new(9, 6, 0);
    
    public static Plugin Singleton;
    public override void OnEnabled()
    {
        Singleton = this;
        
        // Setup the RoleAPI
        if (!RoleAPI.Startup.SetupAPI(this.Name))
            return;
        
        // Register the custom role
        Config.Scp066RoleConfig.Register();
        
        new EventHandler();
        
        base.OnEnabled();
    }
}