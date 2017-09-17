using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class QiduiScripts : MonoBehaviour {
    public Toggle kehuqidui;
    public GameObject huangjinqidui, huangjinqidui1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (kehuqidui.isOn)
        {
            huangjinqidui.SetActive(false);
            huangjinqidui1.SetActive(true);
        }
        else
        {
            huangjinqidui.SetActive(true);
            huangjinqidui1.SetActive(false);
        }
	}
}
