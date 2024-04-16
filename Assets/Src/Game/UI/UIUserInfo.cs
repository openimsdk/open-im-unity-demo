using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using UnityGameFramework.Runtime;
using open_im_sdk;
using open_im_sdk.util;

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
            string userId = userData as string;
            userName.text = userId;
            OnClick(backBtn, () =>
            {
                CloseSelf();
            });
            IMSDK.CheckFriend((list, err, errMsg) =>
            {
                if (list != null && list.Count == 1)
                {
                    if (list[0].Result == 1)
                    {
                        friendTrans.gameObject.SetActive(true);
                        addTrans.gameObject.SetActive(false);

                        OnClick(sendMsgBtn, () =>
                        {
                            IMSDK.GetOneConversation((conversation, err, errMsg) =>
                            {
                                if (conversation != null)
                                {
                                    GameEntry.UI.OpenUI("Chat", conversation);
                                }
                                else
                                {
                                    Debug.LogError(err + ":" + errMsg);
                                }
                            }, (int)ConversationType.Single, userId);
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
                                FromUserID = IMSDK.GetLoginUser(),
                                ToUserID = userId,
                                ReqMsg = reqMsg.text,
                                Ex = "",
                            });
                        });
                    }
                }
            }, new string[] { userId });
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}

