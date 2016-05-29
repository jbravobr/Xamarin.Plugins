using Worklight;
using System.Json;
using System.Diagnostics;

namespace Plugin.MobileFirst.Abstractions
{
    /// <summary>
    /// Class to work with Push Notifications
    /// </summary>
    public class PushNotificationListener : WorklightPushNotificationListener
    {
        /// <summary>
        /// The Notification event.
        /// </summary>
        /// <param name="NotificationProperties">Notification properties.</param>
        /// <param name="Payload">Payload.</param>
        public void OnMessage(JsonObject NotificationProperties, JsonObject Payload)
        {
            //Do stuff here.

#if DEBUG
            Debug.WriteLine("Got notification!");
            Debug.WriteLine(NotificationProperties.ToString()); 
#endif
        }
    }
}

