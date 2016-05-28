using System;
using System.Json;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Worklight;
using System.Text;

namespace WorklightSample
{
	public class HomePage : ContentPage
	{
		#region Fields
		private RelativeLayout indicatorLayout = null;
		private ListView listView = null;
		private ActivityIndicator activityIndicator = null;
		bool busy = false;

		#endregion

		#region Constructors
		public HomePage ()
		{
			BackgroundColor = Color.XamarinLightGray.ToFormsColor ();
			Title = "MobileFirst Sample";

			var activityIndicatorColor = new Xamarin.Forms.Color (Color.XamarinGray.R, Color.XamarinGray.G, Color.XamarinGray.B, .4);

			activityIndicator = new ActivityIndicator{
				Color = Xamarin.Forms.Color.Black,
				BackgroundColor = new Xamarin.Forms.Color (0, 0, 0, 0),
				IsRunning = false,
				IsVisible = true
			};

			listView = new ListView {
				RowHeight = 40,
				BackgroundColor = Xamarin.Forms.Color.White,
				VerticalOptions = LayoutOptions.Start
			};

			listView.ItemsSource = new [] {
				new CommandItem { Command = "Connect", Image = "connect.png", ItemSelected = OnConnect },
				new CommandItem { Command = "Invoke Procedure", Image = "invoke.png", ItemSelected = OnInvokeProcedure },
				new CommandItem { Command = "Subscribe to Push", Image = "subscribe.png", ItemSelected = OnSubscribe },
				new CommandItem { Command = "Unsubscribe from Push", Image = "unsubscribe.png", ItemSelected = OnUnSubscribe },
				new CommandItem { Command = "Is Push Subscribed?", Image = "issubscribed.png", ItemSelected = OnIsSubscribed },
				new CommandItem { Command = "Is Push Supported?", Image = "issubscribed.png", ItemSelected = OnIsPushEnabled },
				new CommandItem { Command = "Log Activity", Image = "logactivity.png", ItemSelected = OnSendActivity },
				new CommandItem { Command = "JSONStore destroy", Image = "logactivity.png", ItemSelected = OnDestroyJSONStore },
				new CommandItem { Command = "Open JSONStore Collection", Image = "logactivity.png", ItemSelected = OnJSONStoreOpenCollection },
				new CommandItem { Command = "Add Data to JSONStore", Image = "logactivity.png", ItemSelected = OnAddDataToJSONStore },
				new CommandItem { Command = "Retrieve all Data", Image = "logactivity.png", ItemSelected = OnRetrieveAllDataFromJSONStore },
				new CommandItem { Command = "Retrieve Filtered Data", Image = "logactivity.png", ItemSelected = OnRetrieveFilteredDataFromJSONStore },
				new CommandItem { Command = "Invoke REST via OAuth", Image = "oauth.png", ItemSelected = OnRestInvoke },
			};

			listView.ItemTemplate = new DataTemplate (typeof(CommandCell));

			listView.ItemSelected += (sender, e) => {
				if (e.SelectedItem == null) return;

				(e.SelectedItem as CommandItem).ItemSelected ();
				listView.SelectedItem = null; 
			};

			Content = CreateLayout (activityIndicatorColor);
		}

		#endregion

		#region Properties
		private bool Busy {
			get {
				return busy;
			} 
			set {

				if (busy != value) {
					busy = value;
					if (busy) {
						listView.IsVisible = true;
						listView.IsEnabled = false;
						// TODO: Uncomment after bug https://bugzilla.xamarin.com/show_bug.cgi?id=32068 is fixed.
						// Also see https://forums.xamarin.com/discussion/comment/139670#Comment_139670
						//activityIndicator.IsRunning = true;
						indicatorLayout.IsVisible = true;
					} 
					else {
						listView.IsVisible = true;
						listView.IsEnabled = true;
						// TODO: Uncomment after bug https://bugzilla.xamarin.com/show_bug.cgi?id=32068 is fixed.
						// Also see https://forums.xamarin.com/discussion/comment/139670#Comment_139670
						//activityIndicator.IsRunning = false;
						indicatorLayout.IsVisible = false;
					}
				}

			}
		}
		#endregion

