using System.Threading.Tasks;

namespace Plugin.MobileFirst.Abstractions
{
    /// <summary>
    /// Interface for MobileFirst
    /// </summary>
    public interface IMobileFirst
    {
        /// <summary>
        /// Init Method for iOS
        /// </summary>
        void Init();

        /// <summary>
        /// Initi Method for Android
        /// </summary>
        /// <param name="activity">Is the Android App Activity</param>
        void Init(object activity);

        /// <summary>
        /// Make a new async connection with the MFP Server
        /// </summary>
        /// <returns>WorlightResult</returns>
        Task<WorklightResult> ConnectAsync();

        /// <summary>
        /// Make a async Rest Invoke to a procedure from the MFP Server
        /// </summary>
        /// <param name="adapterName">The Name of the Adapter</param>
        /// <param name="adapterProcedureName">The Name of the Procedure</param>
        /// <param name="methodType">The Type of HTTP Verb</param>
        /// <returns>WorlightResult</returns>
        Task<WorklightResult> RestInvokeAsync(string adapterName, string adapterProcedureName, string methodType);

        /// <summary>
        /// Make a async call to a procedure from the MFP Server
        /// </summary>
        /// <param name="adapterName">The Adapter name</param>
        /// <param name="adapterProcedureName">The Procedure name</param>
        /// <returns>WorlightResult</returns>
        Task<WorklightResult> InvokeAsync(string adapterName, string adapterProcedureName);

        /// <summary>
        /// Use this to send logs and other data to the MFP Server
        /// </summary>
        /// <param name="data">Data to be sent to the MFP server</param>
        /// <returns>WorlightResult</returns>
        Task<WorklightResult> SendActivityAsync(string data);

        /// <summary>
        /// Do a new Subscription on the Push Notification server
        /// </summary>
        /// <returns>WorlightResult</returns>
        Task<WorklightResult> SubscribeAsync();

        /// <summary>
        /// Undo a existing subscription on the Push Notification server
        /// </summary>
        /// <returns></returns>
        Task<WorklightResult> UnSubscribeAsync();
    }
}
