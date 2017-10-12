using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using LitJson;

public class SeZiBuyZhuan : MonoBehaviour {

    //public string id;
    //public string image;
    //public int song;
    //public int num;
    //public int price;
    //public int hot;

    public int id;
    public Image image;
    public Image hot;
    public Text song;
    public Text num;
    public Text price;

    private bool isCoin = false;
    private int needMoneyNum = 0;


    public void SetUI(int _id ,int _song,int _num,int _price,int _hot) 
    {
        needMoneyNum = _num;
        id = _id;
        song.text = _song.ToString();
        num.text = _num+"万";
        price.text = _price.ToString();
        hot.gameObject.SetActive(false);
        if (_hot==0)
        {
            hot.gameObject.SetActive(false);
        }
        else
        {
            //hot.gameObject.SetActive(true);
        }
        isCoin = false;
    }
    public void SetUICoin(int _id, int _song, int _num, int _price, int _hot)
    {
        needMoneyNum = _num;
        id = _id;
        song.text = _song+"万金豆";
        num.text = _num+"" ;
        price.text = _price + "万";
        hot.gameObject.SetActive(false);
        if (_hot == 0)
        {
            hot.gameObject.SetActive(false);
        }
        else
        {
           // hot.gameObject.SetActive(true);
        }
        isCoin = true;
    }

    public void onBuyClick()
    {

        SoundCtrl.getInstance().playSoundByActionButton(1);

        if (isCoin)
        {
            if (GlobalDataScript.loginResponseData.account.roomcard < needMoneyNum)
            {
                //PrefabManage.loadPerfab("Prefab/sezi/Panel_ContactService");
                //PrefabManage.loadPerfab("Prefab/YueqinPanel/Panel_Shouchong");

                SZChargeRequestVO vo = new SZChargeRequestVO();
                vo.money = price.text;
                string msg = JsonMapper.ToJson(vo);
                CustomSocket.getInstance().sendMsg(new SZChargeRequest(msg));

                return;
            }
            GameObject obj = PrefabManage.loadPerfab("Prefab/sezi/PanelTipBuy");
            obj.GetComponent<PanelTipBuy>().setOnBuy(needMoneyNum);
        }
        else
        {
            SZChargeRequestVO vo = new SZChargeRequestVO();
            vo.money = price.text;
            string msg = JsonMapper.ToJson(vo);
            CustomSocket.getInstance().sendMsg(new SZChargeRequest(msg));
            //充值
            //PrefabManage.loadPerfab("Prefab/sezi/Panel_ContactService");
        }
    }

    // Use this for initialization

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
