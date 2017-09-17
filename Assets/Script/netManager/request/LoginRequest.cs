using System;
using LitJson;
using System.Collections;

namespace AssemblyCSharp
{
    public class LoginRequest : ClientRequest
    {

        public LoginRequest(string data)
        {
            MyDebug.Log("----------------4------------------");
            headCode = APIS.LOGIN_REQUEST;
            /**
            LoginVo loginvo = new LoginVo ();
            if (data != null) {
                MyDebug.Log (data.toJson());
                try {
					
                    loginvo.openId = (string)data ["openid"];
                    loginvo.nickName = (string)data ["nickname"];
                    loginvo.headIcon = (string)data ["headimgurl"];
                    loginvo.unionid = (string)data ["unionid"];
                    loginvo.province = (string)data ["province"];
                    loginvo.city = (string)data ["city"];
                    string sex = data ["sex"].ToString();
                    loginvo.sex = int.Parse(sex);
                    loginvo.IP = GlobalDataScript.getInstance().getIpAddress();
                } catch (Exception e) {
                    MyDebug.Log ("微信接口有变动！" + e.Message);
                    TipsManagerScript.getInstance ().setTips ("请先打开你的微信客户端");
                    return;
                }
            } else {

            }


            MyDebug.Log ("loginvo.IP" + loginvo.IP);

**/

            if (data == null)
            {
                LoginVo loginvo = new LoginVo();
                Random ran = new Random();
                string str = ran.Next(100, 1000) + "for" + ran.Next(2000, 5000);
                loginvo.openId = "1666333";
                MyDebug.Log("----------------5------------------");

                loginvo.nickName = "111112322444";
                loginvo.headIcon = "imgico221";
                loginvo.unionid = "12732233";
                loginvo.province = "21sfsd";
                loginvo.city = "afafsdf";
                loginvo.sex = 1;
                loginvo.IP = GlobalDataScript.getInstance().getIpAddress();
                data = JsonMapper.ToJson(loginvo);

                GlobalDataScript.loginVo = loginvo;
                GlobalDataScript.loginResponseData = new AvatarVO();
                GlobalDataScript.loginResponseData.account = new Account();
                GlobalDataScript.loginResponseData.account.city = loginvo.city;
                GlobalDataScript.loginResponseData.account.openid = loginvo.openId;
                GlobalDataScript.loginResponseData.account.nickname = loginvo.nickName;
                GlobalDataScript.loginResponseData.account.headicon = loginvo.headIcon;
                GlobalDataScript.loginResponseData.account.unionid = loginvo.city;
                GlobalDataScript.loginResponseData.account.sex = loginvo.sex;
                GlobalDataScript.loginResponseData.IP = loginvo.IP;
            }
            MyDebug.Log("----------------6------------------" + messageContent);
            messageContent = data;

        }


        public LoginRequest(int id, string name = null)
        {
            MyDebug.Log("----------------4------------------");
            headCode = APIS.LOGIN_REQUEST;
            string[] ids = { "128", "121", "122", "123", "124" };
            string[] img = {
							   "http://c.hiphotos.baidu.com/image/pic/item/54fbb2fb43166d222d97f490442309f79152d2d6.jpg",
							   "http://e.hiphotos.baidu.com/image/pic/item/7af40ad162d9f2d303f4c1e5abec8a136227ccd7.jpg",
							   "http://b.hiphotos.baidu.com/image/pic/item/6a600c338744ebf863958135dbf9d72a6159a7d0.jpg",
							   "http://c.hiphotos.baidu.com/image/pic/item/d1160924ab18972bb6bb49c4e4cd7b899e510a3e.jpg",
							   "http://e.hiphotos.baidu.com/image/pic/item/962bd40735fae6cd1fb6e36f0db30f2442a70fb9.jpg"
						   };
            if (name != null)
            {
                name = name.Trim();
                id = 0;
            }
            if (string.IsNullOrEmpty(name))
            {
                if (id >= 0 && id <= 4)
                {
                    name = ids[id];
                }
                else
                {
                    id = 0;
                    name = ids[0];
                }
            }

            LoginVo loginvo = new LoginVo();
            loginvo.openId = name;
            loginvo.nickName = name + "_昵称";
            loginvo.unionid = name + "_uuid";
            loginvo.province = name + "province";
            loginvo.city = name + "city";
            loginvo.headIcon = img[id];
            loginvo.sex = id % 2 + 1;
            loginvo.IP = GlobalDataScript.getInstance().getIpAddress();

            GlobalDataScript.loginVo = loginvo;
            GlobalDataScript.loginResponseData = new AvatarVO();
            GlobalDataScript.loginResponseData.account = new Account();
            GlobalDataScript.loginResponseData.account.city = loginvo.city;
            GlobalDataScript.loginResponseData.account.openid = loginvo.openId;
            GlobalDataScript.loginResponseData.account.nickname = loginvo.nickName;
            GlobalDataScript.loginResponseData.account.headicon = loginvo.headIcon;
            GlobalDataScript.loginResponseData.account.unionid = loginvo.city;
            GlobalDataScript.loginResponseData.account.sex = loginvo.sex;

            MyDebug.Log("----------------6------------------" + messageContent);
            GlobalDataScript.loginResponseData.IP = loginvo.IP;
            messageContent = JsonMapper.ToJson(loginvo);

        }
        /**用于重新登录使用**/


        //退出登录
        public LoginRequest()
        {
            headCode = APIS.QUITE_LOGIN;
            if (GlobalDataScript.loginResponseData != null)
            {
                messageContent = GlobalDataScript.loginResponseData.account.uuid + "";
            }

        }


    }
}

