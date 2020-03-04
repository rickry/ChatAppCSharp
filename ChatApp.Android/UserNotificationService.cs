using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;


[assembly: Dependency(typeof(ChatApp.Droid.UserNotificationService))]

namespace ChatApp.Droid
{
	public class UserNotificationService : IUserNotificationService
	{
		[Obsolete]
		public void Snack(string message, int duration, string actionText = null, Action<object> action = null)
		{
			var contentView = (Forms.Context as Activity)?.FindViewById(Android.Resource.Id.Content);
			var snackbar = Android.Support.Design.Widget.Snackbar.Make(contentView, message, duration);
			if (actionText != null && action != null)
				snackbar.SetAction(actionText, action);
			snackbar.Show();
		}
	}
}