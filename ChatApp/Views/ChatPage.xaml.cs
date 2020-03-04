using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ChatApp.ViewModels;
using Xamarin.Forms;

namespace ChatApp.Views
{
    public partial class ChatPage : ContentPage
    {

        public ChatPage()
        {
            InitializeComponent();
        }

        public void ScrollTap(object sender, System.EventArgs e)
        {
            lock (new object())
            {
                if (BindingContext != null)
                {
                    var vm = BindingContext as ChatPageViewModel;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        while (vm.DelayedMessages.Count > 0)
                        {
                            vm.Messages.Insert(0, vm.DelayedMessages.Dequeue());
                        }
                        vm.ShowScrollTap = false;
                        vm.LastMessageVisible = true;
                        vm.PendingMessageCount = 0;
                        ChatList?.ScrollToFirst();
                    });


                }

            }
        }
       
        public void OnListTapped(object sender, ItemTappedEventArgs e)
        {
            chatInput.UnFocusEntry();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!App.IsUserLoggedIn)
            {
                if (App.NavigationService.CurrentPageKey == PageNames.LoginPage) return;
                await App.NavigationService.NavigateModalAsync(PageNames.LoginPage, true);
            }
        }
    }
}
