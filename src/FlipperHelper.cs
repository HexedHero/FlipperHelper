using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HexedHero.Blish_HUD.FlipperHelper.Enums;
using HexedHero.Blish_HUD.FlipperHelper.Utils;
using HexedHero.Blish_HUD.FlipperHelper.Objects;
using Blish_HUD.Settings;

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

        public Settings Settings { get; private set; }

        private CornerIcon cornerIcon;
        private StandardWindow flipWindow;

        private Texture2D iconTexture;
        private Texture2D emblemTexture;
        private Texture2D backgroundTexture;
        private Texture2D backgroundTextureResized;
        private Texture2D coinGoldTexture;
        private Texture2D coinSilverTexture;
        private Texture2D coinCopperTexture;

        private TextBox goldBuyTextBox;
        private TextBox silverBuyTextBox;
        private TextBox copperBuyTextBox;
        private TextBox goldSellTextBox;
        private TextBox silverSellTextBox;
        private TextBox copperSellTextBox;

        private Label buyLabel;
        private Label sellLabel;
        private Label profitLabel;
        private Label goldProfitLabel;
        private Label silverProfitLabel;
        private Label copperProfitLabel;

        private StandardButton resetButton;

        private readonly List<IDisposable> disposableBin = new List<IDisposable>();

        [ImportingConstructor]
        public FlipperHelper([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters)
        {

            Instance = this;
        
        }

        protected override void DefineSettings(SettingCollection settings)
        {

            Settings = new Settings(settings);
            Settings.CloseWindowOnESC.SettingChanged += delegate
            {

                flipWindow.CanCloseWithEscape = Settings.CloseWindowOnESC.Value;

            };

        }

        protected override void Initialize()
        {

            // Load needed textures
            iconTexture = ContentsManager.GetTexture("779571_modified.png");
            emblemTexture = ContentsManager.GetTexture("779571.png");
            backgroundTexture = ContentsManager.GetTexture("155985.png");
            coinGoldTexture = ContentsManager.GetTexture("coin_gold.png");
            coinSilverTexture = ContentsManager.GetTexture("coin_silver.png");
            coinCopperTexture = ContentsManager.GetTexture("coin_copper.png");

            // Make corner icon
            cornerIcon = new CornerIcon()
            {

                Icon = iconTexture,
                BasicTooltipText = $"{Name}",
                Priority = 1212374129,
                Parent = GameService.Graphics.SpriteScreen

            };

            cornerIcon.Click += delegate { flipWindow.ToggleWindow(); };

            // Make window
            backgroundTextureResized = Common.ResizeTexture2D(backgroundTexture, (int) (backgroundTexture.Width / 3.10), (int) (backgroundTexture.Height / 3.1));
            flipWindow = new StandardWindow(backgroundTextureResized,
                new Rectangle(5, 5, (int) (backgroundTextureResized.Width * 0.935), (int) (backgroundTextureResized.Height * 0.725)),
                new Rectangle(30, 30, (int) (backgroundTextureResized.Width * 0.78), (int) (backgroundTextureResized.Height * 0.55)))
            {

                Emblem = emblemTexture,
                Title = "Flipper Helper",
                Location = new Point(100, 100),
                SavesPosition = true,
                Id = "FlipperHelperCalculateWindow",
                Parent = GameService.Graphics.SpriteScreen

            };

            // Buy row
            buyLabel = new Label()
            {

                Text = "Buy Price:",
                Size = new Point(75, 18),
                Location = new Point(10, 0),
                Parent = flipWindow

            };

            CreateCoinControl(ECoinType.GOLD, coinGoldTexture, 10, 20, ref goldBuyTextBox);
            CreateCoinControl(ECoinType.SILVER, coinSilverTexture, 110, 20, ref silverBuyTextBox);
            CreateCoinControl(ECoinType.COPPER, coinCopperTexture, 185, 20, ref copperBuyTextBox);

            // Sell row
            sellLabel = new Label()
            {

                Text = "Sell Price:",
                Size = new Point(75, 18),
                Location = new Point(10, 55),
                Parent = flipWindow,
                ShowShadow = true

            };

            CreateCoinControl(ECoinType.GOLD, coinGoldTexture, 10, 75, ref goldSellTextBox);
            CreateCoinControl(ECoinType.SILVER, coinSilverTexture, 110, 75, ref silverSellTextBox);
            CreateCoinControl(ECoinType.COPPER, coinCopperTexture, 185, 75, ref copperSellTextBox);

            // Profit row
            profitLabel = new Label()
            {

                Text = "Profit:",
                Size = new Point(75, 18),
                Location = new Point(10, 110),
                Parent = flipWindow,
                ShowShadow = true

            };

            CreateCoinDisplay(ECoinType.GOLD, coinGoldTexture, 10, 130, ref goldProfitLabel);
            CreateCoinDisplay(ECoinType.SILVER, coinSilverTexture, 110, 130, ref silverProfitLabel);
            CreateCoinDisplay(ECoinType.COPPER, coinCopperTexture, 185, 130, ref copperProfitLabel);

            // Reset button
            resetButton = new StandardButton()
            {

                Text = "Reset",
                Size = new Point(50, 25),
                Location = new Point(10, 160),
                Parent = flipWindow,
                BasicTooltipText = "Reset all the information."

            };

            resetButton.Click += delegate
            {

                goldBuyTextBox.Text = string.Empty;
                silverBuyTextBox.Text = string.Empty;
                copperBuyTextBox.Text = string.Empty;
                goldSellTextBox.Text = string.Empty;
                silverSellTextBox.Text = string.Empty;
                copperSellTextBox.Text = string.Empty;

            };

        }

        private void CreateCoinControl(ECoinType coinType, Texture2D coinTexture, int x, int y, ref TextBox textBox)
        {

            disposableBin.Add(new Image(coinTexture)
            {

                Size = new Point(18, 18),
                Location = new Point(x, y + 5),
                Parent = flipWindow

            });

            textBox = new TextBox()
            {

                PlaceholderText = "0",
                MaxLength = (coinType == ECoinType.GOLD) ? 5 : 2,
                Size = new Point((coinType == ECoinType.GOLD) ? 65 : 40, 25),
                Font = GameService.Content.DefaultFont16,
                Location = new Point(x + 25, y),
                Parent = flipWindow

            };

            TextBox localTextBox = textBox;
            disposableBin.Add(localTextBox);
            textBox.TextChanged += delegate { UpdateInformation(localTextBox); }; // Yuck

        }

        private void CreateCoinDisplay(ECoinType coinType, Texture2D coinTexture, int x, int y, ref Label label)
        {

            disposableBin.Add(new Image(coinTexture)
            {

                Size = new Point(18, 18),
                Location = new Point(x, y + 5),
                Parent = flipWindow

            });

            label = new Label()
            {

                Text = "0",
                Size = new Point((coinType == ECoinType.GOLD) ? 65 : 40, 25),
                Font = GameService.Content.DefaultFont16,
                Location = new Point(x + 35, y),
                Parent = flipWindow

            };

        }

        private void UpdateInformation(TextBox textBox)
        {

            if (textBox.Text.All(char.IsDigit) || textBox.Text == string.Empty)
            {

                // Remove leading zeros
                if (textBox.Text.Length > 1)
                {

                    textBox.Text = textBox.Text.TrimStart('0');
                    if (textBox.Text.Length == 0)
                    {

                        textBox.Text = "0";

                    }

                }

                // Get buy values
                int goldBuyValue = goldBuyTextBox.Text.Length == 0 ? 0 : int.Parse(goldBuyTextBox.Text);
                int silverBuyValue = silverBuyTextBox.Text.Length == 0 ? 0 : int.Parse(silverBuyTextBox.Text);
                int copperBuyValue = copperBuyTextBox.Text.Length == 0 ? 0 : int.Parse(copperBuyTextBox.Text);
                int buyValue = (goldBuyValue * 10000) + (silverBuyValue * 100) + copperBuyValue;

                // Get sell values
                int goldSellValue = goldSellTextBox.Text.Length == 0 ? 0 : int.Parse(goldSellTextBox.Text);
                int silverSellValue = silverSellTextBox.Text.Length == 0 ? 0 : int.Parse(silverSellTextBox.Text);
                int copperSellValue = copperSellTextBox.Text.Length == 0 ? 0 : int.Parse(copperSellTextBox.Text);
                int sellValue = (goldSellValue * 10000) + (silverSellValue * 100) + copperSellValue;

                // Get the raw profit
                int difference = sellValue - buyValue;

                Boolean isValidSale = (goldBuyValue != 0 || silverBuyValue != 0 || copperBuyValue != 0) && (goldSellValue != 0 || silverSellValue != 0 || copperSellValue != 0);

                // Get the fees for selling
                int listingFee = !isValidSale ? 0 : Math.Max(1, (int) Math.Round(sellValue * 0.05)); // 5% with a minimum of 1 copper and rounds up
                int saleFee = !isValidSale ? 0 : Math.Max(1, (int) Math.Round(sellValue * 0.10)); // 10% with a minimum of 1 copper and rounds up

                // Get the actual profit
                int profit = difference - (listingFee + saleFee);

                // Convert the profit into gold/silver/copper
                int gold = profit / 10000;
                int silver = (profit % 10000) / 100;
                int copper = profit % 100;

                // Update the displays
                goldProfitLabel.Text = !isValidSale ? "0" : gold.ToString();
                silverProfitLabel.Text = !isValidSale ? "0" : silver.ToString();
                copperProfitLabel.Text = !isValidSale ? "0" : copper.ToString();

                goldProfitLabel.TextColor = !isValidSale ? Color.White : (profit >= 1 ? Color.Green : Color.Red);
                silverProfitLabel.TextColor = !isValidSale ? Color.White : (profit >= 1 ? Color.Green : Color.Red);
                copperProfitLabel.TextColor = !isValidSale ? Color.White : (profit >= 1 ? Color.Green : Color.Red);

            }
            else
            {

                textBox.Text = new string(textBox.Text.Where(char.IsDigit).ToArray()); // Yuck
                textBox.CursorIndex = textBox.Length; // YUCK! - Fixes an issue where entering invalid characters/changing the text resets its index to 1?

            }

        }

        protected override void Unload()
        {
            
            cornerIcon?.Dispose();
            flipWindow?.Dispose();
            iconTexture.Dispose();
            emblemTexture?.Dispose();
            backgroundTexture?.Dispose();
            backgroundTextureResized?.Dispose();
            coinGoldTexture?.Dispose();
            coinSilverTexture?.Dispose();
            coinCopperTexture?.Dispose();
            goldBuyTextBox?.Dispose();
            silverBuyTextBox?.Dispose();
            copperBuyTextBox?.Dispose();
            goldSellTextBox?.Dispose();
            silverSellTextBox?.Dispose();
            copperSellTextBox?.Dispose();
            goldProfitLabel?.Dispose();
            silverProfitLabel?.Dispose();
            copperProfitLabel?.Dispose();
            resetButton?.Dispose();
            buyLabel?.Dispose();
            sellLabel?.Dispose();
            profitLabel?.Dispose();
            foreach (IDisposable disposable in disposableBin)
            {

                disposable?.Dispose();

            }
            Instance = null;

        }

    }

}