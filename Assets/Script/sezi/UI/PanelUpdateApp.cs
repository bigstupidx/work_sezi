using UnityEngine;
using System.Collections;

public class PanelUpdateApp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onURLClick()
	{
		Application.OpenURL (GlobalDataScript.downloadPath);
		onClick ();
	}

    public void onClick()
    {
        Destroy(this);
        Destroy(gameObject);
    }

}
