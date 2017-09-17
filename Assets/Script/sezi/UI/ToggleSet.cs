using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ToggleSet : MonoBehaviour {

    public GameObject SetOffmusic;
    public GameObject SetOnmusic;
    public Toggle mytoggle;

	
    public void TogVulaChange()
    {
        if (mytoggle.isOn)
        {
            SetOffmusic.SetActive(true);
            SetOnmusic.SetActive(false);
        }
        else
        {
            SetOffmusic.SetActive(false);
            SetOnmusic.SetActive(true);
        }
    }
}
