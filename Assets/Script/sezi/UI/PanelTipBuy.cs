using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using LitJson;
using UnityEngine.UI;

public class PanelTipBuy : MonoBehaviour {

    public Text needZuanText;
    private int buyNum;

	// Use this for initialization
	void Start () {
        needZuanText.text = buyNum.ToString();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void onBuyClick()
    {

        SoundCtrl.getInstance().playSoundByActionButton(1);

        if (GlobalDataScript.loginResponseData.account.roomcard < buyNum)
        {
            PrefabManage.loadPerfab("Prefab/YueqinPanel/Panel_Shouchong");
            return;
        }
        SZExchangeRequestVO vo = new SZExchangeRequestVO();
        vo.value = buyNum;
        string sendmsgstr = JsonMapper.ToJson(vo);
        CustomSocket.getInstance().sendMsg(new SZExchangeRequest(sendmsgstr));

        onCacelClick();
    }

    public void onCacelClick()
    {
        Destroy(this);
        Destroy(gameObject);
    }

    public void setOnBuy(int num)
    {
        buyNum = num;
    }

}
