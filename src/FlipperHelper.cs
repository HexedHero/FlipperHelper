using System.ComponentModel.Composition;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HexedHero.Blish_HUD.FlipperHelper.Utils;
using HexedHero.Blish_HUD.FlipperHelper.Objects;
using Blish_HUD.Settings;
using Blish_HUD.Graphics.UI;
using Blish_HUD.Settings.UI.Views;

namespace HexedHero.Blish_HUD.FlipperHelper
{

    [Export(typeof(Module))]
    public class FlipperHelper : Module
    {

        internal static FlipperHelper Instance;

        internal SettingsManager SettingsManager => this.ModuleParameters.SettingsManager;
        internal ContentsManager ContentsManager => this.ModuleParameters.ContentsManager;
        internal DirectoriesManager DirectoriesManager => this.ModuleParameters.DirectoriesManager;
        internal Gw2ApiManager Gw2ApiManager => this.ModuleParameters.Gw2ApiManager;

        public SettingCollection Settings { get; private set; }
        public ModuleSettings ModuleSettings { get; private set; }
        public TabbedWindow2 MainWindow { get; private set; }

        private CornerIcon cornerIcon;

        private Texture2D iconTexture;
        private Texture2D emblemTexture;
        private Texture2D calculatorTexture;
        private Texture2D settingsTexture;
        private Texture2D backgroundTexture;
        private Texture2D backgroundTextureResized;

        [ImportingConstructor]
        public FlipperHelper([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters)
        {

            Instance = this;
        
        }

        protected override void DefineSettings(SettingCollection settings)
        {

            ModuleSettings = new ModuleSettings(settings);
            ModuleSettings.CloseWindowOnESC.SettingChanged += delegate
            {

                MainWindow.CanCloseWithEscape = ModuleSettings.CloseWindowOnESC.Value;

            };

        }

        protected override void Initialize()
        {

            // Load needed textures
            iconTexture = ContentsManager.GetTexture("779571_modified.png");
            emblemTexture = ContentsManager.GetTexture("779571.png");
            calculatorTexture = ContentsManager.GetTexture("156753.png");
            settingsTexture = ContentsManager.GetTexture("155052.png");
            backgroundTexture = ContentsManager.GetTexture("155985.png");

            // Make corner icon
            cornerIcon = new CornerIcon()
            {

                Icon = iconTexture,
                BasicTooltipText = $"{Name}",
                Priority = 1212374129,
                Parent = GameService.Graphics.SpriteScreen

            };

            cornerIcon.Click += delegate { MainWindow.ToggleWindow(); };

            // Make main window
            backgroundTextureResized = Common.ResizeTexture2D(backgroundTexture, (int) (backgroundTexture.Width / 2.7), (int) (backgroundTexture.Height / 3.1));
            MainWindow = new TabbedWindow2(backgroundTextureResized,
                new Rectangle(5, 5, (int) (backgroundTextureResized.Width * 0.935), (int) (backgroundTextureResized.Height * 0.725)),
                new Rectangle(76, 30, (int) (backgroundTextureResized.Width * 0.78), (int) (backgroundTextureResized.Height * 0.55)))
            {

                Emblem = emblemTexture,
                Title = "Flipper Helper",
                Location = new Point(100, 100),
                SavesPosition = true,
                Id = "FlipperHelperMainWindow",
                Parent = GameService.Graphics.SpriteScreen,
                CanCloseWithEscape = ModuleSettings.CloseWindowOnESC.Value

            };
            
            // Add tabs
            MainWindow.Tabs.Add(new Tab(calculatorTexture, () => new CalculatorView(), "Calculator", 1));
            MainWindow.Tabs.Add(new Tab(settingsTexture, () => new ModuleSettingsView(), "Settings", 2));

        }

        public override IView GetSettingsView()
        {

            return new SettingRedirectView();

        }

        protected override void Unload()
        {
            
            cornerIcon?.Dispose();
            MainWindow?.Dispose();
            iconTexture?.Dispose();
            emblemTexture?.Dispose();
            backgroundTexture?.Dispose();
            backgroundTextureResized?.Dispose();
            Instance = null;

        }

    }

}