using System;
using LitJson;
namespace AssemblyCSharp
{
	public class PengCardRequest : ClientRequest
	{
		public PengCardRequest (CardVO cardvo)
		{
			headCode = APIS.PENGPAI_REQUEST;
			messageContent = JsonMapper.ToJson (cardvo);;
		}
	}
    public class ChiCardRequest : ClientRequest
    {
        public ChiCardRequest(CardVO cardvo)
        {
            headCode = APIS.CHIPAI_REQUEST;
            messageContent = JsonMapper.ToJson(cardvo); ;
        }
    }

    public class ChiDataRequest : ClientRequest
    {
        public ChiDataRequest(CardVO cardvo)
        {
            headCode = APIS.CHIPAIDATA_REQUEST;
            messageContent = JsonMapper.ToJson(cardvo); ;
        }
    }
}

