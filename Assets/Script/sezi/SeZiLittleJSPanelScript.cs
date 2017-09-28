using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SeZiLittleJSPanelScript : MonoBehaviour {

    public Button close_button;
    public List<SeZiLJSItemScript> jList;
    private List<LittleGameOverPlayerInfo> _list;
    private bool _showDYJ;

    // Use this for initialization
    void Start () {
        reAddDataToUI();
    }

    public void setData(List<LittleGameOverPlayerInfo> list,bool showDYJ)
    {
        _list = list;
        _showDYJ = showDYJ;
    }

    public void reAddDataToUI()
    {

        int maxScore = 0;
        if (_showDYJ)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                maxScore = Mathf.Max(maxScore, _list[i].totolscore);
            }
        }
       
        //print("maxScore====" + maxScore);
        if (_list != null && _list.Count > 0)
        {
            bool show = false;
            for (int i = 0; i < jList.Count; i++)
            {
                if (i < _list.Count && _list[i] != null)
                {
                    show = _showDYJ == true ? _list[i].totolscore >= maxScore : false;
                    jList[i].setData(_list[i].headIcon, _list[i].userName, _list[i].score, _list[i].totolscore, show);
                    jList[i].gameObject.SetActive(true);
                }
                else
                {
                    jList[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void onExitClick()
    {
        Destroy(this);
        Destroy(gameObject);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
