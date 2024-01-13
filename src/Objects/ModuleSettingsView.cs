using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace HexedHero.Blish_HUD.FlipperHelper.Objects
{

    public class ModuleSettingsView : View
    {

        public ModuleSettingsView() { }

        protected override void Build(Container container)
        {

            // Full settings panel
            FlowPanel settingFlowPanel = new FlowPanel()
            {

                CanScroll = true,
                FlowDirection = ControlFlowDirection.SingleTopToBottom,
                Size = new Point(260, 180),
                Parent = container

            };

            // Close settings panel
            Panel closeSettingsPanel = new Panel()
            {

                Title = "Close Behaviour",
                Size = new Point(225, 125),
                Location = new Point(0, 0),
                Collapsed = true,
                CanCollapse = true,
                ShowBorder = true,
                Parent = settingFlowPanel

            };

            // Close settings inside
            FlowPanel closeSettingsFlow = new FlowPanel()
            {

                CanScroll = true,
                FlowDirection = ControlFlowDirection.SingleTopToBottom,
                Size = new Point(215, 78),
                Parent = closeSettingsPanel

            };

            // Add the settings into the panel
            List<SettingEntry> closeBehaviourSettings = FlipperHelper.Instance.ModuleSettings.CloseBehaviourSettings;
            foreach (SettingEntry settingEntry in closeBehaviourSettings)
            {

                IView settingView = SettingView.FromType(settingEntry, settingFlowPanel.Width);
                // Check if it is renderable
                if (settingView != null)
                {

                    new ViewContainer()
                    {

                        Parent = closeSettingsFlow

                    }.Show(settingView);

                }

            }

        }

    }

}