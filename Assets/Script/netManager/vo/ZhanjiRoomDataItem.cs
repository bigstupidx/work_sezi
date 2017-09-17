using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	[Serializable]
	public class ZhanjiRoomDataItem
	{
		public ZhanjiOneRoomData data;
		public int roomId;
		public ZhanjiRoomDataItem ()
		{
		}
	}
    [Serializable]
    public class RankType 
    {
        public int type;
        public RankType()
		{
		}
    }
}

