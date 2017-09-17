using UnityEngine;
using System.Collections;

public class PanlelShouChong : MonoBehaviour {


    public void EnterShop()
    {
        if (GlobalDataScript.shouchongrugou)
        {
            GameObject obj = PrefabManage.loadPerfab("Prefab/YueqinPanel/Panel_Shop");
        }
        else
        {
            PrefabManage.loadPerfab("Prefab/YueqinPanel/Panel_Coin");
        }
        
        Destroy(gameObject);
    }

    public void ExitPanel() 
    {
        Destroy(gameObject);
    }
}
