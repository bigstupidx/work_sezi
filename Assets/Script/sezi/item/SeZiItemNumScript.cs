using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace global{ 
public class SeZiItemNumScript : MonoBehaviour {

	public int id;
	public Text button_txt;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setId(int i) {
		id = i;
		button_txt.text = "" + i;
	}

}
}
