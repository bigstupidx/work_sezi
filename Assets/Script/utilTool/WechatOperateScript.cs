using UnityEngine;
using System.Collections;
using cn.sharesdk.unity3d;
using AssemblyCSharp;
using System.IO;
using UnityEngine.UI;
using System;
using LitJson;

/**
 * 微信操作
 */ 
public class WechatOperateScript : MonoBehaviour {
	public ShareSDK shareSdk;
	private string picPath;

   

	void Start () {
		if (shareSdk != null) {
          
			shareSdk.showUserHandler = getUserInforCallback;
			shareSdk.shareHandler = onShareCallBack;
           
		}

	}
	

	void Update () {
	
	}



	/**
	 * 登录，提供给button使用
	 * 
	 */ 
	public void login(){
    
	shareSdk.GetUserInfo(PlatformType.WeChat);
    Debug.Log( " ranger   shareSdk.GetUserInfo(PlatformType.WeChat);");
    //testLogin();

	}

	/**
	 * 获取微信个人信息成功回调,登录
	 *
	 */ 
	public void getUserInforCallback(int reqID, ResponseState state, PlatformType type, Hashtable data){
        

         Debug.Log("ranger getUserInforCallback Start");
        //这个地方log下 打印到屏幕上，然后打包apk  看点了微信以后能不能走到这
		//TipsManagerScript.getInstance ().setTips ("获取个人信息成功");

		if (data != null) {
			MyDebug.Log (data.toJson ());
			LoginVo loginvo = new LoginVo ();
			try {

				loginvo.openId = (string)data ["openid"];
				loginvo.nickName = (string)data ["nickname"];
				loginvo.headIcon = (string)data ["headimgurl"];
				loginvo.unionid = (string)data ["unionid"];
				loginvo.province = (string)data ["province"];
				loginvo.city = (string)data ["city"];
				string sex = data ["sex"].ToString ();
				loginvo.sex = int.Parse (sex);
				loginvo.IP = GlobalDataScript.getInstance ().getIpAddress ();
				String msg = JsonMapper.ToJson (loginvo);

				CustomSocket.getInstance ().sendMsg (new LoginRequest (msg));

				GlobalDataScript.loginVo = loginvo;
				GlobalDataScript.loginResponseData = new AvatarVO ();
				GlobalDataScript.loginResponseData.account = new Account ();
				GlobalDataScript.loginResponseData.account.city = loginvo.city;
				GlobalDataScript.loginResponseData.account.openid = loginvo.openId;
				MyDebug.Log(" loginvo.nickName:"+loginvo.nickName);
				GlobalDataScript.loginResponseData.account.nickname = loginvo.nickName;
				GlobalDataScript.loginResponseData.account.headicon = loginvo.headIcon;
				GlobalDataScript.loginResponseData.account.unionid = loginvo.city;
				GlobalDataScript.loginResponseData.account.sex = loginvo.sex;
				GlobalDataScript.loginResponseData.IP = loginvo.IP;
                Debug.Log("ranger getUserInforCallbackStop");
			} catch (Exception e) {
				MyDebug.Log ("微信接口有变动！" + e.Message);
				TipsManagerScript.getInstance ().setTips ("请先打开你的微信客户端");
				return;
			}
		} else {
			TipsManagerScript.getInstance ().setTips ("微信登录失败");
		}




	}


	/***
	 * 分享战绩成功回调
	 */ 
	public void onShareCallBack(int reqID,ResponseState state,PlatformType type,Hashtable result){
		if (state == ResponseState.Success) {
			//TipsManagerScript.getInstance ().setTips ("分享成功");

		} else if(state == ResponseState.Fail){
			MyDebug.Log ("shar fail :" + result ["error_msg"]);
		}
	}

	/**
	 * 分享战绩、战绩
	 */ 
	public void shareAchievementToWeChat(PlatformType platformType){
		StartCoroutine(GetCapture(platformType));
	}

	/**
	 * 执行分享到朋友圈的操作
	 */ 
	private void shareAchievement(PlatformType platformType){
		ShareContent customizeShareParams = new ShareContent();
		customizeShareParams.SetText("");
		customizeShareParams.SetImagePath(picPath);
		customizeShareParams.SetShareType(ContentType.Image);
		customizeShareParams.SetObjectID("");
		customizeShareParams.SetShareContentCustomize(platformType, customizeShareParams);
		shareSdk.ShareContent (platformType, customizeShareParams);
	}

