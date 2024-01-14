using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using HexedHero.Blish_HUD.FlipperHelper.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace HexedHero.Blish_HUD.FlipperHelper.Objects
{

    public class CalculatorView : View
    {

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

        public CalculatorView() { }

        protected override void Unload()
        {

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

        }

        protected override void Build(Container container)
        {

            // Textures
            coinGoldTexture = FlipperHelper.Instance.Module.ContentsManager.GetTexture("coin_gold.png");
            coinSilverTexture = FlipperHelper.Instance.Module.ContentsManager.GetTexture("coin_silver.png");
            coinCopperTexture = FlipperHelper.Instance.Module.ContentsManager.GetTexture("coin_copper.png");

            // Buy row
            buyLabel = new Label()
            {

                Text = "Buy Price:",
                Size = new Point(75, 18),
                Location = new Point(10, 0),
                Parent = container

            };

            CreateCoinControl(container, ECoinType.GOLD, coinGoldTexture, 10, 20, ref goldBuyTextBox);
            CreateCoinControl(container, ECoinType.SILVER, coinSilverTexture, 110, 20, ref silverBuyTextBox);
            CreateCoinControl(container, ECoinType.COPPER, coinCopperTexture, 185, 20, ref copperBuyTextBox);

            // Sell row
            sellLabel = new Label()
            {

                Text = "Sell Price:",
                Size = new Point(75, 18),
                Location = new Point(10, 55),
                ShowShadow = true,
                Parent = container

            };

            CreateCoinControl(container, ECoinType.GOLD, coinGoldTexture, 10, 75, ref goldSellTextBox);
            CreateCoinControl(container, ECoinType.SILVER, coinSilverTexture, 110, 75, ref silverSellTextBox);
            CreateCoinControl(container, ECoinType.COPPER, coinCopperTexture, 185, 75, ref copperSellTextBox);

            // Profit row
            profitLabel = new Label()
            {

                Text = "Profit:",
                Size = new Point(75, 18),
                Location = new Point(10, 110),
                ShowShadow = true,
                Parent = container

            };

            CreateCoinDisplay(container, ECoinType.GOLD, coinGoldTexture, 10, 130, ref goldProfitLabel);
            CreateCoinDisplay(container, ECoinType.SILVER, coinSilverTexture, 110, 130, ref silverProfitLabel);
            CreateCoinDisplay(container, ECoinType.COPPER, coinCopperTexture, 185, 130, ref copperProfitLabel);

            // Reset button
            resetButton = new StandardButton()
            {

                Text = "Reset",
                Size = new Point(50, 25),
                Location = new Point(10, 160),
                BasicTooltipText = "Reset all the information.",
                Parent = container

            };

            resetButton.Click += delegate { ResetInformation(); };

        }

        private void CreateCoinControl(Container container, ECoinType coinType, Texture2D coinTexture, int x, int y, ref TextBox textBox)
        {

            new Image(coinTexture)
            {

                Size = new Point(18, 18),
                Location = new Point(x, y + 5),
                Parent = container

            };

            textBox = new TextBox()
            {

                PlaceholderText = "0",
                MaxLength = (coinType == ECoinType.GOLD) ? 5 : 2,
                Size = new Point((coinType == ECoinType.GOLD) ? 65 : 40, 25),
                Font = GameService.Content.DefaultFont16,
                Location = new Point(x + 25, y),
                Parent = container

            };

            TextBox localTextBox = textBox;
            textBox.TextChanged += delegate { UpdateInformation(localTextBox); }; // Yuck

        }

        private void CreateCoinDisplay(Container container, ECoinType coinType, Texture2D coinTexture, int x, int y, ref Label label)
        {

            new Image(coinTexture)
            {

                Size = new Point(18, 18),
                Location = new Point(x, y + 5),
                Parent = container

            };

            label = new Label()
            {

                Text = "0",
                Size = new Point((coinType == ECoinType.GOLD) ? 65 : 40, 25),
                Font = GameService.Content.DefaultFont16,
                Location = new Point(x + 35, y),
                Parent = container

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

                System.Boolean isValidSale = (goldBuyValue != 0 || silverBuyValue != 0 || copperBuyValue != 0) && (goldSellValue != 0 || silverSellValue != 0 || copperSellValue != 0);

                // Get the fees for selling
                int listingFee = !isValidSale ? 0 : Math.Max(1, (int)Math.Round(sellValue * 0.05)); // 5% with a minimum of 1 copper and rounds up
                int saleFee = !isValidSale ? 0 : Math.Max(1, (int)Math.Round(sellValue * 0.10)); // 10% with a minimum of 1 copper and rounds up

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

        public void ResetInformation()
        {

            goldBuyTextBox.Text = string.Empty;
            silverBuyTextBox.Text = string.Empty;
            copperBuyTextBox.Text = string.Empty;
            goldSellTextBox.Text = string.Empty;
            silverSellTextBox.Text = string.Empty;
            copperSellTextBox.Text = string.Empty;

        }

    }

}
