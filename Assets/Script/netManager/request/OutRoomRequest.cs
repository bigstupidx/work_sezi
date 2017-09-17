using System;

namespace AssemblyCSharp
{
	public class OutRoomRequest :ClientRequest
	{
		
		public OutRoomRequest (string sendMsg)
		{
			headCode = APIS.Leave_Room_request;
			messageContent = sendMsg;
		}
	}
}

