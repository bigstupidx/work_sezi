using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyCSharp
{
    class SZChargeRequest : ClientRequest
    {
        public SZChargeRequest(string sendMsg)
        {
            headCode = APIS.SZ_Charge_Request;
            messageContent = sendMsg;
        }
    }
}
