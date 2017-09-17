using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyCSharp
{
    class SZReShockRequest : ClientRequest
    {
        public SZReShockRequest()
        {
            headCode = APIS.WHO_RE_SHOCK_RESPONSE;
            messageContent = "";
        }
    }
}
