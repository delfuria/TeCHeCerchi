// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TeCHeCerchi.iOS
{
    [Register ("GameCompleteViewController")]
    partial class GameCompleteViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ButtonShare { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageMain { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelMessage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView ProgressBar { get; set; }

        [Action ("ButtonShare_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ButtonShare_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ButtonShare != null) {
                ButtonShare.Dispose ();
                ButtonShare = null;
            }

            if (ImageMain != null) {
                ImageMain.Dispose ();
                ImageMain = null;
            }

            if (LabelMessage != null) {
                LabelMessage.Dispose ();
                LabelMessage = null;
            }

            if (ProgressBar != null) {
                ProgressBar.Dispose ();
                ProgressBar = null;
            }
        }
    }
}