using System;
using LitJson;
namespace AssemblyCSharp
{
	public class PutOutCardRequest:ClientRequest
	{
		public PutOutCardRequest (String msg)
		{
			headCode = APIS.BET_REQUEST;
			messageContent = msg;
		}
	}
}

