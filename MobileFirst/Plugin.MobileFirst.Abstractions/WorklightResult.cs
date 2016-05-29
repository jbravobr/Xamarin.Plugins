namespace Plugin.MobileFirst.Abstractions
{
    /// <summary>
    /// Class for holding the result from a server call.
    /// </summary>
    public class WorklightResult
    {
        /// <summary>
        /// Is Success return.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// JSON message from server return.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Response for the server message.
        /// </summary>
        public string Response { get; set; }
    }
}

