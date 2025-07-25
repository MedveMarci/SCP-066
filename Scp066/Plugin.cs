using System;
using System.Linq;
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
        new EventHandler();

        // Checking that the ProjectMER plugin is loaded on the server
        if (!AppDomain.CurrentDomain.GetAssemblies().Any(x => x.FullName.ToLower().Contains("projectmer")))
        {
            Log.Error("ProjectMER is not installed. Schematics can't spawn the game model.");
            return;
        }
        
        // Register the custom role
        Config.Scp066RoleConfig.Register();
        
        // Setup the RoleAPI
        RoleAPI.Startup.SetupAPI(this.Name);
        
        // Register the abilities
        RoleAPI.API.Managers.AbilityRegistrator.RegisterAbilities();
        RoleAPI.API.Managers.KeybindManager.RegisterKeybinds(
            RoleAPI.API.Managers.AbilityRegistrator.GetAbilities,
            "SCP-066");

        base.OnEnabled();
    }
}