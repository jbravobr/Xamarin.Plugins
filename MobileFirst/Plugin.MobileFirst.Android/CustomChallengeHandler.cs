using System.Collections.Generic;
using System.Diagnostics;
using Worklight;

namespace Plugin.MobileFirst.Abstractions
{
    /// <summary>
    /// Create a Challenge to a Realm.
    /// </summary>
    public class CustomChallengeHandler : ChallengeHandler
    {
        /// <summary>
        /// Parameters required to submit a form.
        /// </summary>
        public LoginFormInfo LoginFormParameters { get; set; }

        /// <summary>
        /// The Realm.
        /// </summary>
        string Realm { get; set; }

        /// <summary>
        /// For a authentication Forms.
        /// </summary>
        bool _authSuccess { get; set; }

        /// <summary>
        /// For validate if is a Authenticated Adapter.
        /// </summary>
        bool _isAdapterAuth { get; set; }

        /// <summary>
        ///    If you want to set this property to true in your implementation
        ///    if the realm is protected by an FormBasedAuthenticator. When this property is
        ///    set to true, Worklight will send a response back to the server similar to submitting
        ///    a form. The LoginFormParameters property must be populated with the necessary
        ///    information to submit the form. Default:false.
        /// </summary>
        bool _shouldSubmitLoginForm { get; set; }

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="realm">MFP App Name.</param>
        public CustomChallengeHandler(string realm)
        {
            Realm = realm;
        }

        /// <summary>
        /// Return parameters for Login.
        /// </summary>
        /// <returns></returns>
        public override LoginFormInfo GetLoginFormParameters() => LoginFormParameters;

        /// <summary>
        /// Return the App Realm.
        /// </summary>
        /// <returns></returns>
        public override string GetRealm() => Realm;

        /// <summary>
        /// Return the status for the Authentication.
        /// </summary>
        /// <returns></returns>
        public override bool ShouldSubmitSuccess() => _authSuccess;

        /// <summary>
        /// Return the Adapter Authentication Core Status.
        /// </summary>
        /// <returns></returns>
        public override bool ShouldSubmitAdapterAuthentication() => _isAdapterAuth;

        /// <summary>
        /// Return if it's ready or not to submit the Form Login Information.
        /// </summary>
        /// <returns></returns>
        public override bool ShouldSubmitLoginForm() => _shouldSubmitLoginForm;

        /// <summary>
        /// This method is called whenever Worklight.ChallengeHandler.IsCustomResponse(Worklight.WorklightResponse)
        /// returns a true value. You must implement this method to handle the challenge
        /// logic, for example to display the login screen. You can handle this for a particular.
        /// <param name="challenge">The challenge that is presented by the server.</param>
        public override void HandleChallenge(WorklightResponse challenge = null)
        {
#if DEBUG
            Debug.WriteLine("We were challenged.. so we are handling it");
#endif
            var parms = new Dictionary<string, string>();
            parms.Add("j_username", "worklight");
            parms.Add("j_password", "password");

            LoginFormParameters = new LoginFormInfo("j_security_check", parms, null, 30000, "post");
            _shouldSubmitLoginForm = true;

            // This is for Adapter based authentication
            if (challenge.ResponseJSON["authRequired"] == true)
            {
                AdapterAuthenticationInfo AdapterAuthenticationParameters = new AdapterAuthenticationInfo();
                WorklightProcedureInvocationData invocationData = new WorklightProcedureInvocationData("HTTP",
                    "submitAuthentication", new object[1]);
                AdapterAuthenticationParameters.InvocationData = invocationData;
                AdapterAuthenticationParameters.RequestOptions = null;
            }
            else
            {
                _isAdapterAuth = false;
                _authSuccess = true;
            }
        }

        /// <summary>
        ///Override this method if you want to decide if Worklight.ChallengeHandler.HandleChallenge(Worklight.WorklightResponse)
        ///     will be called. Here you can parse the response from the Worklight server to
        ///     determine whether or not your custom Challenge Handler will handle the challenge.
        ///     Worklight will then call the HandleChallenge method depending on the return value.
        ///     For example, in a realm protected by a AdapterAuthenticator, the response from
        ///     the Worklight server might contain a JSON value of authRequired : true.
        /// <param name="response">The response from the Worklight server which your implementation must parse.</param>
        /// <returns>true if the response is meant for your Challenge Handler, false otherwise. Default:true.</returns>
        /// </summary>
        public override bool IsCustomResponse(WorklightResponse response)
        {
#if DEBUG
            Debug.WriteLine("Determining if its a custom response");
#endif
            if (response == null ||
                response.ResponseText == null ||
                !(response.ResponseText.Contains("j_security_check")))
                return false;

            return true;
        }

        /// <summary>
        /// Is called by the framework in case of a success
        /// </summary>
        public override void OnSuccess(WorklightResponse challenge) => Debug.WriteLine("Challenge handler success");

        /// <summary>
        /// Is called by the framework in case of a failure
        /// </summary>
        /// <param name="response"></param>
        public override void OnFailure(WorklightResponse response) => Debug.WriteLine("Challenge handler failure");
    }
}

