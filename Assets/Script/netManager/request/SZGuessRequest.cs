using System;
using LitJson;
namespace AssemblyCSharp
{
	public class SZGuessRequest : ClientRequest
	{
		public SZGuessRequest(string msg)
		{
			headCode = APIS.SE_Guess_Request;
            messageContent = msg;
        }
	}
}

