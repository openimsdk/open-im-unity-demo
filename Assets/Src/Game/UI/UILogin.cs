using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using UnityGameFramework.Runtime;
using Dawn.Game.Event;
using System;
using open_im_sdk;
using System.Text;

namespace Dawn.Game.UI
{
    public class UILogin : UGuiForm
    {
        TMP_InputField userId;
        TMP_InputField token;
        Button loginBtn;
        Button registerBtn;

        Button requestTokenBtn;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            userId = GetInputField("Panel/userId/input");
            token = GetInputField("Panel/token/input");
            loginBtn = GetButton("Panel/login");
            registerBtn = GetButton("Panel/register");

            requestTokenBtn = GetButton("Panel/token/requesttoken");
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            userId.text = "";
            token.text = "";
#if UNITY_EDITOR_WIN
            userId.text = "yejian001";
            token.text = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySUQiOiJ5ZWppYW4wMDEiLCJQbGF0Zm9ybUlEIjozLCJleHAiOjE3MjA1MzA2NzEsIm5iZiI6MTcxMjc1NDM3MSwiaWF0IjoxNzEyNzU0NjcxfQ.eU_wa1lzQdhju313liT0wbw9dQ-9nWJmSoP2bGGnaeM";
#endif
            OnClick(loginBtn, () =>
            {
                login();
            });
            OnClick(registerBtn, () =>
            {
                GameEntry.UI.OpenUI("Register");
            });

            OnClick(requestTokenBtn, () =>
            {
                var url = string.Format("{0}/user/user_register?operationID={1}", Setting.Instance.GetTokenURL, "register");
                var userTokenReq = new UserTokenReq()
                {

                };
                var json = JsonUtility.ToJson(userTokenReq);
                GameEntry.WebRequest.AddWebRequest(url, Encoding.UTF8.GetBytes(json));
            });

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        void login()
        {
            if (userId.text == "")
            {
                GameEntry.UI.Tip("UserId is Empty");
                return;
            }
            if (token.text == "")
            {
                GameEntry.UI.Tip("Token is Empty");
                return;
            }
            Player.Instance.Login(userId.text, token.text);
        }
    }
}

