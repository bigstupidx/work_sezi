using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Collections.Generic;
using System;
using LitJson;
using AssemblyCSharp;

public class SeZiChatScript : MonoBehaviour {

	public Button bq_button;
	public Button duanyu_button;
	public GameObject chatContent_Scroll;
	public GameObject bqContent_Scroll;
    public GameObject duContent_Scroll;
    public Transform chatContent_view;
	public Transform bqContent_view;
    public Transform dyContent_view;
    public Button voice_button;
    public InputField input_txt;
    public GameObject micbg;

	private int hideTime = 500;

	// Use this for initialization
	void Start () {
        bqContent_Scroll.SetActive(false);
        duContent_Scroll.SetActive(false);

        initBQ();
        initDY();
    }

    private void initDY()
    {
        for (int i = 0; i < GlobalDataScript.dyChatList.Count; i++)
        {
            GameObject item = (GameObject)Instantiate(Resources.Load("Prefab/sezi/duanyuItem"));
            item.GetComponent<SeZiDuanYuItem>().Id=i;
            item.GetComponent<SeZiDuanYuItem>().textinfo.text = GlobalDataScript.dyChatList[i];
           item.GetComponentInChildren<Button>().onClick.AddListener(delegate {
                clickDuanYu(item);
            });
            item.transform.SetParent(dyContent_view);
            item.transform.localScale = Vector3.one;
        }
    }

    private void clickDuanYu(GameObject obj)
    {
        //print("clickImg===" + obj.GetComponent<SeZiBQItemScript>().getId());
        int face_id = obj.GetComponent<SeZiDuanYuItem>().Id;
        SZRoomChatVO vo = new SZRoomChatVO();
        vo.id = 2;
        vo.msg = face_id.ToString();
        string sendmsgstr = JsonMapper.ToJson(vo);
        CustomSocket.getInstance().sendMsg(new SZRoomChatRequest(sendmsgstr));
        duContent_Scroll.SetActive(false);
    }

    private void initBQ()
    {
        for (int i = 1; i < 37; i++)
        {
            GameObject item = (GameObject)Instantiate(Resources.Load("Prefab/sezi/SeZiBQItem"));
            item.GetComponent<SeZiBQItemScript>().setId(i);
            item.name = "SeZiBQItem" + i;
            item.GetComponentInChildren<Button>().onClick.AddListener(delegate {
                clickImg(item);
            });
            item.transform.SetParent(bqContent_view);
            item.transform.localScale = Vector3.one;
        }
    }
	
    private void clickImg(GameObject obj)
    {
        bqContent_Scroll.SetActive(false);
        //print("clickImg===" + obj.GetComponent<SeZiBQItemScript>().getId());
        int face_id = obj.GetComponent<SeZiBQItemScript>().getId();
		SZRoomChatVO vo = new SZRoomChatVO();
		vo.id = 4;
		vo.msg = face_id>=10  ? ("Emoji_0_" + face_id) : ("Emoji_0_0" + face_id);
		string sendmsgstr = JsonMapper.ToJson(vo);
		CustomSocket.getInstance().sendMsg(new SZRoomChatRequest(sendmsgstr));
    }

    public void onBQClick()
    {
        duContent_Scroll.SetActive(false);
        bqContent_Scroll.SetActive(true);
    }

    public void onVoiceClickDown()
    {
        micbg.SetActive(true);
    }
    public void onVoiceClickUp()
    {
        micbg.SetActive(false);
    }
    public void onDanyuClick()
    {
        bqContent_Scroll.SetActive(false);
        duContent_Scroll.SetActive(true);
    }

	//添加聊天数据
	public void addChatMsg(int msgType,string msgContent,string nikename)
	{
		chatContent_Scroll.SetActive (true);
		hideTime = 500;
		GameObject item = (GameObject)Instantiate(Resources.Load("Prefab/sezi/SZChatItem"));
        item.GetComponent<SeZiChatItemScript>().setChatData(msgType, msgContent, nikename);
        item.transform.SetParent(chatContent_view);
        item.transform.localScale = Vector3.one;
    }

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return)) {
			if (input_txt.text != "") {
				//发送消息
				SZRoomChatVO vo = new SZRoomChatVO();
				vo.id = 1;
				vo.msg = input_txt.text;
				string sendmsgstr = JsonMapper.ToJson(vo);
				CustomSocket.getInstance().sendMsg(new SZRoomChatRequest(sendmsgstr));
                input_txt.text = "";
            }
		}

		hideTime--;
		if (hideTime <= 0) {
			chatContent_Scroll.SetActive (false);
		}
	}

    public void onSendClick()
    {
        if (input_txt.text != "")
        {
            //发送消息
            SZRoomChatVO vo = new SZRoomChatVO();
            vo.id = 1;
            vo.msg = input_txt.text;
            string sendmsgstr = JsonMapper.ToJson(vo);
            CustomSocket.getInstance().sendMsg(new SZRoomChatRequest(sendmsgstr));
            input_txt.text = "";
        }
    }

}
