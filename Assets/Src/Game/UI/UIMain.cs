using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using open_im_sdk;

namespace Dawn.Game.UI
{
    public enum NavMenu
    {
        World, Channel, Group, Friend, Owner
    }

    public partial class UIMain : UGuiForm
    {
        Toggle[] toggles;

        NavMenu selectNavMenu = NavMenu.World;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            toggles = new Toggle[5];
            toggles[(int)NavMenu.World] = GetToggle("Panel/content/bottom/menu/world");
            toggles[(int)NavMenu.Channel] = GetToggle("Panel/content/bottom/menu/channel");
            toggles[(int)NavMenu.Group] = GetToggle("Panel/content/bottom/menu/group");
            toggles[(int)NavMenu.Friend] = GetToggle("Panel/content/bottom/menu/friend");
            toggles[(int)NavMenu.Owner] = GetToggle("Panel/content/bottom/menu/owner");
            InitWorld();
            InitChannel();
            InitGroup();
            InitFriend();
            InitOwner();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            toggles[(int)NavMenu.World].onValueChanged.AddListener((ison) =>
            {
                worldRoot.gameObject.SetActive(ison);
                if (ison)
                {
                    selectNavMenu = NavMenu.World;
                }
            });
            toggles[(int)NavMenu.Channel].onValueChanged.AddListener((ison) =>
            {
                channelRoot.gameObject.SetActive(ison);
                if (ison)
                {
                    selectNavMenu = NavMenu.Channel;
                }
            });
            toggles[(int)NavMenu.Group].onValueChanged.AddListener((ison) =>
            {
                groupRoot.gameObject.SetActive(ison);
                if (ison)
                {
                    selectNavMenu = NavMenu.Group;
                }
            });
            toggles[(int)NavMenu.Friend].onValueChanged.AddListener((ison) =>
            {
                friendRoot.gameObject.SetActive(ison);
                if (ison)
                {
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

            selectNavMenu = NavMenu.Friend;
            toggles[(int)selectNavMenu].isOn = true;

            worldRoot.gameObject.SetActive(selectNavMenu == NavMenu.World);
            channelRoot.gameObject.SetActive(selectNavMenu == NavMenu.Channel);
            groupRoot.gameObject.SetActive(selectNavMenu == NavMenu.Group);
            friendRoot.gameObject.SetActive(selectNavMenu == NavMenu.Friend);
            ownerRoot.gameObject.SetActive(selectNavMenu == NavMenu.Owner);

            OpenWorld();
            OpenChannel();
            OpenGroup();
            OpenFriend();
            OpenOwner();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            CloseWorld();
            CloseChannel();
            CloseGroup();
            CloseFriend();
            CloseOwner();
        }
    }
}

