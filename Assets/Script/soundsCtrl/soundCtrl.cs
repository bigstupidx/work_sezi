using UnityEngine;
using System.Collections;
/**
 * sound control class
 * 
 * author :kevin
 * 
 * */
public class SoundCtrl : MonoBehaviour
{
	private static bool isPlayCardSound = false;

    private Hashtable soudHash = new Hashtable();

    private static SoundCtrl _instance;

    private static AudioSource audioS;
    private static AudioSource numSounPlay;
    private static AudioSource ActionSounPlay;
    private static AudioSource pointSounPlay;
    public static SoundCtrl getInstance()
    {
        if (_instance == null)
        {
            _instance = new SoundCtrl();
            audioS = GameObject.Find("MyAudio").GetComponent<AudioSource>();
			numSounPlay = GameObject.Find("cardSoundPlay").GetComponent<AudioSource>();
            ActionSounPlay = GameObject.Find("ActionSound").GetComponent<AudioSource>();
			pointSounPlay = GameObject.Find("MessageSound").GetComponent<AudioSource>();
		

        }

        return _instance;
    }

	void Update()
	{
		
	}

	public void refresh()
	{
		if (isPlayCardSound) {
			if (numSounPlay && numSounPlay.isPlaying == false) {
				pointSounPlay.Play ();
				isPlayCardSound = false;
			}
		}
	}

    #region 背景音乐和音效的开关
    public void StartaudioS()
    {
        audioS.volume = 1;
    }
    public void StopaudioS()
    {
        audioS.volume = 0;
    }
    public void StartActionSound()
    {
        ActionSounPlay.volume = 1;
    }
    public void StoptActionSound()
    {
        ActionSounPlay.volume = 0;
    } 


	public void StartSound()
	{
		numSounPlay.volume = 1;
		pointSounPlay.volume = 1;
		ActionSounPlay.volume = 1;
	}
	public void StopSound()
	{
		numSounPlay.volume = 0;
		pointSounPlay.volume = 0;
		ActionSounPlay.volume = 0;
	} 

	public void startSoundByAction()
	{
		ActionSounPlay.volume = 1;
	}
	public void stopSoundByAction()
	{
		ActionSounPlay.volume = 1;
	} 

    #endregion

	//播放交点的声音
	public void playSound(int cardNum,int cardPoint, int sex = -1)
    {
		if (GlobalDataScript.getInstance().yuyin)
        {
			if (sex == -1) {
				sex = GlobalDataScript.loginResponseData.account.sex;
			}
			isPlayCardSound = true;
            string path = "Sounds/";
            if (sex == 1)
            {
                path += "boy/";
            }
            else {
                path += "girl/";
            }
			string num_path = path + "0/" + cardNum + "ge";
			string point_path = path + "0/" + cardPoint + "_up";

			print("-------------------------playSound:" + num_path+"   " + point_path);

			AudioClip temp = (AudioClip)soudHash[num_path];
            if (temp == null)
            {
				temp = GameObject.Instantiate(Resources.Load(num_path)) as AudioClip;
				soudHash.Add(num_path, temp);
            }

			AudioClip temp1 = (AudioClip)soudHash[point_path];
			if (temp1 == null)
			{
				temp1 = GameObject.Instantiate(Resources.Load(point_path)) as AudioClip;
				soudHash.Add(point_path, temp1);
			}

			numSounPlay.volume = 1;
			numSounPlay.clip = temp;
			numSounPlay.loop = false;
			numSounPlay.Play();

			pointSounPlay.volume = 1;
			pointSounPlay.clip = temp1;
			pointSounPlay.loop = false;
			pointSounPlay.Pause ();

//            if (audioS != null)
//                audioS.volume = 1;


        }
    }

    public void playMessageBoxSound(int codeIndex, int sex)
    {
        return;
        Debug.Log("----------------------------------------------------playMessageBoxSound");
        if (GlobalDataScript.soundToggle)
        {
            string path;
            if (sex == 1)
                path = "Sounds/boy/0/" + codeIndex;
            else
				path = "Sounds/girl/0/" + codeIndex;
            AudioClip temp = (AudioClip)soudHash[path];
            if (temp == null)
            {
                temp = GameObject.Instantiate(Resources.Load(path)) as AudioClip;
                soudHash.Add(path, temp);
            }
			pointSounPlay.volume = 1;
			pointSounPlay.clip = temp;
			pointSounPlay.Play();
//            if (audioS != null)
//                audioS.volume = 1;


        }
    }

    public void playBGM(int type)
    {
		if (!GlobalDataScript.getInstance().music) {
			return;
		}
        string path = "";
        switch (type)
        {
            case 0:
                audioS.loop = false;
                audioS.Stop();
                return;
            case 1:
                path = "Sounds/HallBg";
                break;
            case 2:
                path = "Sounds/fightBg";
                break;
        }
        AudioClip temp = (AudioClip)soudHash[path];
        if (temp == null)
        {
            temp = GameObject.Instantiate(Resources.Load(path)) as AudioClip;
            soudHash.Add(path, temp);
        }
        audioS.volume = 1;
        audioS.clip = temp;
        audioS.loop = true;
        audioS.Play();
        if (GlobalDataScript.soundToggle)
        {
            audioS.mute = false;
        }
        else {
            audioS.mute = true;
        }
    }



    public void playSoundByAction(string str, int sex = 0)
    {
        
		if (!GlobalDataScript.getInstance ().yuyin) {
			return;
		}
        string path = "Sounds/";
        if (sex == 1)
        {
			path += "boy/0/" + str;
        }
        else {
			path += "girl/0/" + str;
        }
        AudioClip temp = (AudioClip)soudHash[path];
		Debug.Log("----playSoundByAction::" + path);
        if (temp == null)
        {
            temp = GameObject.Instantiate(Resources.Load(path)) as AudioClip;
            soudHash.Add(path, temp);
        }

        ActionSounPlay.volume = 1;
        ActionSounPlay.clip = temp;
        ActionSounPlay.Play();
        //if (audioS != null)
        //    audioS.volume = 1;
    }



	public void playSoundByActionButton(int type)
	{
		if (!GlobalDataScript.getInstance().yinxiao) {
			return;
		}
		string path = "Sounds/other/";
		//按钮
		if (type == 1)
		{
			path += "clickbutton";
			//发牌
		}else if (type == 2)
		{
			path += "dice";
			//准备
		}else if (type == 3)
		{
			path += "ready";
			//打牌
		}else if (type == 4)
		{
			//path += "tileout";
			path += "out";
			//摸牌
		} else if (type == 5)
		{
			path += "select";
		} else if (type == 6)
		{
			//path += "tileout";
			path += "tileout";
			//摸牌
		}
		AudioClip temp = (AudioClip)soudHash[path];
		if (temp == null)
		{
			temp = GameObject.Instantiate(Resources.Load(path)) as AudioClip;
			soudHash.Add(path, temp);
		}

		ActionSounPlay.volume = 1;
		ActionSounPlay.clip = temp;
		ActionSounPlay.Play();
		//if (audioS != null && GlobalDataScript.getInstance().music)
		//	audioS.volume = 1;
	}



}
