using Intermediario.Views;
using Prism;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Intermediario
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync(nameof(MasterView));
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<MasterView>();
            Container.RegisterTypeForNavigation<InputListView>();
            Container.RegisterTypeForNavigation<StockView>();
            Container.RegisterTypeForNavigation<ManagerView>();
            Container.RegisterTypeForNavigation<PayListView>();
            Container.RegisterTypeForNavigation<InputDetailsView>();


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