		#region Layout Methods
		private RelativeLayout CreateLayout (Xamarin.Forms.Color activityIndicatorColor)
		{
			var relativeLayout = new RelativeLayout ();
			indicatorLayout = new RelativeLayout {
				IsVisible = false
			};

			indicatorLayout.Children.Add (new ContentView {
				BackgroundColor = activityIndicatorColor
			}, Constraint.Constant (0), Constraint.Constant (0), 
				Constraint.RelativeToParent (parent => parent.Width), 
				Constraint.RelativeToParent (parent => parent.Height));

			 indicatorLayout.Children.Add (activityIndicator, 
				Constraint.RelativeToParent (parent => parent.Width / 2 - 25), 
				Constraint.RelativeToParent (parent => parent.Height / 2 - 25), 
				Constraint.Constant (50), Constraint.Constant (50));

			relativeLayout.Children.Add (listView, Constraint.Constant (0), Constraint.Constant (0));
			relativeLayout.Children.Add (indicatorLayout, Constraint.Constant (0), Constraint.Constant (0), 
				Constraint.RelativeToParent (parent => parent.Width), 
				Constraint.RelativeToParent (parent => parent.Height));

			return relativeLayout;
		}
		#endregion

		#region Selection Handlers
		private async void OnConnect ()
		{
			ShowWorking();
			var result = await App.WorklightClient.ConnectAsync();
			await HandleResult(result, "Connect Result");
		}

		private async void OnInvokeProcedure ()
		{
			ShowWorking();
			var result = await App.WorklightClient.InvokeAsync();
			await HandleResult(result, "Invoke Result", true);
		}

		private async void OnSendActivity ()
		{
			ShowWorking();
			var result = await App.WorklightClient.SendActivityAsync();
			await HandleResult(result, "Send Activity Result");
		}

		private async void OnSubscribe()
		{
			ShowWorking();
			var result = await App.WorklightClient.SubscribeAsync ();
			await HandleResult(result, "Subscribe Result");
		}

		private async void OnUnSubscribe()
		{
			ShowWorking();
			var result = await App.WorklightClient.UnSubscribeAsync();
			await HandleResult(result, "Unsubcribe Result", true);
		}

		private void OnIsSubscribed()
		{
			DisplayAlert("Subcribiption Status", String.Format("{0} subscribed", (App.WorklightClient.IsSubscribed) ? "Is" : "Isn't"),"OK");
		}

		private void OnIsPushEnabled()
		{
			DisplayAlert("Push Status", String.Format("Push {0} supported", (App.WorklightClient.IsPushSupported) ? "Is" : "Isn't"),"OK");
		}

		private void OnDestroyJSONStore()
		{
			DisplayAlert("Destroy JSONStore Status", String.Format("JSONStore was {0} destroyed", (App.WorklightClient.client.JSONStoreService.JSONStore.Destroy()) ? "successfully" : "not"),"OK");
		}

		private void OnJSONStoreOpenCollection()
		{
			List<WorklightJSONStoreCollection> collectionList = new List<WorklightJSONStoreCollection>();
			WorklightJSONStoreCollection collection = App.WorklightClient.client.JSONStoreService.JSONStoreCollection ("people");
			Dictionary<string, WorklightJSONStoreSearchFieldType> searchFieldDict = new Dictionary<string, WorklightJSONStoreSearchFieldType> ();
			//WorklightJSONStoreSearchFieldType searchFied = App.WorklightClient.client.JSONStoreService.JSONStore.se
			searchFieldDict.Add ("name",WorklightJSONStoreSearchFieldType.String);
			collection.SearchFields = searchFieldDict;
			searchFieldDict = collection.SearchFields;
			collectionList.Add( collection);

			DisplayAlert("JSONStore Open Collection Status",
				String.Format("JSONStore Person Collection was {0} opened",
					(App.WorklightClient.client.JSONStoreService.JSONStore.OpenCollections(collectionList)) ? "successfully" : "not"),"OK");
		}

