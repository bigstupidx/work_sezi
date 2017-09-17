using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using AssemblyCSharp;
public class PaiHangItem : MonoBehaviour {
    public int id;
    public Image headicon;
    public Text nickname;
    public Text time;
    public Text winnum;


    public  void SetUI(string _winnum,string headicon) 
    {      
        winnum.text = _winnum;
        StartCoroutine(LoadImg(headicon));
    }
    Texture2D texture2D;         //下载的图片
    private IEnumerator LoadImg(string headIcon)
    {
        //开始下载图片
        if (headIcon != null && headIcon != "")
        {
            if (FileIO.wwwSpriteImage.ContainsKey(headIcon))
            {
                headicon.sprite = FileIO.wwwSpriteImage[headIcon];
                yield break;
            }

            WWW www = new WWW(headIcon);
            yield return www;
            //下载完成，保存图片到路径filePath
            try
            {
                texture2D = www.texture;
                byte[] bytes = texture2D.EncodeToPNG();
                //将图片赋给场景上的Sprite
                Sprite tempSp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));
                headicon.sprite = tempSp;
                FileIO.wwwSpriteImage.Add(headIcon, tempSp);
            }
            catch (Exception e)
            {

                MyDebug.Log("LoadImg" + e.Message);
            }
        }
    }
}
