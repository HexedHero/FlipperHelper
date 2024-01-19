using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings;
using Blish_HUD.Settings.UI.Views;
using HexedHero.Blish_HUD.FlipperHelper.Managers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace HexedHero.Blish_HUD.FlipperHelper.Objects
{

    public class ModuleSettingsView : View
    {

        private FlowPanel SettingFlowPanel;
        private Panel CloseSettingsPanel;
        private FlowPanel CloseSettingsFlow;

        public ModuleSettingsView() { }

        protected override void Unload()
        {

            SettingFlowPanel?.Dispose();
            CloseSettingsPanel?.Dispose();
            CloseSettingsFlow?.Dispose();

        }

        protected override void Build(Container container)
        {

            // Full settings panel
            SettingFlowPanel = new FlowPanel()
            {

                CanScroll = true,
                FlowDirection = ControlFlowDirection.SingleTopToBottom,
                Size = new Point(260, 180),
                Parent = container

            };

            // Add close behaviour settings
            LoadCloseBehaviourSettings(container);

        }

        private void LoadCloseBehaviourSettings(Container container)
        {

            // Close settings panel
            CloseSettingsPanel = new Panel()
            {

                Title = "Close Behaviour",
                Size = new Point(225, 125),
                Location = new Point(0, 0),
                Collapsed = true,
                CanCollapse = true,
                ShowBorder = true,
                Parent = SettingFlowPanel

            };

            // Close settings inside
            CloseSettingsFlow = new FlowPanel()
            {

                CanScroll = true,
                FlowDirection = ControlFlowDirection.SingleTopToBottom,
                Size = new Point(215, 78),
                Parent = CloseSettingsPanel

            };

            // Add the settings into the panel
            List<SettingEntry> closeBehaviourSettings = ModuleSettingsManager.Instance.ModuleSettings.CloseBehaviourSettings;
            foreach (SettingEntry settingEntry in closeBehaviourSettings)
            {

                IView settingView = SettingView.FromType(settingEntry, SettingFlowPanel.Width);
                // Check if it is renderable
                if (settingView != null)
                {

                    new ViewContainer()
                    {

                        Parent = CloseSettingsFlow

                    }.Show(settingView);

                }

            }

        }

    }

}