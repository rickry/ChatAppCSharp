using System;
using ChatApp.Services;
using ChatApp.ViewModels;
using ChatApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ChatApp
{
    public partial class App : Application
    {

        public static INavigationService NavigationService { get; } = new ViewNavigationService();
        public static bool IsUserLoggedIn { get; set; }


        public App()
        {
            InitializeComponent();

            new Constants();

            NavigationService.Configure(PageNames.LoginPage, typeof(Views.LoginPage));
            NavigationService.Configure(PageNames.ChatPage, typeof(Views.ChatPage));

            var mainPage = ((ViewNavigationService)NavigationService).SetRootPage(PageNames.ChatPage);

            MainPage = mainPage;

        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            ChatPageViewModel.client.Emit("user-unjoin", Constants.User);
            Console.WriteLine("Sleep");

        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            ChatPageViewModel.client.Emit("user-join", Constants.User);
            DependencyService.Get<IToast>().LongAlert("Welcome back");
        }

    }
}
