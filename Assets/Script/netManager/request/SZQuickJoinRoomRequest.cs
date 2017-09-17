using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyCSharp
{
    public class SZQuickJoinRoomRequest : ClientRequest
    {
        public SZQuickJoinRoomRequest(string msg)
        {
            headCode = APIS.QickStart_REQUEST;
            messageContent = msg;
        }
    }

}
