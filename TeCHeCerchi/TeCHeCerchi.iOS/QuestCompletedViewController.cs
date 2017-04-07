using System;
using UIKit;
using TeCHeCerchi.Shared.ViewModels;
using TeCHeCerchi.Shared.Helpers;

namespace TeCHeCerchi.iOS
{
    partial class QuestCompletedViewController : UIViewController
    {
        public QuestViewModel ViewModel { get; set; }

        public QuestCompletedViewController(IntPtr handle)
            : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ButtonContinue.Layer.CornerRadius = 5;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            LabelQuestNumber.Text = ViewModel.CompletionDisplayShort;
            Settings.QuestDone = true;
        }

        partial void ButtonContinue_TouchUpInside(UIButton sender)
        {
            DismissViewControllerAsync(true); 
        }
    }
}
