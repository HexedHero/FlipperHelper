using Blish_HUD.Controls;
using Blish_HUD;
using HexedHero.Blish_HUD.FlipperHelper.Objects;
using HexedHero.Blish_HUD.FlipperHelper.Utils;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HexedHero.Blish_HUD.FlipperHelper.Managers
{
    public class WindowManager
    {

        // Singleton
        private static Lazy<WindowManager> instance = new Lazy<WindowManager>(() => new WindowManager());
        public static WindowManager Instance {
            get {
                if (instance == null) {
                    instance = new Lazy<WindowManager>(() => new WindowManager());
                }
                return instance.Value;
            }
        }

        public TabbedWindow2 MainWindow { get; private set; }
        public CalculatorView CalculatorView { get; private set; }
        public ModuleSettingsView ModuleSettingsView { get; private set; }
        public SettingRedirectView SettingRedirectView { get; private set; }

        private CornerIcon cornerIcon;

        private Texture2D iconTexture;
        private Texture2D emblemTexture;
        private Texture2D calculatorTexture;
        private Texture2D settingsTexture;
        private Texture2D backgroundTexture;
        private Texture2D backgroundTextureResized;

        private WindowManager() { }

        public void Load()
        {

            // Load needed textures
            iconTexture = FlipperHelper.Instance.Module.ContentsManager.GetTexture("779571_modified.png");
            emblemTexture = FlipperHelper.Instance.Module.ContentsManager.GetTexture("779571.png");
            calculatorTexture = FlipperHelper.Instance.Module.ContentsManager.GetTexture("156753.png");
            settingsTexture = FlipperHelper.Instance.Module.ContentsManager.GetTexture("155052.png");
            backgroundTexture = FlipperHelper.Instance.Module.ContentsManager.GetTexture("155985.png");

            // Make corner icon
            cornerIcon = new CornerIcon()
            {

                Icon = iconTexture,
                BasicTooltipText = "FlipperHelper",
                Priority = 1212374129,
                Parent = GameService.Graphics.SpriteScreen

            };

            cornerIcon.Click += delegate { MainWindow.ToggleWindow(); };

            // Make main window
            backgroundTextureResized = Common.ResizeTexture2D(backgroundTexture, (int)(backgroundTexture.Width / 2.7), (int)(backgroundTexture.Height / 3.1));
            MainWindow = new TabbedWindow2(backgroundTextureResized,
                new Rectangle(5, 5, (int)(backgroundTextureResized.Width * 0.935), (int)(backgroundTextureResized.Height * 0.725)),
                new Rectangle(76, 30, (int)(backgroundTextureResized.Width * 0.78), (int)(backgroundTextureResized.Height * 0.55)))
            {

                Emblem = emblemTexture,
                Title = "Flipper Helper",
                Location = new Point(100, 100),
                SavesPosition = true,
                Id = "FlipperHelperMainWindow",
                Parent = GameService.Graphics.SpriteScreen,
                CanCloseWithEscape = ModuleSettingsManager.Instance.ModuleSettings.CloseWindowOnESC.Value

            };

            // Add tabs
            MainWindow.Tabs.Add(new Tab(calculatorTexture, () => CalculatorView = new CalculatorView(), "Calculator", 1));
            MainWindow.Tabs.Add(new Tab(settingsTexture, () => ModuleSettingsView = new ModuleSettingsView(), "Settings", 2));

            // Close event
            MainWindow.Hidden += delegate
            {

                if (ModuleSettingsManager.Instance.ModuleSettings.ResetOnWindowClosure.Value)
                {

                    CalculatorView.ResetInformation();

                }

            };

            // Add redirect
            SettingRedirectView = new SettingRedirectView();

        }

        public void Unload()
        {

            // Icon
            cornerIcon?.Dispose();

            // Windows
            MainWindow?.Dispose();
            CalculatorView?.DoUnload();
            ModuleSettingsView?.DoUnload();
            SettingRedirectView?.DoUnload();

            // Textures
            iconTexture?.Dispose();
            emblemTexture?.Dispose();
            calculatorTexture?.Dispose();
            settingsTexture?.Dispose();
            backgroundTexture?.Dispose();
            backgroundTextureResized?.Dispose();

        }

    }

}
