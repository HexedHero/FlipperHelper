using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using FlipperHelper.Utils;
using HexedHero.Blish_HUD.FlipperHelper.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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

        private AsyncTexture2D emblemTexture;
        private AsyncTexture2D calculatorTexture;
        private AsyncTexture2D settingsTexture;
        private AsyncTexture2D backgroundTexture;

        private WindowManager()
        {

            Load();

        }

        private void Load()
        {

            // Load needed textures
            iconTexture = FlipperHelper.Instance.Module.ContentsManager.GetTexture("779571_modified.png");
            emblemTexture = AsyncTexture2D.FromAssetId(779571);
            calculatorTexture = AsyncTexture2D.FromAssetId(156753);
            settingsTexture = AsyncTexture2D.FromAssetId(155052);
            backgroundTexture = AsyncTexture2D.FromAssetId(155985);

            // Make corner icon
            cornerIcon = new CornerIcon()
            {

                Icon = iconTexture,
                BasicTooltipText = "Flipper Helper",
                Priority = 1212374129,
                Parent = GameService.Graphics.SpriteScreen

            };

            cornerIcon.Click += delegate { MainWindow.ToggleWindow(); };

            // Make main window
            MainWindow = new TabbedWindow2(
                    ContentService.Textures.TransparentPixel, // See Below
                    new Rectangle(5, 5, 350, 240), // Window
                    new Rectangle(70, 30, 300, 180) // Content
                )
            {

                Emblem = ContentService.Textures.TransparentPixel, // See Below
                Title = "Flipper Helper",
                Location = new Point(100, 100),
                SavesPosition = true,
                Id = "FlipperHelperMainWindow",
                Parent = GameService.Graphics.SpriteScreen,
                CanCloseWithEscape = ModuleSettingsManager.Instance.ModuleSettings.CloseWindowOnESC.Value

            };

            // Add the background - Check if the texture was loaded by Blish or another module or this module at a different runtime else run the injection when it is loaded.
            void injectBackground() => Reflection.InjectNewBackground(MainWindow, backgroundTexture, new Rectangle(30, 30, 350, 350));
            if (backgroundTexture.HasSwapped) { injectBackground(); } else { backgroundTexture.TextureSwapped += delegate { injectBackground(); }; }

            // Add the Emblem - Emblem doesn't have AsyncTexture2D support so we need to set it later, same issue as the background
            void injectEmblem() => MainWindow.Emblem = emblemTexture;
            if (emblemTexture.HasSwapped) { injectEmblem(); } else { emblemTexture.TextureSwapped += delegate { injectEmblem(); }; }

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
            CalculatorView = null;
            ModuleSettingsView = null;
            SettingRedirectView = null;

            // Textures
            iconTexture?.Dispose();

            // Reset instance
            instance = null;

        }

    }

}
