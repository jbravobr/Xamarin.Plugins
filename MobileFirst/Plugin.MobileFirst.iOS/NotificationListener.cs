using Worklight;
using System.Json;
using System.Diagnostics;

namespace Plugin.MobileFirst.Abstractions
{
    /// <summary>
    /// Class for the Notification (Push)
    /// </summary>
    public class PushNotificationListener : WorklightPushNotificationListener
    {
        /// <summary>
        /// Listener for Message events
        /// </summary>
        /// <param name="NotificationProperties"></param>
        /// <param name="Payload"></param>
        public void OnMessage(JsonObject NotificationProperties, JsonObject Payload)
        {
            Debug.WriteLine("Got notification!");
            Debug.WriteLine(NotificationProperties.ToString());
        }
    }
}

