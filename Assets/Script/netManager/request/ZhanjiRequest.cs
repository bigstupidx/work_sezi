using System;

namespace AssemblyCSharp
{
	public class ZhanjiRequest :ClientRequest
	{
		public ZhanjiRequest (string Msg)
		{
            headCode = APIS.zhanji_request;
			messageContent=Msg;
		}
	}

    public class RankRequest : ClientRequest
    {
        public RankRequest(string Msg)
        {
            headCode = APIS.rank_request;
            messageContent = Msg;
        }
    }
}

