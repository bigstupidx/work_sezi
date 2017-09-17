using UnityEngine;
using System.Collections;

public class PanelHelp : MonoBehaviour {


    public GameObject obj1;
    public GameObject obj2;

    public void setType(int type)
    {
        obj1.SetActive(false);
        obj2.SetActive(false);
        if (type == 1)
        {
            obj1.SetActive(true);
        }
        else
        {
            obj2.SetActive(true);
        }
    }

    public void ExitPanel()
    {
        Destroy(gameObject);
    }
}
