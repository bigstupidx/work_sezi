using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Collections.Generic;
using DG.Tweening;
using LitJson;
using UnityEngine.EventSystems;

public class PlayerItemScript : MonoBehaviour
{

    public static int SHOW_CHAT_TIME = 100;

    public GameObject itemContainer;
    public Animator shockAnimator;
    public effbiankuang effect_kuang;
	public bool isReady = false;
	public Image zhichi_img;//竞猜支持小手
	public Image goldImg;//金币图标
	//public Image kaImg;	//开的图片
    public Image ke_img;//外面的壳，开牌
    public GameObject openHand_img;
    public Image zhongjie_img;//终结
    public Image lianShen_img;//连胜
	public Image kuang_img;//边框
	public Image maskImg;//遮罩
    public Image headerIcon;//头像
    public Image bankerImg;//庄家
    public Image chatImg;//聊天
    public Text nameText;//名字
    public Image readyImg;//准备
    public Text scoreText;//积分
    public Text winGold_txt;//获利字段
    //public Text offline;//离线字样
    public Image offlineImage;//离线图片
	public Image guanzhanImage;//观战
    public Text dingText;//顶的数量

    public GameObject chatPaoPao;
    public GameObject chatAction;
    public GameObject HuFlag;//豹子/顺子的大图片
	public Image myshunzi_img;//自己豹子/顺子小图

    private AvatarVO avatarvo;
    private int showTime;
    private int showChatTime;
    public string dir;

    private int shockTimes = 50;
    private int[] mypoints;
    private int openpoint;

	private Vector3 oldPosition;

    public List<Image> Pointlist = new List<Image> ();

	public List<Sprite> splist = new List<Sprite> ();

	//特别注意，豹子很顺子是不能变成灰色的
	private bool isShunZi = false;//是不是顺子
	private bool isBaoZi = false;//是不是豹子

    // Use this for initialization
    void Start()
    {
        if (kuang_img != null) {
			kuang_img.transform.gameObject.SetActive (false);
		}
        if (openHand_img != null)
        {
            openHand_img.transform.gameObject.SetActive(false);
        }

        shockAnimator.Stop();
        shockAnimator.enabled = false;
        shockAnimator.gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);

        if (effect_kuang != null)
        {
            effect_kuang.reset(false);
        }   
		if (zhichi_img != null) {
			zhichi_img.gameObject.SetActive (false);
		}

		oldPosition = ke_img.transform.localPosition;
        myshunzi_img.gameObject.SetActive(false);
        goldImg.gameObject.SetActive(false);
		//guanzhanImage.gameObject.SetActive (false);
        offlineImage.gameObject.SetActive(false);

