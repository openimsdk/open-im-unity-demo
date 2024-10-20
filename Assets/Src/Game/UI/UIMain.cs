using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using OpenIM.IMSDK.Unity;
using Dawn.Game.Event;
using GameFramework;
using GameFramework.Event;

namespace Dawn.Game.UI
{
    public enum NavMenu
    {
        Conversation, Friend, Owner
    }

    public partial class UIMain : UGuiForm
    {
        TextMeshProUGUI title;
        Button topSearchBtn;
        Toggle[] toggles;
        RectTransform unRead;
        TextMeshProUGUI unReadVal;
        NavMenu selectNavMenu = NavMenu.Conversation;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            title = GetTextPro("Panel/content/top/title");
            topSearchBtn = GetButton("Panel/content/top/search");
            toggles = new Toggle[3];
            toggles[(int)NavMenu.Conversation] = GetToggle("Panel/content/bottom/menu/conversaton");
            unRead = GetRectTransform("Panel/content/bottom/menu/conversaton/unread");
            unReadVal = GetTextPro("Panel/content/bottom/menu/conversaton/unread/val");
            toggles[(int)NavMenu.Friend] = GetToggle("Panel/content/bottom/menu/friend");
            toggles[(int)NavMenu.Owner] = GetToggle("Panel/content/bottom/menu/owner");
            InitConversation();
            InitFriend();
            InitOwner();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            toggles[(int)NavMenu.Conversation].onValueChanged.AddListener((ison) =>
            {
                conversationRoot.gameObject.SetActive(ison);
                if (ison)
                {
                    title.text = "会话";
                    selectNavMenu = NavMenu.Conversation;
                }
            });
            toggles[(int)NavMenu.Friend].onValueChanged.AddListener((ison) =>
            {
                friendRoot.gameObject.SetActive(ison);
                if (ison)
                {
                    title.text = "好友";
                    selectNavMenu = NavMenu.Friend;
                }
            });
            toggles[(int)NavMenu.Owner].onValueChanged.AddListener((ison) =>
            {
                ownerRoot.gameObject.SetActive(ison);
                if (ison)
                {
                    selectNavMenu = NavMenu.Owner;
                }
            });
            OnClick(topSearchBtn, () =>
            {
                GameEntry.UI.OpenUI("Search", this);
            });

            selectNavMenu = NavMenu.Conversation;
            toggles[(int)selectNavMenu].isOn = true;

            conversationRoot.gameObject.SetActive(selectNavMenu == NavMenu.Conversation);
            friendRoot.gameObject.SetActive(selectNavMenu == NavMenu.Friend);
            ownerRoot.gameObject.SetActive(selectNavMenu == NavMenu.Owner);
            unRead.gameObject.SetActive(false);
            OpenConversation();
            OpenFriend();
            OpenOwner();

            RefreshUnRead();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            CloseConversation();
            CloseFriend();
            CloseOwner();
        }

        void RefreshUnRead()
        {
            IMSDK.GetTotalUnreadMsgCount((unreadCount, errCode, errMsg) =>
            {
                unRead.gameObject.SetActive(unreadCount > 0);
                if (unreadCount > 0)
                {
                    unReadVal.text = unreadCount > 99 ? "99" : unreadCount.ToString();
                }
            });
        }
    }
}

