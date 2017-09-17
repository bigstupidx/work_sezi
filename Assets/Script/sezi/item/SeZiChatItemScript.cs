using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SeZiChatItemScript : MonoBehaviour {

	public Text nickNameText;
    public Text infoText;
    public Image faceImg;

	// Use this for initialization
	void Start () {
	
	}

    public void setChatData(int msgType, string msgContent,string nikename)
    {
        string newContent = msgContent;
        if (nikename.Length > 5)
        {
            nikename = nikename.Substring(0,5) + "..";
        }
		nickNameText.text = nikename + ":";
        if (msgType == (int)CHAT_TYPE.TEXT_TYPE)
        {
            faceImg.gameObject.SetActive(false);
			infoText.text = msgContent;
        }
        else if (msgType == (int)CHAT_TYPE.DUANYU_TYPE)
        {
			nickNameText.text = nikename;
			infoText.text = GlobalDataScript.dyChatList[int.Parse (msgContent)];
			faceImg.gameObject.SetActive (false);
			string path = "quick_chat_" + (int.Parse (msgContent) + 1)  ;
			SoundCtrl.getInstance ().playSoundByAction (path);
        }
        else if (msgType == (int)CHAT_TYPE.FACE_TYPE)
        {
            faceImg.gameObject.SetActive(true);
			infoText.text = "";
            faceImg.sprite = Resources.Load("sizi/Emoji_0/" + msgContent, typeof(Sprite)) as Sprite;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
