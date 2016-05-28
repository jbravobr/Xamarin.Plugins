using System;
using Worklight;
using System.Json;

namespace WorklightSample
{
	public class NotificationListener : WorklightPushNotificationListener
	{
		public void OnMessage(JsonObject NotificationProperties, JsonObject Payload)
		{
			Console.WriteLine("Got notification!");
			Console.WriteLine(NotificationProperties.ToString());
		}
	}
}

