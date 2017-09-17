using System;

namespace AssemblyCSharp
{
	public class SZUsePropRequest : ClientRequest
	{
		public SZUsePropRequest (string msg)
		{
			headCode = APIS.SZ_UseProp_Request;
			messageContent = msg;
		}
	}
}

