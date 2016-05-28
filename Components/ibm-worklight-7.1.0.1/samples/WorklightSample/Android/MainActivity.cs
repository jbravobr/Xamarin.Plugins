using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms.Platform.Android;
using Worklight.Xamarin.Android;


namespace WorklightSample.Android
{
	[Activity (Label = "MobileFirst Sample", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	[IntentFilter (new[]{"worklightsample.android.WorklightSample.Android.NOTIFICATION"} , 
		Categories=new[]{Intent.CategoryDefault})]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

			App.WorklightClient =  new SampleClient ( WorklightClient.CreateInstance (this));

			LoadApplication (new App ());
		}
		protected override void OnResume ()
		{
			base.OnResume();
			App.WorklightClient.client.PushService.Foreground = true;
		}
		protected override void OnPause ()
		{
			base.OnPause();
			App.WorklightClient.client.PushService.Foreground = false;
		}
		protected override void OnDestroy ()
		{
			base.OnDestroy();
			App.WorklightClient.client.PushService.UnregisterReceivers ();
		}
	}
}

