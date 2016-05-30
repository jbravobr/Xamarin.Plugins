using Plugin.MobileFirst.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Json;
using System.Threading.Tasks;
using Worklight;

namespace Plugin.MobileFirst
{
    /// <summary>
    /// Implementation for MobileFirst
    /// </summary>
    public class MobileFirstImplementation : IMobileFirst
    {
        #region Fields

        /// <summary>
        /// Name of the connector for Push Notifications managed from the MFP Server
        /// </summary>
        string _pushAlias { get; set; }

        /// <summary>
        /// Name of the App managed from the MFP Server
        /// </summary>
        string _appRealm { get; set; }

        /// <summary>
        /// Object for the MFP understand the client call
        /// </summary>
        JsonObject _metadata = (JsonObject)JsonValue.Parse(" {\"platform\" : \"Xamarin\" } ");

        #endregion

        #region Properties

        /// <summary>
        /// Set up the Name for the App
        /// </summary>
        /// <param name="appRealm"></param>
        public void SetAppRealm(string appRealm) => _appRealm = appRealm;

        /// <summary>
        /// Set up the Name for the Push Notification alias
        /// </summary>
        /// <param name="pushAlias"></param>
        public void SetPushAlias(string pushAlias) => _pushAlias = pushAlias;

        /// <summary>
        /// Interface for the service
        /// </summary>
        public IWorklightClient _client { get; private set; }

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
                    return _client.PushService.IsPushSupported;
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
                    return _client.PushService.IsAliasSubscribed(_pushAlias);
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion

        #region Constuctors

        /// <summary>
        /// Default constructor
        /// </summary>
        public MobileFirstImplementation()
        {
            Init();
        }
        #endregion

        #region Public Functions

        /// <summary>
        /// Init Method
        /// </summary>
        /// <param name="activity"></param>
        public void Init(object activity) { }

        /// <summary>
        /// Configures the Client Instances For the Xamarin Forms App
        /// </summary>
        public void Init()
        {
            _client = Worklight.Xamarin.iOS.WorklightClient.CreateInstance();
        }

        /// <summary>
        /// Make a async connection with the Server
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Make a REST call to the Adapter on the Server
        /// </summary>
        /// <param name="adapterName">The name of the adapter</param>
        /// <param name="adapterProcedureName">The name of the procedure</param>
        /// <param name="methodType">The HTTP verb used to call the procedure</param>
        /// <returns></returns>
        public async Task<WorklightResult> RestInvokeAsync(string adapterName, string adapterProcedureName, string methodType)
        {
            var result = new WorklightResult();

            try
            {
                var uriBuilder = $"{_client.ServerUrl.AbsoluteUri}/adapters/{adapterName}/{adapterProcedureName}";
                WorklightResourceRequest rr = _client.ResourceRequest(new Uri(uriBuilder.ToString()), methodType);
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

        /// <summary>
        /// Call a procedure from Async Method
        /// </summary>
        /// <returns></returns>
        public async Task<WorklightResult> InvokeAsync(string adapterName, string adapterProcedureName)
        {
            var result = new WorklightResult();

            try
            {
                var conResp = await ConnectAsync();

                if (!conResp.Success)
                    return conResp;

                result = await InvokeProc(adapterName, adapterProcedureName);

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Register Analytics Data to the Server
        /// </summary>
        /// <returns></returns>
        public async Task<WorklightResult> SendActivityAsync(string data)
        {
            var result = new WorklightResult();

            try
            {
                var resp = await Task.Run<string>(() =>
                {
                    _client.Analytics.Send();
                    _client.LogActivity(data);

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

        /// <summary>
        /// Call subscribes for Push Notifications on the Server.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Call unsubscribes from the Push Notifications server
        /// </summary>
        /// <returns></returns>
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
            try
            {
                // Lets send a message to the server
                _client.Analytics.Log("Trying to connect to server", _metadata);

                //ChallengeHandler customCH = new CustomChallengeHandler(_appRealm);
                //_client.RegisterChallengeHandler(customCH);

                WorklightResponse task = await _client.Connect();

                // Lets log to the local client (not server)
                _client.Logger("Xamarin").Trace("Connection");
                // Write to the server the connection status
                _client.Analytics.Log("Connect response : " + task.Success);

                return task;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Unsubscribes from push notifications
        /// </summary>
        /// <returns>The push.</returns>
        private async Task<WorklightResponse> UnsubscribePush()
        {
            try
            {
                _client.Analytics.Log("Unsubscribing Push", _metadata);

                WorklightResponse task = await _client.PushService.UnsubscribeFromEventSource(_pushAlias);
                return task;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Subscribes to push notifications
        /// </summary>
        private async Task<WorklightResponse> SubscribePush()
        {
#if DEBUG
            Debug.WriteLine("Subscribing to push");
#endif

            try
            {
                _client.PushService.ReadyToSubscribe += HandleReadyToSubscribe;
                _client.PushService.InitRegistration();

                return await _client.Connect();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Handle for the subscribe method for Push Notification
        /// </summary>
        void HandleReadyToSubscribe(object sender, EventArgs a)
        {
#if DEBUG
            Debug.WriteLine("We are ready to subscribe to the notification service!!");
#endif

            try
            {
                _client.PushService.RegisterEventSourceNotificationCallback(_pushAlias, "PushAdapter", "PushEventSource", new PushNotificationListener());
                _client.PushService.SubscribeToEventSource(_pushAlias, new Dictionary<string, string>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Invokes the procedured
        /// </summary>
        /// <returns>The proc.</returns>
        private async Task<WorklightResult> InvokeProc(string adapterName, string adapterProcedureName)
        {
            var result = new WorklightResult();

            try
            {
                _client.Analytics.Log("Trying to invoking procedure...");

#if DEBUG
                Debug.WriteLine("Trying to invoke proc");
#endif

                var invocationData = new WorklightProcedureInvocationData(adapterName, adapterProcedureName, new object[] { "technology" });
                WorklightResponse task = await _client.InvokeProcedure(invocationData);

#if DEBUG
                _client.Analytics.Log("Invoke Response : " + task.Success);
#endif

                var retval = string.Empty;

                result.Success = task.Success;

                if (task.Success)
                    retval = task.ResponseJSON;
                else
                    retval = $"Failure: { task.Message}";

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