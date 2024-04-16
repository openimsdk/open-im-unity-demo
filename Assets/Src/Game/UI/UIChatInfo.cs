using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using open_im_sdk;

namespace Dawn.Game.UI
{
    public class UIChatInfo : UGuiForm
    {
        class MemeberItem
        {
            public Image Icon;
            public TextMeshProUGUI Name;
            public Button Btn;
        }

        Button backBtn;
        LoopGridView memberList;
        RectTransform groupNameRect;
        Button groupNameBtn;
        TextMeshProUGUI groutName;
        Button searchHistory;
        Button clearChat;
        RectTransform groupExitRect;
        Button groupExitBtn;

        LocalConversation conversation;
        List<LocalGroupMember> membersInfo;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            backBtn = GetButton("Panel/content/top/back");
            memberList = GetGridView("Panel/content/center/members/list");
            groupNameRect = GetRectTransform("Panel/content/center/groupname");
            groupNameBtn = GetButton("Panel/content/center/groupname/btn");
            groutName = GetTextPro("Panel/content/center/groupname/name");
            searchHistory = GetButton("Panel/content/center/searchhistory/btn");
            clearChat = GetButton("Panel/content/center/clearchat/btn");
            groupExitRect = GetRectTransform("Panel/content/center/groupexit");
            groupExitBtn = GetButton("Panel/content/center/groupexit/btn");

            memberList.InitGridView(0, (list, index, row, rol) =>
            {
                if (index < 0) return null;
                bool isAdd = index == membersInfo.Count;
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
                if (isAdd)
                {
                    item.Name.text = "添加";
                    OnClick(item.Btn, () =>
                    {
                        // TODO
                    });
                }
                else
                {
                    var info = membersInfo[index];
                    if (conversation.ConversationType == (int)ConversationType.Single)
                    {

                    }
                    else
                    {

                    }
                    OnClick(item.Btn, () =>
                    {
                        // GameEntry.UI.OpenUI("UserInfo",) // userid
                    });
                }
                return itemNode;
            });
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            conversation = userData as LocalConversation;
            OnClick(backBtn, () =>
            {
                CloseSelf();
            });
            OnClick(searchHistory, () => { });
            OnClick(clearChat, () => { });
            if (conversation.ConversationType == (int)ConversationType.Single)
            {
                groupNameRect.gameObject.SetActive(false);
                groupExitRect.gameObject.SetActive(false);

                membersInfo = new List<LocalGroupMember>();
                membersInfo.Add(new LocalGroupMember()
                {

                });
                RefreshGrid(memberList, membersInfo.Count + 1);
            }
            else if (conversation.ConversationType == (int)ConversationType.Group)
            {
                groupNameRect.gameObject.SetActive(true);
                groupExitRect.gameObject.SetActive(true);

                groutName.text = conversation.GroupID;
                OnClick(groupNameBtn, () =>
                {
                    // TODO
                });
                OnClick(groupExitBtn, () =>
                {
                    IMSDK.QuitGroup((suc, err, errMsg) =>
                    {
                        if (suc)
                        {

                        }
                        else
                        {
                            Debug.LogError(errMsg);
                        }
                    }, conversation.GroupID);
                });
                IMSDK.GetGroupMemberList((list, err, errMsg) =>
                {
                    if (list != null)
                    {
                        membersInfo = list;
                        RefreshGrid(memberList, membersInfo.Count + 1);
                    }
                    else
                    {
                        Debug.LogError(errMsg);
                    }
                }, conversation.GroupID, 0, 0, 0);
            }
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}

