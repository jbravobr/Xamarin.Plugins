using System;
using Xamarin.Forms;

namespace WorklightSample
{
	public class ResultPage : ContentPage
	{


		public ResultPage(WorklightResult result) 
			: base()
		{

			BackgroundColor = Color.XamarinLightGray.ToFormsColor ();

			var htmlSource = new HtmlWebViewSource ();
			htmlSource.Html = (String.IsNullOrWhiteSpace(result.Response)) ? result.Message : result.Response;

			var webView = new WebView()
			{
				Source = htmlSource,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};

			Content = webView;

		}
	}
}

