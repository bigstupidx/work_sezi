using System;

namespace AssemblyCSharp
{
	public class SZChangeRoomRequest : ClientRequest
	{
		public SZChangeRoomRequest (string msg)
		{
			headCode = APIS.SZ_ChangeRoom_Request;
			messageContent = msg;
		}
	}
}

