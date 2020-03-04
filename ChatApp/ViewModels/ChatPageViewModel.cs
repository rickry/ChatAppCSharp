using ChatApp.Models;
using ChatApp.Services;
using H.Socket.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ChatApp.ViewModels
{
    public class ChatPageViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;

        public static SocketIoClient client = new SocketIoClient();
        public bool ShowScrollTap { get; set; } = false;
        public bool LastMessageVisible { get; set; } = true;
        public int PendingMessageCount { get; set; } = 0;
        public bool PendingMessageCountVisible { get { return PendingMessageCount > 0; } }
        public Queue<Message> DelayedMessages { get; set; } = new Queue<Message>();
        public ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();
        public string TextToSend { get; set; }
        public ICommand OnSendCommand { get; set; }
        public ICommand MessageAppearingCommand { get; set; }
        public ICommand MessageDisappearingCommand { get; set; }

        public ChatPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            LogoutCommand = new Command(Logout);
            ConnectToServer().GetAwaiter();


            MessageAppearingCommand = new Command<Message>(OnMessageAppearing);
            MessageDisappearingCommand = new Command<Message>(OnMessageDisappearing);

            OnSendCommand = new Command(() =>
            {
                if (!string.IsNullOrEmpty(TextToSend))
                {
                    Message message = new Message() { Text = TextToSend, User = Constants.User, Time = DateTime.Now.ToFileTime().ToString(), Id = Constants.Id };
                    client.Emit("chat-message", message);
                    TextToSend = string.Empty;
                }

            });
        }

        public async Task ConnectToServer()
        {
            client.Connected += (sender, args) => ClientConnectedAsync().GetAwaiter();
            client.Disconnected += (sender, args) => ClientDisconnected();
            //client.EventReceived += (sender, args) => Console.WriteLine($"EventReceived: Namespace: {args.Namespace}, Value: {args.Value}, IsHandled: {args.IsHandled}");
            client.HandledEventReceived += (sender, args) => Console.WriteLine($"HandledEventReceived: Namespace: {args.Namespace}, Value: {args.Value}");
            //client.UnhandledEventReceived += (sender, args) => Console.WriteLine($"UnhandledEventReceived: Namespace: {args.Namespace}, Value: {args.Value}");
            //client.ErrorReceived += (sender, args) => Console.WriteLine($"ErrorReceived: Namespace: {args.Namespace}, Value: {args.Value}");
            client.ExceptionOccurred += (sender, args) => Console.WriteLine($"ExceptionOccurred: {args.Value}");

            client.On<User>("user-id", user =>
            {
                if (Constants.Id == null)
                {
                    Constants.Id = user.Id;
                    Console.WriteLine("IS RESET to " + user.Id);
                }
            });

            client.On<Message>("chat-message", message =>
            {
                Messages.Insert(0, new Message() { Text = message.Text, Time = message.Time, User = message.User, Id = message.Id });
            });
           


            await client.ConnectAsync(Constants.SocketServer);

            //await client.DisconnectAsync();
        }

        private void ClientDisconnected()
        {
            DependencyService.Get<IUserNotificationService>().Snack("Disconnected");
            Constants.Id = null;
        }

        private async Task ClientConnectedAsync()
        {
            DependencyService.Get<IUserNotificationService>().Snack("Connected");
            await client.Emit("user-join", Constants.User);
        }

        public static async Task OnLeftAsync()
        {
            await client.Emit("user-unjoin", Constants.User);
        }

        void OnMessageAppearing(Message message)
        {
            var idx = Messages.IndexOf(message);
            if (idx <= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    while (DelayedMessages.Count > 0)
                    {
                        Messages.Insert(0, DelayedMessages.Dequeue());
                    }
                    ShowScrollTap = false;
                    LastMessageVisible = true;
                    PendingMessageCount = 0;
                });
            }
        }

        void OnMessageDisappearing(Message message)
        {
            var idx = Messages.IndexOf(message);
            if (idx >= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ShowScrollTap = true;
                    LastMessageVisible = false;
                });

            }
        }

        private void Logout()
        {
            App.IsUserLoggedIn = false;
            _navigationService.NavigateModalAsync(PageNames.LoginPage);
        }

        public ICommand LogoutCommand { private set; get; }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
