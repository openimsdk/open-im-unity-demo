using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using UnityGameFramework.Runtime;
using open_im_sdk;
using UnityEditor.UI;

namespace Dawn.Game.UI
{

    public class UIUserInfo : UGuiForm
    {
        Button backBtn;
        Image userIcon;
        TextMeshProUGUI userName;
        TMP_InputField reqMsg;
        Button addBtn;
        FullUserInfo userInfo;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            backBtn = GetButton("Panel/content/top/back");
            userIcon = GetImage("Panel/content/top/icon");
            userName = GetTextPro("Panel/content/top/userid");
            reqMsg = GetInputField("Panel/content/add/reqmsg");
            addBtn = GetButton("Panel/content/add/btn");
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            userInfo = userData as FullUserInfo;
            userName.text = userInfo.PublicInfo.UserID;
            OnClick(backBtn, () =>
            {
                CloseSelf();
            });
            OnClick(addBtn, () =>
            {
                IMSDK.AddFriend((suc, errCode, errMsg) =>
                {
                    if (!suc)
                    {
                        Debug.Log(errCode + ":" + errMsg);
                    }
                    else
                    {
                        CloseSelf();
                    }
                }, new ApplyToAddFriendReq()
                {
                    FromUserID = Player.Instance.UserId,
                    ToUserID = userInfo.PublicInfo.UserID,
                    ReqMsg = reqMsg.text,
                    Ex = "",
                });
            });
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}

