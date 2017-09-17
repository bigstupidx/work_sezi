using System;

namespace AssemblyCSharp
{
	[Serializable]
	public class RoomCreateVo
	{
		public  bool hong;
		public int ma;
		public int createrId;
		public int roomId;
		public bool isStart;
		public int status;
		public int gametype;
		public int roomType;//1转转；2、划水；3、长沙；4广东 5 温州
        public int roomTiao;  //条数 如果是其他的发0
		/**局数**/
        public int followBank; //是否分猪肉   0不分 ； 1分 默认不分
        public int huangjinDouble; //是否黄金7对 1表示不带  0表示带
        public int isHavaFen; //是否带风  1表示不带  0表示带
		public int roundNumber;  
        public bool sevenDouble; 
        public int ziMo;// 0：可抢杠胡；1：自摸胡；2、抢杠胡 
		public int xiaYu;
		public string name;
		public bool addWordCard;
		public int magnification;
	    public int huaType;//0:小花 1：大花 2：平方花
	    public bool isGoldRoom; //是否是金币场 true 是 false 不是
		public RoomCreateVo()
		{

		}
	}
}

