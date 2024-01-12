using Blish_HUD.Settings;

namespace HexedHero.Blish_HUD.FlipperHelper.Objects
{

    public class Settings
    {

        public SettingCollection SettingCollection { get; private set; }

        public SettingEntry<bool> CloseWindowOnESC { get; private set; }

        public Settings(SettingCollection settings)
        {

            SettingCollection = settings;

            // Set settings
            CloseWindowOnESC = settings.DefineSetting(
                    "CloseWindowOnESC",
                    true,
                    () => "Close On ESC",
                    () => "Should the window close when you press ESC or not?"
                );

        }

    }

}