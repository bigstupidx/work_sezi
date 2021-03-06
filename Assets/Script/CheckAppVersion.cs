﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using AssemblyCSharp;

public class CheckAppVersion : MonoBehaviour {
	
	public Slider slider_p;
    public Text progress_txt;
    public GameObject PanelUpdateApp;

    private string version_str = "1.0.7";

	// Use this for initialization
	void Start () {
		Invoke("checkAppVersion", 1); 		
	}

	private void checkAppVersion(){
        StartCoroutine(LoadVersionTxt());
        //destoryPanel();
	}

	private void actionBytes(string url, string downLoadPathName, byte[] bytes){
		Debug.Log("开始写入文件");
        progress_txt.text = "正在写入文件...";
        File.WriteAllBytes(downLoadPathName, bytes);
	}

	private void percentAciton(float load){
		slider_p.value = load;
        progress_txt.text = Math.Floor(load * 100) + "";
        //Debug.Log("下载===>"+Application.persistentDataPath+"：" + load);
    }

	private void endAciton(bool end, string fileNamePath, string downLoadPathName){
		InstallAPK (fileNamePath,true);
	}

	private IEnumerator LoadVersionTxt(){
		WWW www = new WWW (APIS.webUrl + "checkAppVersion.txt");
		yield return www;
		if (!string.IsNullOrEmpty(www.error)) {
			yield return null;
		}
		//0表示版本    1表示是否在审核	2表示审核的版本号
        string[] arr = www.text.Split(',');
		version_str = version_str.Replace (".","");
		string server_version = arr [0].Replace (".","");
		GlobalDataScript.downloadPath = arr [3];
		if (int.Parse (server_version) > int.Parse (version_str)) {
			if (arr [1].Equals ("APPCheck")) {
				#if UNITY_ANDROID
				GlobalDataScript.hideChargeUI = false;
				doAlertIOS ();
				#elif UNITY_IPHONE
				//GlobalDataScript.hideChargeUI = true;
				slider_p.value = 1;
				Invoke("destoryPanel", 1f);
				#endif

			} else {
				doAlertIOS ();
//				#if UNITY_ANDROID
//					doLoadApk();
//				#elif UNITY_IPHONE
//					doAlertIOS();
//				#endif
			}
		} else {
			destoryPanel();
		}
    }

    private void destoryPanel()
    {
        Destroy(this);
        Destroy(gameObject);
    }

    private void doLoadApk()
    {
        slider_p.value = 0;
        string fileName = Application.persistentDataPath + "/aaa.apk";
        string downURL = "aaa.apk";
        StartCoroutine(DownloadFile(downURL, fileName, actionBytes, percentAciton, endAciton));
    }

    private void doAlertIOS()
    {
		PanelUpdateApp.SetActive (true);
    }

    private IEnumerator DownloadFile(string url, string downLoadPathName,  Action<string, string, byte[]> actionBytes, Action<float> percentAciton, Action<bool, string, string> endAciton)
	{
		WWW www = new WWW(url);
		while (!www.isDone)
		{
			if (null != percentAciton)
			{
				//Debug.Log ("DownloadFile");
				percentAciton(www.progress);
			}

			yield return null;
		}

		if (!string.IsNullOrEmpty(www.error))
		{
			Debug.LogError("WWW DownloadFile:" + www.error);

			if(null != endAciton)
			{
				endAciton(false, www.error, downLoadPathName);
			}

			yield break;
		}

		if (null != actionBytes)
		{
			actionBytes(url, downLoadPathName, www.bytes);
		}

		if(null != endAciton)
		{
			endAciton(true, www.text, downLoadPathName);
		}

		www.Dispose();
	}

	public bool InstallAPK(string path, bool bReTry) {
		try{
			var Intent = new AndroidJavaClass("android.content.Intent");
			var ACTION_VIEW = Intent.GetStatic<string>("ACTION_VIEW");
			var FLAG_ACTIVITY_NEW_TASK = Intent.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");
			var intent = new AndroidJavaObject("android.content.Intent", ACTION_VIEW);
			 
			var file = new AndroidJavaObject("java.io.File", path);
			var Uri = new AndroidJavaClass("android.net.Uri");
			var uri = Uri.CallStatic<AndroidJavaObject>("fromFile", file);
			 
			intent.Call<AndroidJavaObject>("setDataAndType", uri, "application/vnd.android.package-archive");
			 
			if (!bReTry)
			{
				intent.Call<AndroidJavaObject>("addFlags", FLAG_ACTIVITY_NEW_TASK);
				intent.Call<AndroidJavaObject>("setClassName", "com.android.packageinstaller", "com.android.packageinstaller.PackageInstallerActivity");
			}
			 
			var UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			var currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			currentActivity.Call("startActivity", intent);
			 
			//Debug.Log("Install New Apk finish");
			return true;
		}catch (System.Exception e) {
			Debug.LogError("Error Install APK:" + e.Message + " -- " + e.StackTrace + "  bRetry=" + bReTry);
			return false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
