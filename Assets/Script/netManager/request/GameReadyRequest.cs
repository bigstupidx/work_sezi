using System;

namespace AssemblyCSharp
{
	public class GameReadyRequest:ClientRequest
	{
		public GameReadyRequest ()
		{
			headCode = APIS.SZ_Ready_Request;
		}
	}
}

