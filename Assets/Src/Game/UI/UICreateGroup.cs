using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using OpenIM.IMSDK.Unity;
using Dawn.Game.Event;
using GameFramework.Event;
using System;

namespace Dawn.Game.UI
{
    public class UICreateGroup : UGuiForm
    {
        class MemeberItem
        {
            public Image Icon;
            public TextMeshProUGUI Name;
            public Button Btn;
        }
        Button backBtn;
        Button inviteBtn;
        Button createBtn;
        Button faceBtn;
        Image faceIcon;
        TMP_InputField groupNameInput;
        LoopListView2 memberList;
        List<FriendInfo> selectFriends;
        string selectIcon = "";
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            backBtn = GetButton("Panel/content/top/back");
            groupNameInput = GetInputField("Panel/content/center/groupname/input");
            inviteBtn = GetButton("Panel/content/top/invite");
            faceBtn = GetButton("Panel/content/center/face/icon");
            faceIcon = GetImage("Panel/content/center/face/icon");
            createBtn = GetButton("Panel/content/center/createbtn/btn");
            memberList = GetListView("Panel/content/center/members/list");

            selectFriends = new List<FriendInfo>();
            memberList.InitListView(0, (list, index) =>
            {
                if (index < 0) return null;
                var itemNode = list.NewListViewItem("item");
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
                var info = selectFriends[index];
                SetImage(item.Icon, info.FaceURL);
                item.Name.text = info.Nickname;
                return itemNode;
            });
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            OnClick(backBtn, () =>
            {
                CloseSelf();
            });
            OnClick(inviteBtn, () =>
            {
                GameEntry.UI.OpenUI("SelectMember", (OnSelectFriends)OnSelectFriends);
            });
            OnClick(faceBtn, () =>
            {
                GameEntry.UI.OpenUI("SelectIcon", (OnSelectHeadIcon)OnSelectHeadIcon);
            });
            OnClick(createBtn, () =>
            {
                if (groupNameInput.text == "")
                {
                    GameEntry.UI.Tip("groupName is empty");
                    return;
                }
                if (selectFriends.Count <= 0)
                {
                    GameEntry.UI.Tip("not select members");
                    return;
                }
                List<string> membersId = new List<string>();
                foreach (var userInfo in selectFriends)
                {
                    membersId.Add(userInfo.FriendUserID);
                }
                IMSDK.CreateGroup((groupInfo, err, errMsg) =>
                {
                    if (groupInfo != null)
                    {
                        CloseSelf();
                        GameEntry.UI.Tip("Create Group Success");
                    }
                    else
                    {
                        GameEntry.UI.Tip(errMsg);
                    }
                }, new CreateGroupReq()
                {
                    MemberUserIDs = membersId.ToArray(),
                    AdminUserIDs = null,
                    OwnerUserID = IMSDK.GetLoginUserId(),
                    GroupInfo = new GroupInfo()
                    {
                        GroupType = (int)GroupType.Group,
                        GroupName = groupNameInput.text,
                        FaceURL = selectIcon,
                    }
                });
            });
            selectIcon = "headicon/不知火舞";
            groupNameInput.text = "";
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
        public void OnSelectHeadIcon(string url)
        {
            SetImage(faceIcon, url);
            selectIcon = url;
        }
        void OnSelectFriends(FriendInfo[] list)
        {
            if (list.Length <= 0) return;
            selectFriends.Clear();
            selectFriends.AddRange(list);
            RefreshList(memberList, selectFriends.Count);
        }
    }
}