		private void OnAddDataToJSONStore()
		{
			JsonArray data = new JsonArray();
			JsonValue val =  JsonValue.Parse( " {\"name\" : \"Chethan\", \"laptop\" : \"macbook pro\" } ");
			JsonValue val2 =  JsonValue.Parse( " {\"name\" : \"Ajay\", \"laptop\" : \"blackbox\" } ");
			JsonValue val3 =  JsonValue.Parse( " {\"name\" : \"Srihari\", \"laptop\" : \"blackbox\" } ");
			data.Add(val);
			data.Add (val2);
			data.Add(val3);
			WorklightJSONStoreCollection collection = App.WorklightClient.client.JSONStoreService.JSONStore.GetCollectionByName ("people");
			if (collection != null)
				collection.AddData (data);
			else
				DisplayAlert ("JSONStore addData", "Open JSONstore collection before attempting to add data","OK");
		}

		private void OnRetrieveAllDataFromJSONStore()
		{
			WorklightJSONStoreCollection collection = App.WorklightClient.client.JSONStoreService.JSONStore.GetCollectionByName ("people");
			if (collection != null) {
				JsonArray outArr = collection.FindAllDocuments ();
				DisplayAlert ("All JSONStore Data",
					String.Format ("JSONStore Person data is:{0}",
						outArr.ToString ()), "OK");
			}else
				DisplayAlert ("JSONStore RetrieveData", "Open JSONstore collection before attempting to retrieve data","OK");
		}

		private void OnRetrieveFilteredDataFromJSONStore()
		{
//			JsonObject outArr = App.WorklightClient.client.JSONStoreService.JSONStoreCollection("people").FindDocumentByID(1);
			WorklightJSONStoreQueryPart[] queryParts = new WorklightJSONStoreQueryPart[1];
			queryParts [0] = App.WorklightClient.client.JSONStoreService.JSONStoreQueryPart();
			queryParts [0].AddLike ("name","Chethan");
			WorklightJSONStoreCollection collection = App.WorklightClient.client.JSONStoreService.JSONStore.GetCollectionByName("people");
			if (collection != null) {
				JsonArray outArr = collection.
				FindDocumentsWithQueryParts (queryParts);
				DisplayAlert ("Filtered JSONStore Data",
					String.Format ("JSONStore Person data is {0}",
						outArr != null ? outArr.ToString () : "not available"), "OK");
			}else
				DisplayAlert ("JSONStore RetrieveData", "Open JSONstore collection before attempting to retrieve data","OK");
		}

		private async void OnRestInvoke()
		{
			ShowWorking();
			var result = await App.WorklightClient.RestInvokeAsync();
			await HandleResult(result, "REST Invoke Result", true);
		}

		#endregion

		#region private methods
		private async Task HandleResult(WorklightResult result, String title, bool ShowOnSuccess = false)
		{
			Busy = false;

			if (result.Success)
			{
				if (ShowOnSuccess)
				{
					await Navigation.PushAsync (new ResultPage(result) {
						Title = title,
					});
				}
				else
				{
					await DisplayAlert(title,result.Message, "OK");
				}

			}
			else
			{
				await Navigation.PushAsync (new ResultPage(result) {
					Title = title,
				});
			}
		}

		private async void ShowWorking(int timeout = 150)
		{
			Busy = true;

			await Task.Delay(TimeSpan.FromSeconds(timeout));

			if (Busy)
			{
				Busy = false;
				await DisplayAlert("Timeout", "Call timed out", "OK");

			}

		}

		#endregion
	}
}

