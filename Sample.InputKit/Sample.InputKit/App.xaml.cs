using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Sample.InputKit
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Plugin.InputKit.Shared.Controls.CheckBox.GlobalSetting.BorderColor = Color.Accent;
            Plugin.InputKit.Shared.Controls.CheckBox.GlobalSetting.TextColor = Color.Red;
            Plugin.InputKit.Shared.Controls.CheckBox.GlobalSetting.Color = Color.Blue;

            MainPage = new NavigationPage(new Sample.InputKit.MainPage());

            
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
