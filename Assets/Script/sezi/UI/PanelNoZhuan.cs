using UnityEngine;
using System.Collections;

public class PanelNoZhuan : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void onChargeClick()
    {
        PrefabManage.loadPerfab("Prefab/YueqinPanel/Panel_Shouchong");
    }

    public void onCloseClick()
    {
        Destroy(this);
		Destroy(gameObject);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
