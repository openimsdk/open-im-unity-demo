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

namespace Dawn.Game.UI
{
    public class UserHistoryItem
    {
        public Button btn;
        public TextMeshProUGUI text;
    }

    public class UILogin : UGuiForm
    {
        TMP_InputField userId;
        TMP_InputField token;
        Button loginBtn;

        Button switchBtn;
        RectTransform switchPanel;
        LoopListView2 historyList;
        Button closeSwitchBtn;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            userId = GetInputField("Panel/userId/input");
            token = GetInputField("Panel/token/input");
            loginBtn = GetButton("Panel/login");
            switchBtn = GetButton("Panel/switch");
            switchPanel = GetRectTransform("Panel/switchPanel");
            historyList = GetListView("Panel/switchPanel/content/list");
            closeSwitchBtn = GetButton("Panel/switchPanel/content/back");
            switchPanel.gameObject.SetActive(false);
            historyList.InitListView(0, (list, index) =>
            {
                if (index < 0)
                {
                    return null;
                }
                LoopListViewItem2 itemNode = null;
                itemNode = list.NewListViewItem("item");
                if (!itemNode.IsInitHandlerCalled)
                {
                    itemNode.UserObjectData = new UserHistoryItem()
                    {
                        btn = itemNode.transform.GetComponent<Button>(),
                        text = itemNode.transform.Find("userid").GetComponent<TextMeshProUGUI>(),
                    };
                    itemNode.IsInitHandlerCalled = true;
                }
                var item = itemNode.UserObjectData as UserHistoryItem;
                return itemNode;
            });
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
            OnClick(switchBtn, () =>
            {
                switchPanel.gameObject.SetActive(true);
            });

            OnClick(closeSwitchBtn, () =>
            {
                switchPanel.gameObject.SetActive(false);
            });
            historyList.SetListItemCount(0);

            // 自动登录
            // login();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        void login()
        {
            if (userId.text == "")
            {
                UIExtension.ShowTip("UserId is Empty");
                return;
            }
            if (token.text == "")
            {
                UIExtension.ShowTip("Token is Empty");
                return;
            }
            Player.Instance.Login(userId.text, token.text);
        }
    }
}

