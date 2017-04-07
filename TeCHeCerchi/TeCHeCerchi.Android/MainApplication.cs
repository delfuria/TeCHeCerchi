using System;

using Android.App;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using TeCHeCerchi.Droid.Helpers;
using TeCHeCerchi.Shared.Helpers;
using TeCHeCerchi.Shared.Interfaces;

namespace TeCHeCerchi.Droid
{
    //You can specify additional application information in this attribute
#if DEBUG
    [Application(Theme = "@style/Theme.Quest", Debuggable = true)]
#else
	[Application(Theme = "@style/Theme.Quest", Debuggable=false)]
#endif


    public class App : Application, Application.IActivityLifecycleCallbacks
	{
		public static Activity CurrentActivity { get; set; }

		public App(IntPtr handle, JniHandleOwnership transer)
		  : base(handle, transer)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate();
			RegisterActivityLifecycleCallbacks(this);
			//A great place to initialize Xamarin.Insights and Dependency Services!

			base.OnCreate();
			FileCache.SaveLocation = CacheDir.AbsolutePath;
			ServiceContainer.Register<IMessageDialog>(new Messages());
			Xamarin.Insights.Initialize("6978a61611a7e9f6fd20172582cb56911ee3131c", this);
#if DEBUG
			Xamarin.Insights.DisableCollection = true;
			Xamarin.Insights.DisableDataTransmission = true;
			Xamarin.Insights.DisableExceptionCatching = true;
#endif
		}

		public override void OnTerminate()
		{
			base.OnTerminate();
			UnregisterActivityLifecycleCallbacks(this);
		}

		public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
		{
			CrossCurrentActivity.Current.Activity = activity;
		}

		public void OnActivityDestroyed(Activity activity)
		{
		}

		public void OnActivityPaused(Activity activity)
		{
		}

		public void OnActivityResumed(Activity activity)
		{
			CrossCurrentActivity.Current.Activity = activity;
		}

		public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
		{
		}

		public void OnActivityStarted(Activity activity)
		{
			CrossCurrentActivity.Current.Activity = activity;
		}

		public void OnActivityStopped(Activity activity)
		{
		}
	}
}