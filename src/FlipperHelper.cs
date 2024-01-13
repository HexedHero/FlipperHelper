using System.ComponentModel.Composition;
using Blish_HUD.Modules;
using Blish_HUD.Settings;
using Blish_HUD.Graphics.UI;
using HexedHero.Blish_HUD.FlipperHelper.Managers;

namespace HexedHero.Blish_HUD.FlipperHelper
{

    [Export(typeof(Module))]
    public class FlipperHelper : Module
    {

        // Fake Singleton
        public static FlipperHelper Instance { get; private set; }
        public ModuleParameters Module { get; private set; }

        [ImportingConstructor]
        public FlipperHelper([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters)
        {

            // Init
            Module = moduleParameters;
            Instance = this;

        }

        protected override void DefineSettings(SettingCollection settings)
        {

            // Send the settings to the module settings manager
            ModuleSettingsManager.Instance.DefineSettings(settings);

        }

        protected override void Initialize()
        {

            // Load managers
            _ = WindowManager.Instance;
            _ = ModuleSettingsManager.Instance;

        }

        protected override void Unload()
        {

            // Unload windows
            WindowManager.Instance.Unload();
            ModuleSettingsManager.Instance.Unload();

            // Unload module instance
            Instance = null;

        }

        public override IView GetSettingsView()
        {

            // Give the user a redirection to the real settings
            return WindowManager.Instance.SettingRedirectView;

        }

    }

}