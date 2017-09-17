using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyCSharp
{
    class StartGameRequest : ClientRequest
    {
        public StartGameRequest()
        {
            headCode = APIS.STARTGAME_REQUEST;
        }
    }
}
