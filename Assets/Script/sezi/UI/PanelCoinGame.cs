using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;
using UnityEngine.UI;

public class PanelCoinGame : MonoBehaviour {
    public Transform content;
    public Sprite[] spritelist;

    public Text diamondText;

    // Use this for initialization
    void Start()
    {

        diamondText.text = GlobalDataScript.loginResponseData.account.roomcard + "";

        TextAsset textasset = (TextAsset)Resources.Load("Json/coin");
        string jsonTest = textasset.text;
        print(jsonTest);
        Shop_Zhuan json = JsonMapper.ToObject<Shop_Zhuan>(jsonTest);

        for (int i = 0; i < json.array.Length; i++)
        {
            string id = json.array[i].id;
            int song = json.array[i].song;
            int num = json.array[i].num;
            int price = json.array[i].price;
            int hot = json.array[i].hot;
            GameObject clone = Instantiate(Resources.Load("Prefab/sezi/coinItem")) as GameObject;
            clone.transform.SetParent(content);
            clone.transform.localScale = Vector3.one;
            clone.GetComponent<SeZiBuyZhuan>().SetUICoin(int.Parse(id), song, num, price, hot);
            clone.GetComponent<SeZiBuyZhuan>().image.sprite = spritelist[i]; //具体的钻石图标赋值
        }

    }

    public void onChargeClick()
    {
        PrefabManage.loadPerfab("Prefab/YueqinPanel/Panel_Shouchong");
    }

    public void ExitPanel()
    {
        Destroy(gameObject);
    }
}
