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

        OpenIM.IMSDK.Unity.Conversation conversation;
        OpenIM.IMSDK.Unity.Conversation createdConversation;
        List<GroupMember> membersInfo;

        GroupInfo localGroup;

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
                        GameEntry.UI.OpenUI("SelectMember", (OnSelectFriends)OnSelectFriends);
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
            conversation = userData as OpenIM.IMSDK.Unity.Conversation;
            OnClick(backBtn, () =>
            {
                CloseSelf();
            });
            OnClick(searchHistory, () =>
            {
                GameEntry.UI.Tip("TODO");
            });
            OnClick(clearChat, () =>
            {
                IMSDK.ClearConversationAndDeleteAllMsg((suc, err, errMsg) =>
                {
                    if (suc)
                    {
                        GameEntry.UI.Tip("删除成功");
                        GameEntry.Event.Fire(OnConversationChange.EventId, new OnConversationChange
                        {
                            Conversation = conversation,
                            ClearHistory = true,
                        });
                        CloseSelf();
                    }
                    else
                    {
                        GameEntry.UI.Tip(errMsg);
                    }
                }, conversation.ConversationID);
            });

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

                membersInfo = new List<GroupMember>();
                membersInfo.Add(new GroupMember()
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

        void OnSelectFriends(FriendInfo[] selectUsers)
        {
            if (selectUsers.Length <= 0) return;
            var name = "";
            var members = new string[selectUsers.Length];
            for (int i = 0; i < selectUsers.Length; i++)
            {
                name = name + selectUsers[i].Nickname;
                members[i] = selectUsers[i].FriendUserID;
            }
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
                            GameEntry.Event.Fire(OnGroupChange.EventId, new OnGroupChange()
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
                    MemberUserIDs = members,
                    AdminUserIDs = members,
                    OwnerUserID = IMSDK.GetLoginUserId(),
                    GroupInfo = new GroupInfo()
                    {
                        GroupType = (int)GroupType.Group,
                        GroupName = name,
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
                }, conversation.GroupID, "", members);
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

