using System;
using System.Linq;
using UIKit;
using TeCHeCerchi.Shared.Interfaces;
using GCDiscreetNotification;

namespace TeCHeCerchi.iOS.Helpers
{

    public class Messages : IMessageDialog
    {
        public void SendToast(string toast)
        {
            Utils.EnsureInvokedOnMainThread(() =>
                {
                    var notificationView = new GCDiscreetNotificationView(toast, false, GCDNPresentationMode.Bottom,
                                   UIApplication.SharedApplication.KeyWindow.RootViewController.View);

                    notificationView.ShowAndDismissAfter(3);
                });
        }

        public void SendMessage(string title, string message)
        {
            SendMessage(title, message, null);
        }

        public void SendMessage(string title, string message, Action completed)
        {
            Utils.EnsureInvokedOnMainThread(() =>
                {
                    var alert = new UIAlertView(title, message, null, "OK");
                    alert.Dismissed += (sender2, args) =>
                    {
                        if (completed != null)
                            completed();

                    };
                    alert.Show();
                });
        }

        public void EnterTextMessage(string title, string message, Action<string> completed)
        {

            Utils.EnsureInvokedOnMainThread(() =>
                {
                    var alert = new UIAlertView(title, message, null, "Enter", "Cancel")
                    {
                        AlertViewStyle = UIAlertViewStyle.PlainTextInput
                    };
                    alert.Dismissed += (sender2, args) =>
                    {
                        if (args.ButtonIndex != 0)
                            return;

                        completed(alert.GetTextField(0).Text.Trim());

                    };
                    alert.Show();
                });
        }

        public void AskQuestions(string title, Shared.Models.Question question, Action<int> completed)
        {
            Utils.EnsureInvokedOnMainThread(() =>
                {
                    var alert = new UIAlertView(title, question.Text, null, "Cancel", question.Answers.Select(s => s.Text).ToArray());

                    alert.Dismissed += (sender2, args) =>
                    {
                        if (args.ButtonIndex == 0)
                            return;

                        completed((int)args.ButtonIndex - 1);//subtract 1 becuase of cancel button

                    };
                    alert.Show();
                });
        }
    }
}