	/**
	 * 截屏
	 * 
	 * 
	 */ 
	private IEnumerator GetCapture(PlatformType platformType)
	{
		yield return new WaitForEndOfFrame();
		if(Application.platform==RuntimePlatform.Android || Application.platform==RuntimePlatform.IPhonePlayer)  

			picPath=Application.persistentDataPath;  

		else if(Application.platform==RuntimePlatform.WindowsPlayer)  

			picPath=Application.dataPath;  

		else if(Application.platform==RuntimePlatform.WindowsEditor)

		{  
			picPath=Application.dataPath;  
			picPath= picPath.Replace("/Assets",null);  
		}   

		picPath = picPath + "/screencapture.png";

		MyDebug.Log ("picPath:" + picPath);

		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(width,height,TextureFormat.RGB24,false);
		tex.ReadPixels(new Rect(0,0,width,height),0,0,true);
		tex.Apply ();
		byte[] imagebytes = tex.EncodeToPNG();//转化为png图
		tex.Compress(false);//对屏幕缓存进行压缩
		MyDebug.Log("imagebytes:"+imagebytes);
		if (File.Exists (picPath)) {
			File.Delete (picPath);
		}
		File.WriteAllBytes(picPath,imagebytes);//存储png图
		Destroy(tex);
		shareAchievement(platformType);
	}

    //ws分享好友
    public void FenxiangFriend()
    {
        string str = "骰子王 等你来战！ ";
        string title = "  大儒网络  ";
        ShareContent customizeShareParams = new ShareContent();
        customizeShareParams.SetTitle(title);
        customizeShareParams.SetText(str);
        //配置下载地址 
        //customizeShareParams.SetUrl("https://fir.im/ccainiu");
        customizeShareParams.SetUrl("https://fir.im/seziwang");
        //配置分享logo       
       // customizeShareParams.SetImageUrl("http://wx.qlogo.cn/mmopen/mz3Mk129TGDsZL3qND93cKC2JrXtKer8wdwHSnA1CnUaVZ6rLWoMTeFABMwv132BVEeWvPPpCMQBKt33k7vJBMwFnecrjwlia/0");
        customizeShareParams.SetImageUrl(APIS.webUrl + "shaiziwang.png");
        customizeShareParams.SetShareType(ContentType.Webpage);
        customizeShareParams.SetObjectID("");
        shareSdk.ShowShareContentEditor(PlatformType.WeChat, customizeShareParams);
        MyDebug.Log("分享微信好友已经启用");
    }
    //分享到朋友圈
    public void shareAchievement()
    {
        string str = "骰子王 一起来战！ ";
        string title = "  大儒网络  ";

        ShareContent customizeShareParams = new ShareContent();
        customizeShareParams.SetTitle(title);
        customizeShareParams.SetText(str);
        //配置下载地址 
        //customizeShareParams.SetUrl("https://fir.im/ccainiu");
        customizeShareParams.SetUrl("https://fir.im/seziwang");
        //配置分享logo  
        customizeShareParams.SetImageUrl(APIS.webUrl + "shaiziwang.png");
        //customizeShareParams.SetImageUrl("http://wx.qlogo.cn/mmopen/mz3Mk129TGDsZL3qND93cKC2JrXtKer8wdwHSnA1CnUaVZ6rLWoMTeFABMwv132BVEeWvPPpCMQBKt33k7vJBMwFnecrjwlia/0");
        customizeShareParams.SetShareType(ContentType.Image);
        customizeShareParams.SetObjectID("");
        shareSdk.ShowShareContentEditor(PlatformType.WeChatMoments, customizeShareParams);
    }
    //ws 分享房间号
    public void inviteFriend()
    {
        if (GlobalDataScript.roomVo.roomId != null)
        {
            RoomCreateVo roomvo = GlobalDataScript.roomVo;
            GlobalDataScript.totalTimes = roomvo.roundNumber;
            GlobalDataScript.surplusTimes = roomvo.roundNumber;
            string str = "骰子王 不服来战！ ";


            string title = "  我在骰子王  " + GlobalDataScript.roomVo.roomId + "房间，快来一起玩吧";
            ShareContent customizeShareParams = new ShareContent();
            customizeShareParams.SetTitle(title);
            customizeShareParams.SetText(str);

            //配置下载地址 
            //customizeShareParams.SetUrl("https://fir.im/ccainiu");
            customizeShareParams.SetUrl("https://fir.im/seziwang");
            //配置分享logo
            // customizeShareParams.SetImageUrl("http://112.74.52.173/web/images/logo.jpg");
            customizeShareParams.SetImageUrl(APIS.webUrl + "shaiziwang.png");
            //customizeShareParams.SetImageUrl("http://wx.qlogo.cn/mmopen/mz3Mk129TGDsZL3qND93cKC2JrXtKer8wdwHSnA1CnUaVZ6rLWoMTeFABMwv132BVEeWvPPpCMQBKt33k7vJBMwFnecrjwlia/0");
            customizeShareParams.SetShareType(ContentType.Webpage);
            customizeShareParams.SetObjectID("");
            shareSdk.ShowShareContentEditor(PlatformType.WeChat, customizeShareParams);
            MyDebug.Log("邀请微信好友已经启用");
        }
    }


    private void testLogin(){
		CustomSocket.getInstance().sendMsg(new LoginRequest(null));
	}

}
