using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
    public class QickStartRequest : ClientRequest
    {
        public QickStartRequest()
        {
            headCode = APIS.QickStart_REQUEST;
        }
    }
}
