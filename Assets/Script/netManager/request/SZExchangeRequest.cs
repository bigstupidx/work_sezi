using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyCSharp
{
    class SZExchangeRequest : ClientRequest
    {
        public SZExchangeRequest(string sendMsg)
        {
            headCode = APIS.SZ_Exchange_Request;
            messageContent = sendMsg;
        }
    }
}
