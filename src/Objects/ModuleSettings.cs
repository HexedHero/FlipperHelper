using Blish_HUD.Settings;
using System.Collections.Generic;

namespace HexedHero.Blish_HUD.FlipperHelper.Objects
{
    public class ModuleSettings
    {

        public SettingCollection SettingCollection { get; private set; }

        public List<SettingEntry> CloseBehaviourSettings { get; private set; } = new List<SettingEntry>();
        public SettingEntry<bool> CloseWindowOnESC { get; private set; }
        public SettingEntry<bool> ResetOnWindowClosure { get; private set; }

        public ModuleSettings(SettingCollection settingsCollection)
        {

            this.SettingCollection = settingsCollection;

            // Close behaviour settings
            CloseBehaviourSettings.Add(
                CloseWindowOnESC = settingsCollection.DefineSetting(
                    nameof(CloseWindowOnESC),
                    true,
                    () => "Allow ESC Closure",
                    () => "Should the window close when you press ESC or not?"
                )
            );

            CloseBehaviourSettings.Add(
                ResetOnWindowClosure = settingsCollection.DefineSetting(
                    nameof(ResetOnWindowClosure),
                    false,
                    () => "Reset on Closure",
                    () => "Should all values be reset when the window is closed?"
                )
            );

        }

    }
}
