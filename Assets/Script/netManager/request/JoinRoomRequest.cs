using System;

namespace AssemblyCSharp
{
	public class JoinRoomRequest : ClientRequest
	{
		public JoinRoomRequest (string sendMsg)
		{
			headCode = APIS.Denter_game_request;
			messageContent = sendMsg;
		}
	}
}

