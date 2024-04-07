using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using UnityGameFramework.Runtime;
using open_im_sdk;

namespace Dawn.Game.UI
{

    public class UIUserInfo : UGuiForm
    {
        Button backBtn;
        Image userIcon;
        TextMeshProUGUI userName;
        TMP_InputField reqMsg;

        RectTransform friendTrans;
        Button remarkBtn;
        Button sendMsgBtn;
        Button audioChatBtn;
        RectTransform addTrans;
        Button addBtn;
        LocalFriend localFriend;
        PublicUser publicUser;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            backBtn = GetButton("Panel/content/top/back");
            userIcon = GetImage("Panel/content/center/icon");
            userName = GetTextPro("Panel/content/center/userid");
            friendTrans = GetRectTransform("Panel/content/bottom/friend");
            remarkBtn = GetButton("Panel/content/bottom/friend/remark");
            sendMsgBtn = GetButton("Panel/content/bottom/friend/msg");
            audioChatBtn = GetButton("Panel/content/bottom/friend/audiochat");
            addTrans = GetRectTransform("Panel/content/bottom/add");
            reqMsg = GetInputField("Panel/content/bottom/add/reqmsg");
            addBtn = GetButton("Panel/content/bottom/add/btn");
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            string userId = "";
            if (userData is LocalFriend)
            {
                localFriend = userData as LocalFriend;
                userId = localFriend.FriendUserID;
            }
            else if (userData is PublicUser)
            {
                publicUser = userData as PublicUser;
                userId = publicUser.UserID;
            }
            userName.text = userId;
            OnClick(backBtn, () =>
            {
                CloseSelf();
            });
            if (Player.Instance.FriendShip.IsFriend(userId))
            {
                friendTrans.gameObject.SetActive(true);
                addTrans.gameObject.SetActive(false);

                OnClick(sendMsgBtn, () =>
                {
                    GameEntry.UI.OpenUI("Chat", localFriend);
                });
            }
            else
            {
                friendTrans.gameObject.SetActive(false);
                addTrans.gameObject.SetActive(true);
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
                        ToUserID = publicUser.UserID,
                        ReqMsg = reqMsg.text,
                        Ex = "",
                    });
                });
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}

