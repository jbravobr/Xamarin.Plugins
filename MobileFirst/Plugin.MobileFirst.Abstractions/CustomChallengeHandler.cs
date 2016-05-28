using System.Collections.Generic;
using System.Diagnostics;
using Worklight;

namespace Plugin.MobileFirst.Abstractions
{
    public class CustomChallengeHandler : ChallengeHandler
    {
        public LoginFormInfo LoginFormParameters { get; set; }

        string Realm { get; set; }
        bool _authSuccess { get; set; }
        bool _isAdapterAuth { get; set; }
        bool _shouldSubmitLoginForm { get; set; }

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="realm">MFP App Name</param>
        public CustomChallengeHandler(string realm)
        {
            Realm = realm;
        }

        /// <summary>
        /// Return parameters for Login
        /// </summary>
        /// <returns></returns>
        public override LoginFormInfo GetLoginFormParameters() => LoginFormParameters;

        /// <summary>
        /// Return the App Realm
        /// </summary>
        /// <returns></returns>
        public override string GetRealm() => Realm;

        /// <summary>
        /// Return the status for the Authentication
        /// </summary>
        /// <returns></returns>
        public override bool ShouldSubmitSuccess() => _authSuccess;

        /// <summary>
        /// Return the Adapter Authentication Core Status
        /// </summary>
        /// <returns></returns>
        public override bool ShouldSubmitAdapterAuthentication() => _isAdapterAuth;

        /// <summary>
        /// Return if it's ready or not to submit the Form Login Information
        /// </summary>
        /// <returns></returns>
        public override bool ShouldSubmitLoginForm() => _shouldSubmitLoginForm;

        /// <summary>
        /// Handle for the Login Information
        /// </summary>
        /// <param name="challenge">The acctually handle response for the Login</param>
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

            //this is for Adapter based authentication
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
        /// Return if is a custom Response 
        /// </summary>
        /// <param name="response">The response from Server</param>
        /// <returns></returns>
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
        /// On Success response
        /// </summary>
        public override void OnSuccess(WorklightResponse challenge) => Debug.WriteLine("Challenge handler success");

        /// <summary>
        /// On Failure response
        /// </summary>
        /// <param name="response"></param>
        public override void OnFailure(WorklightResponse response) => Debug.WriteLine("Challenge handler failure");
    }
}

