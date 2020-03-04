using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp
{

    public interface IUserNotificationService
    {
        void Snack(string message, int duration = 5000, string actionText = null, Action<object> action = null);
    }
}
