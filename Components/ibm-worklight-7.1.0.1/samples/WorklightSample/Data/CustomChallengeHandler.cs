using System;
using System.Collections.Generic;
using Worklight;

namespace WorklightSample
{
	public class CustomChallengeHandler : ChallengeHandler
	{

		public LoginFormInfo LoginFormParameters{get;  set;}
		private bool authSuccess = false;
		private bool isAdapterAuth = false;
		private bool shouldSubmitLoginForm = false;

		public CustomChallengeHandler(string realm)
		{
			this.Realm = realm;
		}

		public override LoginFormInfo GetLoginFormParameters()
		{
			return LoginFormParameters;
		}
		//string realm;
		public string Realm;
		public override string GetRealm()
		{
			return Realm;
		}

		public override bool ShouldSubmitSuccess ()
		{
			return authSuccess;
		}
		public override bool ShouldSubmitAdapterAuthentication()
		{
			return isAdapterAuth;
		}

		public override bool ShouldSubmitLoginForm ()
		{
			return shouldSubmitLoginForm;
		}

		public override void HandleChallenge(WorklightResponse challenge)
		{
			Console.WriteLine ("We were challenged.. so we are handling it");
			Dictionary<String,String > parms = new Dictionary<String, String> (); 
			parms.Add ("j_username", "worklight");
			parms.Add ("j_password", "password"); 
			LoginFormParameters = new LoginFormInfo ("j_security_check", parms, null, 30000, "post");
			shouldSubmitLoginForm = true;
			//authSuccess = true;
			//this is for Adapter based authentication
			//            if (challenge.ResponseJSON["authRequired"] == true)
			//            {
			//				AdapterAuthenticationInfo AdapterAuthenticationParameters = new AdapterAuthenticationInfo();
			//                WorklightProcedureInvocationData invocationData = new WorklightProcedureInvocationData("HTTP", 
			//                    "submitAuthentication" , new object[1]);
			//				AdapterAuthenticationParameters.InvocationData = invocationData;
			//				AdapterAuthenticationParameters.RequestOptions = null;
			//            }
			//            else
			//            {
			//				isAdapterAuth = false;
			//				authSuccess = true;
			//            }
		}

		public override bool IsCustomResponse(WorklightResponse response)
		{
			Console.WriteLine ("Determining if its a custom response");
			if (response == null || response.ResponseText==null || !(response.ResponseText.Contains("j_security_check")))
			{
				return false;
			}

			return true;

		}

		public override void OnSuccess(WorklightResponse challenge)
		{
			Console.WriteLine("Challenge handler success");

		}

		public override void OnFailure(WorklightResponse response)
		{
			Console.WriteLine("Challenge handler failure");
		}
	}
}

