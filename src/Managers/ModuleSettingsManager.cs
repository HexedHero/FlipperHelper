using Blish_HUD.Settings;
using HexedHero.Blish_HUD.FlipperHelper.Objects;
using System;

namespace HexedHero.Blish_HUD.FlipperHelper.Managers
{
    public class ModuleSettingsManager
    {

        // Singleton
        private static Lazy<ModuleSettingsManager> instance = new Lazy<ModuleSettingsManager>(() => new ModuleSettingsManager());
        public static ModuleSettingsManager Instance {
            get {
                if (instance == null) {
                    instance = new Lazy<ModuleSettingsManager>(() => new ModuleSettingsManager());
                }
                return instance.Value;
            }
        }

        public SettingCollection Settings { get; private set; }
        public ModuleSettings ModuleSettings { get; private set; }

        private ModuleSettingsManager() { }

        public void Load() { }

        public void Unload()
        {

            instance = null;

        }

        public void DefineSettings(SettingCollection settings)
        {

            ModuleSettings = new ModuleSettings(settings);
            ModuleSettings.CloseWindowOnESC.SettingChanged += delegate
            {

                WindowManager.Instance.MainWindow.CanCloseWithEscape = ModuleSettings.CloseWindowOnESC.Value;

            };

        }

    }

}
