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
        TextMeshProUGUI groupName;
        Button searchHistory;
        Button clearChat;
        RectTransform groupExitRect;
        Button groupExitBtn;
        // 在创建群组时会用到
        LocalConversation conversation;
        LocalConversation createdConversation;
        List<LocalGroupMember> membersInfo;

        LocalGroup localGroup;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            backBtn = GetButton("Panel/content/top/back");
            memberList = GetGridView("Panel/content/center/members/list");
            groupNameRect = GetRectTransform("Panel/content/center/groupname");
            groupNameBtn = GetButton("Panel/content/center/groupname/btn");
            groupName = GetTextPro("Panel/content/center/groupname/name");
            searchHistory = GetButton("Panel/content/center/searchhistory/btn");
            clearChat = GetButton("Panel/content/center/clearchat/btn");
            groupExitRect = GetRectTransform("Panel/content/center/groupexit");
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
                    if (conversation.ConversationType == (int)ConversationType.Single)
                    {

                    }
                    else
                    {

                    }
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
            conversation = userData as LocalConversation;
            OnClick(backBtn, () =>
            {
                CloseSelf();
            });
            OnClick(searchHistory, () => { });
            OnClick(clearChat, () => { });

            RefreshUI();

            GameEntry.Event.Subscribe(OnConversationChange.EventId, HandleConversationChange);
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(OnConversationChange.EventId, HandleConversationChange);
        }
        void RefreshUI()
        {
            if (conversation.ConversationType == (int)ConversationType.Single)
            {
                groupNameRect.gameObject.SetActive(false);
                groupExitRect.gameObject.SetActive(false);

                membersInfo = new List<LocalGroupMember>();
                membersInfo.Add(new LocalGroupMember()
                {
                    UserID = conversation.UserID,
                    Nickname = conversation.ShowName,
                    FaceURL = conversation.FaceURL,
                });
                RefreshGrid(memberList, membersInfo.Count + 1);
            }
            else if (conversation.ConversationType == (int)ConversationType.Group)
            {
                groupNameRect.gameObject.SetActive(true);
                groupExitRect.gameObject.SetActive(true);

                IMSDK.GetSpecifiedGroupsInfo((list, err, errMsg) =>
                {
                    if (list != null)
                    {
                        if (list.Count >= 1)
                        {
                            localGroup = list[0];
                            groupName.text = localGroup.GroupName;

                        }
                    }
                    else
                    {
                        GameEntry.UI.Tip(errMsg);
                    }
                }, new string[] { conversation.GroupID });
                IMSDK.GetGroupMemberList((list, err, errMsg) =>
                {
                    if (list != null)
                    {
                        membersInfo = list;
                        RefreshGrid(memberList, membersInfo.Count + 1);
                    }
                    else
                    {
                        GameEntry.UI.Tip(errMsg);
                    }
                }, conversation.GroupID, 0, 0, 0);
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
                            GameEntry.UI.Tip("Quit Group Success");
                        }
                        else
                        {
                            GameEntry.UI.Tip(errMsg);
                        }
                    }, conversation.GroupID);
                });
            }
        }

        void OnSelectMember(string[] memebers)
        {
            if (memebers.Length <= 0) return;
            if (conversation.ConversationType == (int)ConversationType.Single)
            {
                IMSDK.CreateGroup((groupInfo, err, errMsg) =>
                {
                    if (groupInfo != null)
                    {
                        localGroup = groupInfo;
                        if (createdConversation != null && createdConversation.GroupID == localGroup.GroupID)
                        {
                            var oldConversation = conversation;
                            conversation = createdConversation;
                            RefreshUI();
                            GameEntry.Event.Fire(OnCreateGroup.EventId, new OnCreateGroup()
                            {
                                OldConversation = oldConversation,
                                NewConversation = conversation,
                                Group = localGroup,
                            });
                        }
                    }
                    else
                    {
                        GameEntry.UI.Tip(errMsg);
                    }
                }, new CreateGroupReq()
                {
                    // MemberUserIDs = memebers,
                    AdminUserIDs = memebers,
                    OwnerUserID = IMSDK.GetLoginUser(),
                    GroupInfo = new LocalGroup()
                    {
                        GroupType = (int)GroupType.Group,
                    }
                });
            }
            else if (conversation.ConversationType == (int)ConversationType.Group)
            {
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
                }, conversation.GroupID, "", memebers);
            }
        }

        void HandleConversationChange(object sender, GameEventArgs e)
        {
            var args = e as OnConversationChange;
            if (args.Created && args.Conversation != null)
            {
                createdConversation = args.Conversation;
            }
        }
    }
}

