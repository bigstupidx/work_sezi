using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LitJson;
public class PanelShop : MonoBehaviour {

    public GameObject ScrollViewCoin, ScrollViewZhuan;
    public GameObject ButtonCoinSelect, ButtonZuanselect;
    public GameObject contentCoin, contentZhuan;
    public Sprite[] spritelist;
    public Sprite[] coinspritelist;
    // Use this for initialization
    void Start()
    {
        JsonZhuan();
        JsonCoin();
    }
    private void JsonZhuan() 
    {
        TextAsset textasset = (TextAsset)Resources.Load("Json/Shop_zhuanshi");
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
            GameObject clone = Instantiate(Resources.Load("Prefab/sezi/BuyzuanItem")) as GameObject;
            clone.transform.SetParent(contentZhuan.transform);
            clone.transform.localScale = Vector3.one;
            clone.GetComponent<SeZiBuyZhuan>().SetUI(int.Parse(id), song, num, price, hot);
            clone.GetComponent<SeZiBuyZhuan>().image.sprite = spritelist[i]; //具体的钻石图标赋值
        } 
    }
    private void JsonCoin()
    {
        TextAsset textasset = (TextAsset)Resources.Load("Json/Shop_BuyCoin");
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
            GameObject clone = Instantiate(Resources.Load("Prefab/sezi/BuyCoinItem")) as GameObject;
            clone.transform.SetParent(contentCoin.transform);
            clone.transform.localScale = Vector3.one;
            clone.GetComponent<SeZiBuyZhuan>().SetUICoin(int.Parse(id), song, num, price, hot);
            clone.GetComponent<SeZiBuyZhuan>().image.sprite = coinspritelist[i]; //具体的钻石图标赋值
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ClickRankCoin()
    {
        ScrollViewCoin.SetActive(true);
        ButtonCoinSelect.SetActive(true);
        ScrollViewZhuan.SetActive(false);
        ButtonZuanselect.SetActive(false);
        //发请求接数据 实例具体的面板 

    }
    public void ClickRankZhuan()
    {
        ScrollViewCoin.SetActive(false);
        ButtonCoinSelect.SetActive(false);
        ScrollViewZhuan.SetActive(true);
        ButtonZuanselect.SetActive(true);
        //发请求接数据 实例具体的面板 
    }
    public void ExitPanel()
    {
        Destroy(gameObject);
        //记得注销事件
    }
}
