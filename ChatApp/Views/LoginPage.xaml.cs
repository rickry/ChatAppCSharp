using ChatApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = ViewModel;

            UsernameEntry.Completed += (sender, args) => { PasswordEntry.Focus(); };
            PasswordEntry.Completed += (sender, args) => { ViewModel.AuthenticateCommand.Execute(null); };
        }

        public LoginViewModel ViewModel { get; set; } = new LoginViewModel(App.NavigationService);

        #region Overrides of Page
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        #endregion
    }
}