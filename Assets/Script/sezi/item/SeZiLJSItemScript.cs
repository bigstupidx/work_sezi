using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SeZiLJSItemScript : MonoBehaviour {

    public Image headerIcon;
    public Text nameText;
    public Text cPointTxt;
    public Text tPointTxt;
    public Image dayingjiaImg;

    private string _headIcon;
    private bool isShowDYJ;

    // Use this for initialization
    void Start () {
        dayingjiaImg.gameObject.SetActive(isShowDYJ);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setData(string head,string uname,int cpot, int tpt, bool showDYJ)
    {
        isShowDYJ = showDYJ;
        _headIcon = head;
        dayingjiaImg.gameObject.SetActive(showDYJ);
        nameText.text = uname;
        cPointTxt.text = cpot + "";
        tPointTxt.text = tpt + "";
        StartCoroutine(LoadImg());
        
    }

    /// <summary>
    /// 加载头像
    /// </summary>
    /// <returns>The image.</returns>
    private IEnumerator LoadImg()
    {


        if (FileIO.wwwSpriteImage.ContainsKey(_headIcon))
        {
            headerIcon.sprite = FileIO.wwwSpriteImage[_headIcon];
            yield break;
        }

        //开始下载图片
        WWW www = new WWW(_headIcon);
        yield return www;
        //下载完成，保存图片到路径filePath
        if (www != null)
        {
            Texture2D texture2D = www.texture;
            byte[] bytes = texture2D.EncodeToPNG();

            //将图片赋给场景上的Sprite
            Sprite tempSp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));
            headerIcon.sprite = tempSp;
            headerIcon.gameObject.SetActive(true);
            Sprite _isalready = FileIO.wwwSpriteImage[_headIcon];

            if (_isalready == null)
            {
                FileIO.wwwSpriteImage.Add(_headIcon, tempSp);
            }

        }
        else
        {
            //MyDebug.Log("没有加载到图片");
        }
    }

}
