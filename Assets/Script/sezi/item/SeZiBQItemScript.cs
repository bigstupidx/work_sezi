using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SeZiBQItemScript : MonoBehaviour {

    public Button bq_img;
    private int _id;

	// Use this for initialization
	void Start () {
	
	}

    public void setId(int value)
    {
        _id = value;
        string url = value < 10 ? ("sizi/Emoji_0/Emoji_0_0" + value) : ("sizi/Emoji_0/Emoji_0_" + value);
        bq_img.GetComponent<Image>().sprite = Resources.Load(url, typeof(Sprite)) as Sprite;
    }

    public int getId()
    {
        return _id;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
