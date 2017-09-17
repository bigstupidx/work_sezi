using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using LitJson;
using System.IO;
[SerializeField]
public class Shop_Zhuan 
{
    public Shop_Zhuan() { }

    public Shop_Zhuan_Array []array;

}
[SerializeField]
public class Shop_Zhuan_Array
{
    public Shop_Zhuan_Array() { }
    public string id;
    public string image;
    public int song;
    public int num;
    public int price;
    public int hot;
}
public class PanelJinbi : MonoBehaviour {
    public Transform duihuanmangger;
    public Sprite[] spritelist;
	// Use this for initialization
    void Start()
    {

        TextAsset textasset = (TextAsset)Resources.Load("Json/Shop_zhuanshi");
        string jsonTest = textasset.text;
        print(jsonTest);
        Shop_Zhuan json = JsonMapper.ToObject<Shop_Zhuan>(jsonTest);

        for (int i = 0; i < json.array.Length; i++)
        {
           string id=  json.array[i].id;
           int song=json.array[i].song;
           int num=json.array[i].num;
           int price=json.array[i].price;
           int hot = json.array[i].hot;
           GameObject clone = Instantiate(Resources.Load("Prefab/sezi/BuyzuanItem")) as GameObject;
           clone.transform.SetParent(duihuanmangger);
           clone.transform.localScale = Vector3.one;
           clone.GetComponent<SeZiBuyZhuan>().SetUI(int.Parse(id),song, num, price, hot);
           clone.GetComponent<SeZiBuyZhuan>().image.sprite = spritelist[i]; //具体的钻石图标赋值
        } 
    
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void ExitPanel() 
    {
        Destroy(gameObject);
    }

    public void ChangeOneCoin()
    {
        CustomSocket.getInstance().sendMsg(new GoldChange());
    }
}