        winGold_txt.text = "";
    }

	//显示点数
	public void showPoint(int[] ponits,int openPoint = -1, bool reconnect = false) {

        mypoints = ponits;
        openpoint = openPoint;
        if (reconnect)
        {
            showMyPoints();
            return;
        }
        if (openpoint > 0)
        {
            showMyPoints();
        }
        else
        {
            if (shockAnimator.enabled == false)
            {
                showMyPoints();
            }
        }
	}

    private void showMyPoints()
    {

        if (mypoints == null || mypoints.Length == 0)
        {
            return;
        }
		if (openpoint <= 0 && (avatarvo == null || avatarvo.account.uuid != GlobalDataScript.loginResponseData.account.uuid))
		{
			return;
		}

        maskImg.gameObject.SetActive(false);

        List<int> list = new List<int>();
        int baoziNum = 0;

		if (openpoint > 0) {
			myshunzi_img.gameObject.SetActive (false);
            ke_img.gameObject.SetActive(false);
		}

        for (int i = 0; i < mypoints.Length; i++)
        {

            Pointlist[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);

            Pointlist[i].sprite = splist[(mypoints[i] - 1)];
			//判断顺子
			if (list.IndexOf(mypoints[i]) != -1)
			{
				isShunZi = false;
			}
			//判断豹子,豹子不受有没有叫1的影响
			if (openpoint > 0) {
				if ((mypoints [i] == openpoint) || (mypoints [i] == 1)) {
					baoziNum++;
				}
			} else {
				if ((mypoints[i] == openpoint))
				{
					baoziNum++;
				}
			}
			list.Add(mypoints[i]);
        }
        if (isShunZi)
        {
            //顺子显示图片
			if (openpoint <= 0) {
				myshunzi_img.gameObject.SetActive (true);
				myshunzi_img.sprite = Resources.Load ("sizi/Battle_0/shunzi_myicon", typeof(Sprite)) as Sprite;
			} else {
				HuFlag.GetComponent<Image>().sprite = Resources.Load("sizi/Battle_0/shunzi", typeof(Sprite)) as Sprite;
				SoundCtrl.getInstance().playSoundByAction("shunzi");
			}
        }
        else
        {
            if (baoziNum == 5)
            {
                //豹子，显示图片
				if (openpoint <= 0) {
					myshunzi_img.gameObject.SetActive (true);
					myshunzi_img.sprite = Resources.Load ("sizi/Battle_0/baozi_myicon", typeof(Sprite)) as Sprite;
				} else {
					HuFlag.GetComponent<Image>().sprite = Resources.Load("sizi/Battle_0/baozi", typeof(Sprite)) as Sprite;
					SoundCtrl.getInstance().playSoundByAction("baozi");
					isBaoZi = true;
				}
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shockAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            shockTimes++;
            if (shockTimes >= 50 && shockAnimator.enabled == true)
            {
                shockAnimator.Stop();
                shockAnimator.enabled = false;
                shockAnimator.gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
				showMyPoints();

				if (GlobalDataScript.gamePlayPanel != null && GlobalDataScript.roomVo.isGoldRoom) {
					GlobalDataScript.gamePlayPanel.GetComponent<SeZiLogicScript> ().updateCallPointButton ();
				}
            }
        }

        if (GlobalDataScript.sendedOpenSZRequest == false)
        {
			if (GlobalDataScript.getInstance ().gameStart) {
				DesktopInput ();
				if (Input.GetMouseButtonDown (1) || (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved)) {
					#if !UNITY_EDITOR && ( UNITY_IOS || UNITY_ANDROID )
					MobileInput(); 
					#else
					DesktopInput ();
					#endif
				}
			}

        }
        

		if (showTime > 0) {
			showTime--;
			if (showTime <= 0) {
				chatPaoPao.SetActive (false);
			}
		}

		if (showChatTime > 0) {
			showChatTime--;
			if (showChatTime <= 0) {
				chatAction.SetActive (false);
			}
		} 
    }

	private void DesktopInput(){
		if (Input.GetMouseButtonDown(1) && GlobalDataScript.sendedOpenSZRequest == false)
        {
			if (EventSystem.current.IsPointerOverGameObject ()) {
                bool isGetUILayer = IsPointerUIObject(Input.mousePosition);
				if (isGetUILayer) {
                    if (GlobalDataScript.canOpenSZ)
                    {
                        sendOpenRequest();
                    }
                    else
                    {
                        TipsManagerScript.getInstance().setTips("自己不能开自己哦哦哦哦哦哦");
                    }
                }
			} 
        }
	}

    public bool IsPointerUIObject(Vector2 Pos)
    {
        bool isGetUILayer = false;
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Pos;
        eventDataCurrentPosition.pressPosition = Pos;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.name == "ke_img")
            {
                isGetUILayer = true;
                break;
            }
        }
        return isGetUILayer;
    }

    private void MobileInput(){
		if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)){
            bool isGetUILayer = IsPointerUIObject(Input.mousePosition);
			if (isGetUILayer) {
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                    ke_img.transform.Translate(oldPosition.x, touchDeltaPosition.y, 0);
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    if (Input.touchCount > 0 && (avatarvo != null && (avatarvo.account.uuid == GlobalDataScript.loginResponseData.account.uuid ||
                        avatarvo.account.id == GlobalDataScript.loginResponseData.account.uuid)))
                    {
                        if (ke_img.transform.localPosition.y >= 20 && GlobalDataScript.sendedOpenSZRequest == false)
                        {
                            sendOpenRequest();
                        }
                        ke_img.transform.localPosition = oldPosition;
                    }
                }
            }
		}
	}

	private void sendOpenRequest(){

		if (!GlobalDataScript.canOpenSZ)
		{
			TipsManagerScript.getInstance().setTips("自己不能开自己哦哦哦哦哦哦");
			return;
		}

		//发送消息
		SZRoomChatVO chatVo = new SZRoomChatVO();
		chatVo.id = 5;
		chatVo.msg = "";
		string sendmsgstr1 = JsonMapper.ToJson(chatVo);
		CustomSocket.getInstance().sendMsg(new SZRoomChatRequest(sendmsgstr1));

		//开骰子
		GlobalDataScript.sendedOpenSZRequest = true;
		CustomSocket.getInstance().sendMsg(new SEOpenPlayerRequest());

		if (GlobalDataScript.currentBeiShu >= 2)
		{
			SoundCtrl.getInstance().playSoundByAction("qiangkai");
		}
		else
		{
			SoundCtrl.getInstance().playSoundByAction("kai");
		}
	}

    public void setAvatarVo(AvatarVO value)
    {
        if (value != null)
        {
            avatarvo = value;
            if (!GlobalDataScript.roomVo.isGoldRoom)
            {
                readyImg.enabled = avatarvo.isReady;
            }
            //bankerImg.enabled = avatarvo.main;
            nameText.text = avatarvo.account.nickname;
			if (GlobalDataScript.roomVo.isGoldRoom)
            {
				scoreText.text = GlobalDataScript.getInstance().chageNumToW(avatarvo.account.gold);
                goldImg.sprite = Resources.Load("sizi/goldpk_doudou", typeof(Sprite)) as Sprite;
            }
            else
            {
                scoreText.text = avatarvo.scores + "";
                goldImg.sprite = Resources.Load("sizi/fangkapk_flyicon", typeof(Sprite)) as Sprite;
            }
           
//            offlineImage.transform.gameObject.SetActive(!avatarvo.isOnLine);
            StartCoroutine(LoadImg());

        }
        else {
            nameText.text = "";
            readyImg.enabled = false;
            //bankerImg.enabled = false;
            if (GlobalDataScript.roomVo.isGoldRoom)
            {
                scoreText.text = GlobalDataScript.getInstance().chageNumToW(avatarvo.gold) ;
            }
            else
            {
                scoreText.text = avatarvo.scores + "";
            }
           // scoreText.text = "";
            readyImg.enabled = false;
			avatarvo = null;
            //			SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer> ();
            //			Texture2D texture =(Texture2D)Resources.Load ("Image/gift");
            //			Sprite sp = Sprite.Create (texture, spr.sprite.textureRect, new Vector2 (0.5f, 0.5f));
            headerIcon.sprite = Resources.Load("Image/morentouxiang", typeof(Sprite)) as Sprite;
        }
    }

    public AvatarVO getAvatarVo()
    {
        return avatarvo;
    }

    /// <summary>
    /// 加载头像
    /// </summary>
    /// <returns>The image.</returns>
    private IEnumerator LoadImg()
    {


        if (FileIO.wwwSpriteImage.ContainsKey(avatarvo.account.headicon))
        {
            headerIcon.sprite = FileIO.wwwSpriteImage[avatarvo.account.headicon];
            yield break;
        }

        //开始下载图片
        WWW www = new WWW(avatarvo.account.headicon);
        yield return www;
        //下载完成，保存图片到路径filePath
        if (www != null)
        {
            Texture2D texture2D = www.texture;
            byte[] bytes = texture2D.EncodeToPNG();

            //将图片赋给场景上的Sprite
            Sprite tempSp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));
            headerIcon.sprite = tempSp;
            Sprite _isalready = FileIO.wwwSpriteImage[avatarvo.account.headicon]; 
           
            if (_isalready == null)
            {
                FileIO.wwwSpriteImage.Add(avatarvo.account.headicon, tempSp);
            }
            
        }
        else {
            MyDebug.Log("没有加载到图片");
        }
    }

    public void setbankImgEnable(bool flag)
    {
        //bankerImg.enabled = flag;

    }
    
    //播放cd动画
    public void showBetCDAnimation()
    {
		if (effect_kuang != null)
        {
            effect_kuang.reset(true);
        }
        
    }

    //显示盈利数
    public void showWinNum(int gold)
    {
        winGold_txt.text = "+ " + gold.ToString();
    }

    //播放摇色子的动画
    public void playShockSeZiAnimation()
    {
        maskImg.gameObject.SetActive(true);
        ke_img.gameObject.SetActive(true);
        shockTimes = 0;
        openpoint = -1;
        shockAnimator.enabled = false;
        shockAnimator.enabled = true;
    }

	//播放飞金币动画
	public void playThrowGoldAnimation(GameObject targetObj)
	{
		if (GlobalDataScript.roomVo.isGoldRoom) {
			SeZiGlobalData.getMe ().flyGoldOrPK (goldImg.gameObject,targetObj,gameObject,2,5);
		} else {
			SeZiGlobalData.getMe ().flyGoldOrPK (goldImg.gameObject,targetObj,gameObject);
		}
	}

	//显示观战中的图片
	public void showGuanZhanImg(bool show)
	{
        if (GlobalDataScript.roomVo.isGoldRoom)
        {
            itemContainer.SetActive(!show);
            guanzhanImage.gameObject.SetActive(show);
        }       
	}

    public void showChatAction()
    {
        showChatTime = 120;
        chatAction.SetActive(true);
    }

	public void startGame(bool value) {
		if (kuang_img != null) {
			kuang_img.enabled = value;
			kuang_img.gameObject.SetActive (value);
		}
		if (effect_kuang != null)
        {
			effect_kuang.reset(value);
        }
        winGold_txt.text = "";
        dingText.text = "";
        zhichi_img.gameObject.SetActive(false);
        if (lianShen_img)
        {
            lianShen_img.gameObject.SetActive(false);
        }
    }

	//获取uuid
    public int getUuid()
    {
        int result = -1;
        if (avatarvo != null)
        {
            result = avatarvo.account.uuid;
        }
        return result;
    }

	//清理，不知道干啥用的
    public void clean()
    {
        Destroy(headerIcon.gameObject);
        //Destroy(bankerImg.gameObject);
        Destroy(nameText.gameObject);
        Destroy(readyImg.gameObject);
		Destroy(myshunzi_img.gameObject);
		Destroy(guanzhanImage.gameObject);
		Destroy(maskImg.gameObject);
		Destroy(HuFlag.gameObject);
		Destroy (chatAction);
		Destroy (chatPaoPao);
    }

    /**设置游戏玩家离线**/
    public void setPlayerOffline()
    {
        offlineImage.transform.gameObject.SetActive(true);
    }

	//是不是准备了，处理一些Image
	public void setIsReady(bool value) {
		isShunZi = isBaoZi = false;
		isReady = value;
        if (GlobalDataScript.roomVo.isGoldRoom)
        {
            readyImg.enabled = false;
            readyImg.transform.gameObject.SetActive(false);
        }
        else
        {
            readyImg.enabled = value;
            readyImg.transform.gameObject.SetActive(value);
        }
		
        gameObject.SetActive(true);
        if (GlobalDataScript.getInstance().reConnect)
        {
            maskImg.gameObject.SetActive(false);
        }
        else
        {
            maskImg.gameObject.SetActive(value);
        }
        
		HuFlag.gameObject.SetActive (false);
		myshunzi_img.gameObject.SetActive (false);
        if (!GlobalDataScript.roomVo.isGoldRoom)
        {
            guanzhanImage.gameObject.SetActive(false);
        }
		
        if (chatAction)
        {
            chatAction.SetActive(false);
            chatPaoPao.SetActive(false);
        }
		
		showTime = showChatTime = 0;
    }

	//离开房间
	public void leaveRoom() {
		if (kuang_img != null) {
			kuang_img.transform.gameObject.SetActive (false);
		}
		headerIcon.sprite = null;
		if (offlineImage != null) {
			offlineImage.transform.gameObject.SetActive(false);
		}
		setIsReady (false);
		readyImg.transform.gameObject.SetActive(false);
		avatarvo = null;
		showTime = showTime = 0;
		//chatMessage.text = "";
		nameText.text = "";
		scoreText.text = "";
	}

    /**设置游戏玩家上线**/
    public void setPlayerOnline()
    {
       offlineImage.transform.gameObject.SetActive(false);
    }

	//显示聊天
	public void showChatMessage(int msgType,string msgContent)
    {
        
		string newContent = msgContent;
		if (msgType == (int)CHAT_TYPE.TEXT_TYPE) {
            showChatTime = SHOW_CHAT_TIME;
			if (msgContent.Length > 5) {
				newContent = msgContent.Substring (0, 5) + "...";
			}
            chatPaoPao.SetActive(true);
            chatPaoPao.GetComponentInChildren<Text>().text = newContent;
            chatPaoPao.transform.GetChild(1).GetComponent<Text>().text = newContent;
        }
        else if (msgType == (int)CHAT_TYPE.DUANYU_TYPE) {
            showChatTime = SHOW_CHAT_TIME;
            msgContent = GlobalDataScript.dyChatList[int.Parse(msgContent)];
			if (msgContent.Length > 5) {
				newContent = msgContent.Substring (0, 5) + "...";
			}
			SoundCtrl.getInstance ().playSoundByAction ("quick_chat_" + msgContent );
            chatPaoPao.SetActive(true);
            chatPaoPao.GetComponentInChildren<Text>().text = newContent;
            chatPaoPao.transform.GetChild(1).GetComponent<Text>().text = newContent;
        }
        else if (msgType == (int)CHAT_TYPE.CALLPOINT_TYPE) {
            showTime = SHOW_CHAT_TIME;
			string[] arr = msgContent.Split (' ');
            if (arr.Length == 2)
            {
                chatImg.gameObject.SetActive(true);
                int index = int.Parse(arr[1]);
                chatImg.sprite = Resources.Load("sizi/size_scene/point/point_" + index, typeof(Sprite)) as Sprite;
            }
            else
            {
                chatImg.gameObject.SetActive(false);
            }
            chatAction.SetActive(true);
            chatAction.GetComponentInChildren<Text>().text = arr[0];            
        }
        else if (msgType == (int)CHAT_TYPE.FACE_TYPE) { 
            showChatTime = SHOW_CHAT_TIME;
            chatAction.SetActive(true);
            chatAction.GetComponentInChildren<Text>().text = "";
            chatImg.sprite = Resources.Load("sizi/Emoji_0/" + msgContent, typeof(Sprite)) as Sprite;
        }
		else if (msgType == (int)CHAT_TYPE.CALLKAI_TYPE)
		{
			showChatTime = SHOW_CHAT_TIME;
			chatAction.GetComponentInChildren<Text>().text = "";
			chatImg.sprite = Resources.Load("sizi/call_kai", typeof(Sprite)) as Sprite;
			chatAction.SetActive(true);
		}
    }


    public void displayAvatorIp()
    {
        //userInfoPanel.SetActive (true);
        if (avatarvo != null)
        {
            GameObject obj = PrefabManage.loadPerfab("Prefab/userInfo");
            obj.GetComponent<ShowUserInfoScript>().setUIData(avatarvo);
			SoundCtrl.getInstance().playSoundByActionButton(1);
        }
    }

    //匹配手牌
    public void showResult(int openPoint,int openNum, LittleGameOverPlayerInfo vo)
    {
        showPoint(vo.pointArr,openPoint);
        ke_img.gameObject.SetActive(false);
		maskImg.gameObject.SetActive (false);
		for (int i = 0; i < vo.pointArr.Length; i++)
        {
			//豹子和顺子不把手牌变灰色
			if (!isShunZi && !isBaoZi) {
				if (!GlobalDataScript.isCallOne) {
					if (openPoint != vo.pointArr [i] && vo.pointArr [i] != 1 ) {
						Pointlist [i].GetComponent<Image> ().color = new Color (0.2f, 0.2f, 0.2f, 1);
						//print("=============change to gray=============");
					}
				}else {
					if (openPoint != vo.pointArr [i]) {
						Pointlist [i].GetComponent<Image> ().color = new Color (0.2f, 0.2f, 0.2f, 1);
						//print("=============change to gray=============");
					}
				}
			}
        }

        if (vo.score < 0)
        {
            //输了
            if (vo.lianshen >= 5)
            {
                showLianSheng(-999);
            }
        }
        else
        {
            //赢了
            if (vo.lianshen >= 2)
            {
                //显示连胜图标
               showLianSheng(vo.lianshen);
            }
        }

		showTime = showChatTime = 0;
		if (chatAction) {
			chatAction.SetActive (false);
			chatPaoPao.SetActive (false);
		}
    }

    //显示连胜
    private int lianshen;
    private void showLianSheng(int result)
    {
        bool visible = true;
        lianshen = result;
        if (result == -999)
        {
            //被终结
            //visible = false;
            //zhongjie_img.sprite = Resources.Load("Image/morentouxiang", typeof(Sprite)) as Sprite;
		}
        lianShen_img.enabled = visible;
        lianShen_img.transform.gameObject.SetActive(visible);
		if (result >= 2)
        {
			lianShen_img.sprite = Resources.Load("sizi/liansheng/liansheng_"+ result, typeof(Sprite)) as Sprite;
        }
    }

    //显示顶的数量
    public void showdingNum()
    {
        int num = int.Parse(dingText.text.Replace("x", ""));
        num++;
        dingText.text = "x" + num.ToString();
        zhichi_img.gameObject.SetActive(true);
    }

//    public void setHuFlagDisplay()
//    {
//        HuFlag.SetActive(true);
//    }
//
//    public void setHuFlagHidde()
//    {
//        HuFlag.SetActive(false);
//    }

}

public enum CHAT_TYPE {
	TEXT_TYPE = 1,
	DUANYU_TYPE,
	CALLPOINT_TYPE,
	FACE_TYPE,
	CALLKAI_TYPE
}