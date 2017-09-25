using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PanelSheZhi : MonoBehaviour {

	public Toggle music;
	public Toggle yuyin;
	public Toggle yinxiao;
	public Toggle zhendong;

    private static bool isOpened = false;

   // public Toggle yinxiaotoggle,yinliangtoggle;

	void Start() {
		music.onValueChanged.AddListener (onMusicHandler);
		yuyin.onValueChanged.AddListener (onYunYinHandler);
		yinxiao.onValueChanged.AddListener (onYinXiaoHandler);
		zhendong.onValueChanged.AddListener (onZhendDongHandler);

        if (isOpened == false)
        {
            isOpened = true;
            music.isOn = true;
            yuyin.isOn = true;
            yinxiao.isOn = true;
            zhendong.isOn = true;
        }
        else
        {
            if (GlobalDataScript.getInstance().music)
            {
                music.isOn = true;
            }
            else
            {
                music.isOn = false;                
            }

            if (GlobalDataScript.getInstance().yuyin)
            {
                yuyin.isOn = true;
            }
            else
            {
                yuyin.isOn = false;                
            }

            if (GlobalDataScript.getInstance().yinxiao)
            {
                yinxiao.isOn = true;
            }
            else
            {
                yinxiao.isOn = false;                
            }

            if (GlobalDataScript.getInstance().zhendong)
            {
                zhendong.isOn = true;
            }
            else
            {
                zhendong.isOn = false;
            }           
        }
        music.gameObject.GetComponent<ToggleSet>().TogVulaChange();
        yuyin.gameObject.GetComponent<ToggleSet>().TogVulaChange();
        zhendong.gameObject.GetComponent<ToggleSet>().TogVulaChange();
        yinxiao.gameObject.GetComponent<ToggleSet>().TogVulaChange();
    }
  
	private void onMusicHandler(bool check){

        SoundCtrl.getInstance ().playSoundByActionButton (1);
        //print("check***********" + check);
		GlobalDataScript.getInstance ().music = check;
		if (!check) {
			SoundCtrl.getInstance ().StopaudioS ();
		} else {
			SoundCtrl.getInstance ().StartaudioS ();
		}
	}

	private void onYunYinHandler(bool check){
		GlobalDataScript.getInstance ().yuyin = check;
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		if (!check) {
			SoundCtrl.getInstance ().StopSound ();
		} else {
			SoundCtrl.getInstance ().StartSound ();
		}
	}

	private void onYinXiaoHandler(bool check){
		GlobalDataScript.getInstance ().yinxiao = check;
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		if (!check) {
			SoundCtrl.getInstance ().stopSoundByAction ();
		} else {
			SoundCtrl.getInstance ().startSoundByAction ();
		}
	}

	private void onZhendDongHandler(bool check){
		GlobalDataScript.getInstance ().zhendong = check;
		SoundCtrl.getInstance ().playSoundByActionButton (1);
		if (check) {
           // Handheld.Vibrate();
        }

	}

	public void onExitClick() {
		Destroy (this);
		Destroy (gameObject);
	}

	void Update() {

	}
    
}
