using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using HexedHero.Blish_HUD.FlipperHelper.Managers;

namespace HexedHero.Blish_HUD.FlipperHelper.Objects
{

    public class SettingRedirectView : View
    {

        StandardButton RedirectButton;

        public SettingRedirectView() { }

        protected override void Unload()
        {

            RedirectButton?.Dispose();

        }

        protected override void Build(Container buildPanel)
        {

            RedirectButton = new StandardButton()
            {

                Text = "Open Settings",
                Parent = buildPanel,
                Left = 5,
                Top = 5

            };

            RedirectButton.Click += delegate
            {

                WindowManager.Instance.MainWindow.Show();
                WindowManager.Instance.MainWindow.SelectedTab = WindowManager.Instance.MainWindow.Tabs.FromIndex(1);

            };

        }

    }

}
