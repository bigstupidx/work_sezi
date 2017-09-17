using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	[Serializable]
	public class RoomJoinResponseVo
	{
		public int createrId;
		public bool addWordCard;
		public bool hong;
		public int ma;
		public string name;
		public int roomId;
		public int roomType;
		public int roundNumber;
		public int isStart;
		public int status;

		public bool sevenDouble;
		public int xiaYu;
		public int ziMo;
		public int magnification;
		public List<AvatarVO> playerList = new List<AvatarVO> ();
        public bool isGoldRoom;

		public void addAvatarVO(AvatarVO vo) {
			if (playerList.Contains(vo) == true) {
				playerList.Remove (vo);
			}
			playerList.Add (vo);
		}

		public void deleteAvatarVO() {

		}

		public int getIndex(int uuid) {
			if (playerList == null || playerList.Count == 0) {
				return 0;
			}
			for (int i = 0; i < playerList.Count; i++) {
				if (playerList[i].account.uuid == uuid) {
					return i;
				}
			}		
			return -1;
		}

		public void clear() {
			playerList.Clear ();
		}

		//public LastOperationVo lastOperationVo;
		public RoomJoinResponseVo ()
		{
		}
	}
}

