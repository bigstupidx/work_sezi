using System;

namespace AssemblyCSharp
{
	public class SZRoomChatRequest : ClientRequest
	{
		public SZRoomChatRequest (string msg)
		{
			headCode = APIS.SE_RoomChat_Request;
			messageContent = msg;
		}
	}
}

