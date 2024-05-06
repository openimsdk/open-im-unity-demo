using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using open_im_sdk;
using Dawn.Game.Event;
using GameFramework.Event;
using System;

namespace Dawn.Game.UI
{
    public class UIGroupInfo : UGuiForm
    {
        class MemeberItem
        {
            public Image Icon;
            public TextMeshProUGUI Name;
            public Button Btn;
        }
        TextMeshProUGUI title;
        Button backBtn;
        LoopGridView memberList;
        Button groupChatBtn;
        Button searchHistory;
        Button clearChat;
        Button groupExitBtn;
        LocalGroup localGroup;
        List<LocalGroupMember> membersInfo;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            title = GetTextPro("Panel/content/top/title");
            backBtn = GetButton("Panel/content/top/back");
            memberList = GetGridView("Panel/content/center/members/list");
            groupChatBtn = GetButton("Panel/content/center/chat/btn");
            searchHistory = GetButton("Panel/content/center/searchhistory/btn");
            clearChat = GetButton("Panel/content/center/clearchat/btn");
            groupExitBtn = GetButton("Panel/content/center/groupexit/btn");

            memberList.InitGridView(0, (list, index, row, rol) =>
            {
                if (index < 0) return null;
                bool isAdd = index == membersInfo.Count;
                var itemNode = isAdd ? list.NewListViewItem("add") : list.NewListViewItem("item");
                if (!itemNode.IsInitHandlerCalled)
                {
                    var parent = itemNode.transform as RectTransform;
                    itemNode.UserObjectData = new MemeberItem()
                    {
                        Icon = GetImage("icon", parent),
                        Btn = GetButton("", parent),
                        Name = GetTextPro("name", parent)
                    };
                    itemNode.IsInitHandlerCalled = true;
                }
                MemeberItem item = itemNode.UserObjectData as MemeberItem;
                if (isAdd)
                {
                    item.Name.text = "";
                    OnClick(item.Btn, () =>
                    {
                        GameEntry.UI.OpenUI("SelectMember", (OnSelectMember)OnSelectMember);
                    });
                }
                else
                {
                    var info = membersInfo[index];
                    SetImage(item.Icon, info.FaceURL);
                    item.Name.text = info.Nickname;
                    OnClick(item.Btn, () =>
                    {
                        GameEntry.UI.OpenUI("UserInfo", info.UserID);
                    });
                }
                return itemNode;
            });
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            localGroup = userData as LocalGroup;

            title.text = localGroup.GroupName;

            OnClick(backBtn, () =>
            {
                CloseSelf();
            });
            OnClick(groupExitBtn, () =>
            {

            });
            OnClick(groupChatBtn, () =>
            {
                IMSDK.QuitGroup((suc, err, errMsg) =>
                {
                    if (suc)
                    {
                        CloseSelf();
                    }
                    else
                    {
                        GameEntry.UI.Tip(errMsg);
                    }
                }, localGroup.GroupID);
            });

            OnClick(searchHistory, () =>
            {
                GameEntry.UI.Tip("TODO");
            });
            OnClick(clearChat, () =>
            {
                GameEntry.UI.Tip("TODO");
            });

            RefreshUI();
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
        void RefreshUI()
        {
            IMSDK.GetGroupMemberList((list, err, errMsg) =>
            {
                if (list != null)
                {
                    Debug.Log(list.Count);
                    membersInfo = list;
                    RefreshGrid(memberList, membersInfo.Count + 1);
                }
                else
                {
                    GameEntry.UI.Tip(errMsg);
                }
            }, localGroup.GroupID, 0, 0, 0);
        }

        void OnSelectMember(FullUserInfo[] selectUsers)
        {
            if (selectUsers.Length <= 0) return;
            var name = "";
            var members = new string[selectUsers.Length];
            for (int i = 0; i < selectUsers.Length; i++)
            {
                name = name + selectUsers[i].FriendInfo.Nickname;
                members[i] = selectUsers[i].FriendInfo.FriendUserID;
            }
            IMSDK.InviteUserToGroup((suc, err, errMsg) =>
            {
                if (suc)
                {
                    GameEntry.UI.Tip("Invite User Suc");
                }
                else
                {
                    GameEntry.UI.Tip(errMsg);
                }
            }, localGroup.GroupID, "", members);
        }
    }
}

