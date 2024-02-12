using Blish_HUD.Content;
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

        private Panel BehaviourSettingsPanel;
        private Image BehaviourSettingsPanelIcon;
        private FlowPanel BehaviourSettingsFlow;

        public ModuleSettingsView() { }

        protected override void Unload()
        {

            SettingFlowPanel?.Dispose();
            BehaviourSettingsPanel?.Dispose();
            BehaviourSettingsPanelIcon?.Dispose();
            BehaviourSettingsFlow?.Dispose();

        }

        protected override void Build(Container container)
        {

            // Full settings panel
            SettingFlowPanel = new FlowPanel()
            {

                FlowDirection = ControlFlowDirection.SingleTopToBottom,
                Size = new Point(260, 180),
                Parent = container

            };

            // Add behaviour settings
            AddBehaviourSettings(container);

        }

        private void AddBehaviourSettings(Container container)
        {

            // Behaviour settings panel
            BehaviourSettingsPanel = new Panel()
            {

                Title = "    Behaviour Settings",
                Size = new Point(250, 110),
                Location = new Point(0, 0),
                Collapsed = false,
                CanCollapse = false,
                ShowBorder = true,
                Parent = SettingFlowPanel,

            };

            BehaviourSettingsPanelIcon = new Image()
            {

                Location = new Point(12, 8),
                Size = new Point(20, 20),
                Texture = AsyncTexture2D.FromAssetId(1234928),
                Parent = container

            };

            // Behaviour settings inside
            BehaviourSettingsFlow = new FlowPanel()
            {

                FlowDirection = ControlFlowDirection.SingleTopToBottom,
                OuterControlPadding = new Vector2(5, 5),
                Size = new Point(215, 78),
                Parent = BehaviourSettingsPanel

            };

            // Add the settings into the panel
            List<SettingEntry> behaviourSettings = ModuleSettingsManager.Instance.ModuleSettings.BehaviourSettings;
            foreach (SettingEntry settingEntry in behaviourSettings)
            {

                IView settingView = SettingView.FromType(settingEntry, SettingFlowPanel.Width);
                // Check if it is renderable
                if (settingView != null)
                {

                    new ViewContainer()
                    {

                        Parent = BehaviourSettingsFlow

                    }.Show(settingView);

                }

            }

        }

    }

}