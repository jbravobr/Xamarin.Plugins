using Worklight;
using System.Json;
using System.Diagnostics;

namespace Plugin.MobileFirst.Abstractions
{
	public class NotificationListener : WorklightPushNotificationListener
	{
		public void OnMessage(JsonObject NotificationProperties, JsonObject Payload)
		{
			Debug.WriteLine("Got notification!");
			Debug.WriteLine(NotificationProperties.ToString());
		}
	}
}

