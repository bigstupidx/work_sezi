using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class MicInputRequest:ClientRequest
	{
		public MicInputRequest (byte[] sound)
		{
			headCode = APIS.MicInput_Request;
			
			ChatSound = sound;
		}
	}
}

