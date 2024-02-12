using Blish_HUD.Settings;
using System.Collections.Generic;

namespace HexedHero.Blish_HUD.FlipperHelper.Objects
{
    public class ModuleSettings
    {

        public SettingCollection SettingCollection { get; private set; }

        public List<SettingEntry> BehaviourSettings { get; private set; } = new List<SettingEntry>();
        public SettingEntry<bool> CloseWindowOnESC { get; private set; }
        public SettingEntry<bool> ResetOnWindowClosure { get; private set; }

        public ModuleSettings(SettingCollection settingsCollection)
        {

            this.SettingCollection = settingsCollection;

            // Behaviour settings
            BehaviourSettings.Add(
                CloseWindowOnESC = settingsCollection.DefineSetting(
                    nameof(CloseWindowOnESC),
                    true,
                    () => "Allow ESC Closure",
                    () => "Should the window close when you press ESC or not?\n\nWarning: Blish HUD's \"Close Window on Escape\" can override this."
                )
            );

            BehaviourSettings.Add(
                ResetOnWindowClosure = settingsCollection.DefineSetting(
                    nameof(ResetOnWindowClosure),
                    false,
                    () => "Reset on Closure",
                    () => "Should all calculator values be reset when the window is closed?"
                )
            );

        }

    }
}
