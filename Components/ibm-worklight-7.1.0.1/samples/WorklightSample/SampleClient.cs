using System;
using Worklight;

using System.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace WorklightSample
{
	/// <summary>
	/// Sample Worklight client 
	/// </summary>
	public class SampleClient
	{
		#region Fields

		private string pushAlias = "myAlias2";
		private string appRealm = "SampleAppRealm";
		private JsonObject metadata = (JsonObject)JsonObject.Parse(" {\"platform\" : \"Xamarin\" } ");

		#endregion

		#region Properties

		public IWorklightClient client { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this instance is push supported.
		/// </summary>
		/// <value><c>true</c> if this instance is push supported; otherwise, <c>false</c>.</value>
		public bool IsPushSupported
		{
			get
			{
				try
				{
					return client.PushService.IsPushSupported;
				}
				catch
				{
					return false;
				}

			}

		}

		/// <summary>
		/// Gets a value indicating whether this instance is subscribed.
		/// </summary>
		/// <value><c>true</c> if this instance is subscribed; otherwise, <c>false</c>.</value>
		public bool IsSubscribed
		{
			get
			{
				try
				{
					return client.PushService.IsAliasSubscribed(pushAlias);
				}
				catch
				{
					return false;
				}


			}

		}

		#endregion

		#region Constuctors

		public SampleClient(IWorklightClient wlc)
		{
			this.client = wlc;
		}

		#endregion

		#region Async functions

		public async Task<WorklightResult> ConnectAsync()
		{
			var result = new WorklightResult();

			try
			{
				var resp = await Connect();

				result.Success = resp.Success;
				result.Message = (resp.Success) ? "Connected" : resp.Message;
				result.Response = resp.ResponseText;
			}
			catch (Exception ex)
			{
				result.Success = false;
				result.Message = ex.Message;
			}

			return result;
		}

		public async Task<WorklightResult> RestInvokeAsync()
		{
			var result = new WorklightResult();

			try
			{
				StringBuilder uriBuilder = new StringBuilder()
					.Append(client.ServerUrl.AbsoluteUri) // Get the server URL
					.Append("/adapters") 	
					.Append("/SampleHTTPAdapter") //Name of the adapter
					.Append("/getStories");	// Name of the adapter procedure
				WorklightResourceRequest rr = client.ResourceRequest(new Uri(uriBuilder.ToString()), "GET" );

				WorklightResponse resp = await rr.Send(); 

				result.Success = resp.Success;
				result.Message = (resp.Success) ? "Connected" : resp.Message;
				result.Response = resp.ResponseText;
			}
			catch (Exception ex)
			{
				result.Success = false;
				result.Message = ex.Message;
			}

			return result;
		}

		public async Task<WorklightResult> InvokeAsync()
		{
			var result = new WorklightResult();

			try
			{
				var conResp = await ConnectAsync();

				if (!conResp.Success)
					return conResp;

				result = await InvokeProc();

			}
			catch (Exception ex)
			{
				result.Success = false;
				result.Message = ex.Message;
			}

			return result;
		}

		public async Task<WorklightResult> SendActivityAsync()
		{
			var result = new WorklightResult();

			try
			{
				var resp = await Task.Run<string>(()=>
				{
					client.Analytics.Send();

					client.LogActivity("sample data from Xamarin app");

					return "Activity Logged";
				});

				result.Success = true;
				result.Message = resp;

			}
			catch (Exception ex)
			{
				result.Success = false;
				result.Message = ex.Message;
			}

			return result;
		}

		public async Task<WorklightResult> SubscribeAsync()
		{
			var result = new WorklightResult();

			try
			{
				var resp = await SubscribePush();
				result.Success = resp.Success;
				result.Message = "Subscribed";
				result.Response = resp.ResponseText;
			}
			catch (Exception ex)
			{
				result.Success = false;
				result.Message = ex.Message;
			}

			return result;
		}

		public async Task<WorklightResult> UnSubscribeAsync()
		{
			var result = new WorklightResult();

			try
			{
				var resp = await UnsubscribePush();

				result.Success = resp.Success;
				result.Message = "Unsubscribed";
				result.Response = resp.ResponseText;
			}
			catch (Exception ex)
			{
				result.Success = false;
				result.Message = ex.Message;
			}

			return result;
		}

		#endregion
	
		#region Worklight Methods
		/// <summary>
		/// Connect to the server instance
		/// </summary>
		private async Task<WorklightResponse> Connect()
		{
			//lets send a message to the server
			client.Analytics.Log("Trying to connect to server", metadata);

			ChallengeHandler customCH = new CustomChallengeHandler(appRealm);
			client.RegisterChallengeHandler(customCH);
			WorklightResponse task = await client.Connect();
			//lets log to the local client (not server)
			client.Logger("Xamarin").Trace("connection");
			//write to the server the connection status
			client.Analytics.Log("Connect response : " + task.Success);
			return task;
		}

		/// <summary>
		/// Unsubscribes from push notifications
		/// </summary>
		/// <returns>The push.</returns>
		private async Task<WorklightResponse> UnsubscribePush()
		{
			try
			{
				client.Analytics.Log("Unsubscribing Push", metadata);
				WorklightResponse task = await client.PushService.UnsubscribeFromEventSource(pushAlias);
				return task;
			}
			catch (Exception ex)
			{
				return null;
			}

		}

		/// <summary>
		/// Subscribes to push notifications
		/// </summary>
		/// <param name="callBack">Call back.</param>
		private async Task<WorklightResponse> SubscribePush()
		{
			Console.WriteLine("Subscribing to push");

			client.PushService.ReadyToSubscribe += HandleReadyToSubscribe;
			client.PushService.InitRegistration();
			return await client.Connect ();

		}
		void HandleReadyToSubscribe(object sender, EventArgs a)
		{
			Console.WriteLine ("We are ready to subscribe to the notification service!!");
			client.PushService.RegisterEventSourceNotificationCallback(pushAlias,"PushAdapter","PushEventSource",new NotificationListener ());
			client.PushService.SubscribeToEventSource(pushAlias,new Dictionary<string,string>());
		}

		/// <summary>
		/// Invokes the procedured
		/// </summary>
		/// <returns>The proc.</returns>
		private async Task<WorklightResult> InvokeProc()
		{
			var result = new WorklightResult();

			try
			{
				client.Analytics.Log("trying to invoking procedure");
				System.Diagnostics.Debug.WriteLine("Trying to invoke proc");
				WorklightProcedureInvocationData invocationData = new WorklightProcedureInvocationData("SampleHTTPAdapter", "getStories", new object[] { "technology" });
				WorklightResponse task = await client.InvokeProcedure(invocationData);
				client.Analytics.Log("invoke response : " + task.Success);
				StringBuilder retval = new StringBuilder();

				result.Success = task.Success;

				if (task.Success)
				{
					JsonArray jsonArray = (JsonArray)task.ResponseJSON["rss"]["channel"]["item"];
					foreach (JsonObject title in jsonArray)
					{
						System.Json.JsonValue titleString;
						title.TryGetValue("title", out titleString);
						retval.Append(titleString.ToString());
						retval.AppendLine();
					}
				}
				else
				{
					retval.Append("Failure: " + task.Message);
				}

				result.Message = retval.ToString();
			}
			catch (Exception ex)
			{
				result.Success = false;
				result.Message = ex.Message;
			}

			return result;
		
		}

		#endregion
	}

}

