using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using LitJson;

public class PanelUseFiveProp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void onUseClick(){
		SZUsePropRequestVO vo = new SZUsePropRequestVO ();
		vo.type = 2;
		string sendMsg = JsonMapper.ToJson(vo);
		CustomSocket.getInstance().sendMsg(new SZUsePropRequest(sendMsg));
		onCloseClick ();
	}

	public void onCloseClick(){
		SoundCtrl.getInstance().playSoundByActionButton(1);
		Destroy (this);
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
