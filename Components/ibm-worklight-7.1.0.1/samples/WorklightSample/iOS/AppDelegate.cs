using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Foundation;
using UIKit;

using Xamarin.Forms;
using Worklight.Xamarin.iOS;

namespace WorklightSample.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			var splashDelay = Task.Delay (2000);

			global::Xamarin.Forms.Forms.Init ();

			App.WorklightClient =  new SampleClient ( WorklightClient.CreateInstance());

			LoadApplication (new App ());

			splashDelay.Wait ();
			
			return base.FinishedLaunching (app, options);
		}
	}
}

