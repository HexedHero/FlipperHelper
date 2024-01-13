using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

namespace HexedHero.Blish_HUD.FlipperHelper.Objects
{

    class SettingRedirectView : View
    {

        public SettingRedirectView() { }

        protected override void Unload()
        {

            base.Unload();

        }

        protected override void Build(Container buildPanel)
        {

            StandardButton redirectButton = new StandardButton()
            {

                Text = "Open Settings",
                Parent = buildPanel,
                Left = 5,
                Top = 5

            };

            redirectButton.Click += delegate
            {

                FlipperHelper.Instance.MainWindow.Show();
                FlipperHelper.Instance.MainWindow.SelectedTab = FlipperHelper.Instance.MainWindow.Tabs.FromIndex(1);

            };

        }

    }

}
