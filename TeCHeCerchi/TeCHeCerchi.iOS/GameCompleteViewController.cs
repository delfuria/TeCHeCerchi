using System;
using TeCHeCerchi.Shared.ViewModels;
using CoreLocation;
using TeCHeCerchi.Shared.Extensions;
using Foundation;
using UIKit;

namespace TeCHeCerchi.iOS
{
	partial class GameCompleteViewController : UIViewController
	{
		public GameCompleteViewController(IntPtr handle)
			: base(handle)
		{
		}


		CLBeaconRegion secretRegion;
		const string BeaconId = "com.refractored";
		CLLocationManager manager;
		GameCompleteViewModel viewModel;
		UIBarButtonItem scanButton;
		ZXing.Mobile.MobileBarcodeScanner scanner;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
	
			viewModel = new GameCompleteViewModel();
			scanner = new ZXing.Mobile.MobileBarcodeScanner();

			ButtonShare.Layer.CornerRadius = 5;
			scanButton = new UIBarButtonItem("Scan", UIBarButtonItemStyle.Plain, async (sender, args) =>
				{

					var result = await scanner.Scan(true);

					if (result == null)
						return;

					Console.WriteLine("Scanned Barcode: " + result.Text);
					viewModel.CheckBeacon(result.Text);
				});


			NavigationItem.RightBarButtonItem = scanButton;

			viewModel.PropertyChanged += HandlePropertyChanged;

			manager = new CLLocationManager();
			manager.DidRangeBeacons += (sender2, e) =>
			{
				if (e.Beacons == null)
					return;

				foreach (var beacon in e.Beacons)
				{
					if (beacon.Proximity != CLProximity.Immediate)
					{
						return;
					}

					if (beacon.Accuracy > .2)//close, but not close enough.
						return;

					viewModel.CheckBeacon(beacon.Major.Int32Value, beacon.Minor.Int32Value);
				}
			};



			viewModel.LoadGameCommand.Execute(null);
		}

	

		partial void ButtonShare_TouchUpInside(UIButton sender)
		{

			var items = new NSObject[] { new NSString("Ho terminato #TeCHeCerchi ed ho vinto uno splendido premio! http://www.elea9003.cloud/Quest/questimages/gattino.jpg") };
			var activityController = new UIActivityViewController(items, null);
			PresentViewController(activityController, true, null);

		}

		private void StopRanging()
		{
			if (secretRegion != null)
				manager.StopRangingBeacons(secretRegion);
		}

		private void StartRanging()
		{
			StopRanging();

			if (viewModel == null || viewModel.Prize == null)
				return;

			if (secretRegion == null)
			{
				secretRegion = new CLBeaconRegion(new NSUuid(viewModel.UUID), 9999, BeaconId);
			}

			manager.StartRangingBeacons(secretRegion);
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			StartRanging();
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);
			StopRanging();
		}




		void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			InvokeOnMainThread(() =>
				{
					switch (e.PropertyName)
					{

						case BaseViewModel.IsBusyPropertyName:
							if (viewModel.IsBusy)
								ProgressBar.StartAnimating();
							else
								ProgressBar.StopAnimating();
							break;
						case GameCompleteViewModel.PrizePropertyName:
							{
								LabelMessage.Text = viewModel.Prize.Message;
								ImageMain.LoadUrl(viewModel.Prize.Image);

								if (viewModel.SecretBeaconFound)
									ImageMain.Image = UIImage.FromBundle("ic_secret_prize");
								StartRanging();
							}
							break;
						case GameCompleteViewModel.SecretBeaconFoundPropertyName:
							if (viewModel.SecretBeaconFound)
								ImageMain.Image = UIImage.FromBundle("ic_secret_prize");
							break;
					}
				});
		}

	}
}
