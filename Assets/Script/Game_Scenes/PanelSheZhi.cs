using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PanelSheZhi : MonoBehaviour {

	public Toggle music;
	public Toggle yuyin;
	public Toggle yinxiao;
	public Toggle zhendong;

   // public Toggle yinxiaotoggle,yinliangtoggle;

	void Start() {
		music.onValueChanged.AddListener (onMusicHandler);
		yuyin.onValueChanged.AddListener (onYunYinHandler);
		yinxiao.onValueChanged.AddListener (onYinXiaoHandler);
		zhendong.onValueChanged.AddListener (onZhendDongHandler);

		if (PlayerPrefs.HasKey ("setting_music") && PlayerPrefs.GetInt ("setting_music") == 1) {
            music.gameObject.GetComponent<ToggleSet>().TogVulaChange();
		} else {
            music.gameObject.GetComponent<ToggleSet>().TogVulaChange();
		}

		if (PlayerPrefs.HasKey ("setting_yuyin") && PlayerPrefs.GetInt ("setting_yuyin") == 1) {
            yuyin.gameObject.GetComponent<ToggleSet>().TogVulaChange();
		} else {
            yuyin.gameObject.GetComponent<ToggleSet>().TogVulaChange();
		}

		if (PlayerPrefs.HasKey ("setting_yinxiao") && PlayerPrefs.GetInt ("setting_yinxiao") == 1) {
			yinxiao.isOn = true;
		} else {
			yinxiao.isOn = false;
		}

		if (PlayerPrefs.HasKey ("setting_zhendong") && PlayerPrefs.GetInt ("setting_zhendong") == 1) {
			zhendong.isOn = true;
		} else {
			zhendong.isOn = false;
		}


	}
  
	private void onMusicHandler(bool check){

        SoundCtrl.getInstance ().playSoundByActionButton (1);
        print("check***********" + check);
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
