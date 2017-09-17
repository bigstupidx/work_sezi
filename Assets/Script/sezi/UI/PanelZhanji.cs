using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using LitJson;
public class PanelZhanji : MonoBehaviour {
    public Transform contont;
	// Use this for initialization
	void Start () 
    {
        SocketEventHandle.getInstance().zhanjiResponse += zhanjiResponse;
        //Init();
	}

    private void zhanjiResponse(ClientResponse response)
    {
      
        //string message = response.message;

        JsonData json = JsonMapper.ToObject(response.message);
        print("zhanjiDetailResponse" + response.message);      
        string message = json["zhanji"].ToString();
        string[] arr = message.Split(',');


        for (int i = 0; i < arr.Length/2; i++)
        {
            GameObject clone = Instantiate(Resources.Load("Prefab/sezi/paihangItem")) as GameObject;
            clone.transform.SetParent(contont);
            clone.transform.localScale = Vector3.one;
            clone.GetComponent<PaiHangItem>().SetUI(arr[2 * i+1],GlobalDataScript.loginResponseData.account.headicon);
            clone.GetComponent<PaiHangItem>().time.text = arr[2 * i];
            clone.GetComponent<PaiHangItem>().nickname.text = GlobalDataScript.loginResponseData.account.nickname;
            clone.GetComponent<PaiHangItem>().id = i + 1;
        }
    }

	
    public void ExitPanel() 
    {
        SocketEventHandle.getInstance().zhanjiResponse -= zhanjiResponse;
        Destroy(gameObject);
    }
   


   
}
