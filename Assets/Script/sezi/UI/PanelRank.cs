using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using LitJson;

public class PanelRank : MonoBehaviour {
    public GameObject ScrollViewCoin,ScrollViewZhuan;
    public GameObject ButtonCoinSelect, ButtonZuanselect;
    public GameObject contentCoin, contentZhuan;
    public int icon_type;
    private bool isRequestReturn = true;

    private double requestDiamondime;
    private double requestGoldTime;

	// Use this for initialization
	void Start () {
        SocketEventHandle.getInstance().rank_response += rank_response;
        ClickRankCoin();
	}
    
    private void rank_response(ClientResponse response)
    {
        isRequestReturn = true;
        JsonData json = JsonMapper.ToObject(response.message);
        print("rank_response" + response.message);
        string message = json["ranklist"].ToString();
        string[] arr = message.Split(',');
        GameObject clone = null;

        if (icon_type==0)
        {
            for (int i = 0; i < contentCoin.transform.childCount; i++)
            {
                Destroy(contentCoin.transform.GetChild(i).gameObject);
            }
        }
        else
        {
            for (int i = 0; i < contentZhuan.transform.childCount; i++)
            {
                Destroy(contentZhuan.transform.GetChild(i).gameObject);
            }
        }
        
      
        for (int i = 0; i < arr.Length / 3; i++)
        {
            if (icon_type == 0)
            {
                clone = Instantiate(Resources.Load("Prefab/sezi/RankCoin")) as GameObject;
                clone.transform.SetParent(contentCoin.transform);
               
            }
            else
            {
                clone = Instantiate(Resources.Load("Prefab/sezi/RankZhuan")) as GameObject;
                clone.transform.SetParent(contentZhuan.transform);
            }
            
            
            clone.transform.localScale = Vector3.one;
            clone.GetComponent<PaiHangItem>().nickname.text = arr[3 * i + 1];
            clone.GetComponent<PaiHangItem>().SetUI(arr[3 * i + 2], arr[3 * i]);
            clone.GetComponent<PaiHangItem>().time.text = (i + 1).ToString();
        }
    }

    
    public void ClickRankCoin()
    {
        icon_type = 0;
        if (!isRequestReturn)
        {
            return;
        }
        ScrollViewCoin.SetActive(true);
        ButtonCoinSelect.SetActive(true);
        ScrollViewZhuan.SetActive(false);
        ButtonZuanselect.SetActive(false);
        //发请求接数据 实例具体的面板 

        if (sendRequest() == false)
        {
            return;
        }
        isRequestReturn = false;
        
        RankType type = new RankType();
        type.type = 0;
        string sendmsgstr = JsonMapper.ToJson(type);
        CustomSocket.getInstance().sendMsg(new RankRequest(sendmsgstr));
    }
     public void ClickRankZhuan()
    {
        icon_type = 1;
        if (!isRequestReturn)
        {
            return;
        }
        ScrollViewCoin.SetActive(false);
        ButtonCoinSelect.SetActive(false);
        ScrollViewZhuan.SetActive(true);
        ButtonZuanselect.SetActive(true);

        if (sendRequest() == false)
        {
            return;
        }
        isRequestReturn = false;
        
        //发请求接数据 实例具体的面板 
        RankType type = new RankType();
        type.type = 1;
        string sendmsgstr = JsonMapper.ToJson(type);
        CustomSocket.getInstance().sendMsg(new RankRequest(sendmsgstr));

    }

     private bool sendRequest()
     {
         double nowTime = GlobalDataScript.getInstance().getTime();
         if (icon_type == 0)
         {
             if (nowTime - requestGoldTime >= 10000)
             {
                 requestGoldTime = nowTime;
             }
             else
             {
                 return false;
             }
         }
         else if (icon_type == 1)
         {
             if (nowTime - requestDiamondime >= 10000)
             {
                 requestDiamondime = nowTime;
             }
             else
             {
                 return false;
             }
         }
         
         return true;
     }

     public void ExitPanel() 
     {
         SocketEventHandle.getInstance().rank_response -= rank_response;
         Destroy(gameObject);
       
         //记得注销事件
     }
}
