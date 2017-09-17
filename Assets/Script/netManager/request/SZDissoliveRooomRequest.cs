using System;

namespace AssemblyCSharp
{
	public class SZDissoliveRooomRequest :ClientRequest
	{

		//public string msg;

		public SZDissoliveRooomRequest (string msg)
		{
			headCode = APIS.Disspose_Confirm_Request;
			messageContent = msg;
		}
	}
}

