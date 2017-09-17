using System;

namespace AssemblyCSharp
{
	public class DissoliveRoomRequest:ClientRequest
	{
		public DissoliveRoomRequest (string msg)
		{
			headCode = APIS.Disspose_Room_Request;
			messageContent = msg;
		}
	}
}

