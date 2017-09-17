using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace global
{
    public class SeZiItemPointScript : MonoBehaviour
    {

        public Image poing_img;
        public Image other_img;
        private int _id;
        private int _value;
        private bool _show;

        // Use this for initialization
        void Start()
        {

            other_img.enabled = _show;
            other_img.gameObject.SetActive(_show);
            if (_value == (int)IMG_ENUM.ZHAI)
            {
                other_img.sprite = Resources.Load("sizi/size_scene/point/zai", typeof(Sprite)) as Sprite;
            }
            else if (_value == (int)IMG_ENUM.TUI)
            {
                other_img.sprite = Resources.Load("sizi/size_scene/point/tui", typeof(Sprite)) as Sprite;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void setId(int value)
        {
            _id = value;
            poing_img.sprite = Resources.Load("sizi/size_scene/point/point_" + value, typeof(Sprite)) as Sprite;
            //poing_img.GetComponent<Image>().sprite = Resources.Load ("sizi/size_scene/point/point_" + value, typeof(Sprite)) as Sprite;
            //print (("sizi/size_scene/point/point_" + value));
        }
        /**
         * value==2 推荐按钮
         * value==1 宅按钮
         * */
        public void showDDImg(int value, bool show)
        {
            _value = value;
            _show = show;
            other_img.enabled = show;
            other_img.gameObject.SetActive(show);
            if (value == (int)IMG_ENUM.ZHAI)
            {
				other_img.sprite = Resources.Load("sizi/size_scene/point/zai", typeof(Sprite)) as Sprite;
            }
            else if (value == (int)IMG_ENUM.TUI)
            {
				other_img.sprite = Resources.Load("sizi/size_scene/point/tui", typeof(Sprite)) as Sprite;
            }
        }

        public int getId()
        {
            return _id;
        }
    }

    enum IMG_ENUM
    {
        ZHAI = 1,
        TUI
    }
}