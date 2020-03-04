using ChatApp.Models;
using ChatApp.Views.Cells;
using Xamarin.Forms;

namespace ChatApp.Helpers
{
    class ChatTemplateSelector : DataTemplateSelector
    {
        DataTemplate incomingDataTemplate;
        DataTemplate outgoingDataTemplate;

        public ChatTemplateSelector()
        {
            this.incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            this.outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as Message;
            if (messageVm == null)
                return null;

            System.Console.WriteLine(messageVm.Id +" AND "+ Constants.Id);

            return (messageVm.Id == Constants.Id)? incomingDataTemplate : outgoingDataTemplate;
        }

    }
}