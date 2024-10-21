using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using System.Collections.Generic;
using Dawn.Game.Event;
using GameFramework.Event;
using OpenIM.IMSDK.Unity;

namespace Dawn.Game.UI
{
    public class FriendItem
    {
        public Button Btn;
        public Image Bg;
        public Image Icon;
        public TextMeshProUGUI Name;
    }

    public partial class UIMain
    {
        RectTransform friendRoot;
        Button groupBtn;
        Button newFriendBtn;
        LoopListView2 friendList;
        List<FriendInfo> friends;
        void InitFriend()
        {
            friendRoot = GetRectTransform("Panel/content/center/friend");
            groupBtn = GetButton("Panel/content/center/friend/group");
            newFriendBtn = GetButton("Panel/content/center/friend/newfriend");
            friendList = GetListView("Panel/content/center/friend/list");

            friendList.InitListView(0, (list, index) =>
            {
                if (index < 0)
                {
                    return null;
                }
                var itemNode = list.NewListViewItem("item");
                if (!itemNode.IsInitHandlerCalled)
                {
                    itemNode.UserObjectData = new FriendItem()
                    {
                        Icon = itemNode.transform.Find("icon").GetComponent<Image>(),
                        Name = itemNode.transform.Find("name").GetComponent<TextMeshProUGUI>(),
                        Bg = itemNode.transform.Find("bg").GetComponent<Image>(),
                        Btn = itemNode.transform.GetComponent<Button>(),
                    };
                    itemNode.IsInitHandlerCalled = true;
                }
                FriendItem item = itemNode.UserObjectData as FriendItem;
                var info = friends[index];
                item.Name.text = info.Nickname;
                if (info.FaceURL != "")
                {
                    SetImage(item.Icon, info.FaceURL);
                }
                OnClick(item.Btn, () =>
                {
                    GameEntry.UI.OpenUI("UserInfo", info.FriendUserID);
                });
                return itemNode;
            });
        }

        void OpenFriend()
        {
            OnClick(groupBtn, () =>
            {
                GameEntry.UI.OpenUI("GroupList", this);
            });
            OnClick(newFriendBtn, () =>
            {
                GameEntry.UI.OpenUI("NewFriend", this);
            });
            RefreshFriendList();
            GameEntry.Event.Subscribe(OnFriendChange.EventId, HandleFriendChange);
        }

        void RefreshFriendList()
        {
            IMSDK.GetFriendList((list, err, errMsg) =>
            {
                if (list != null)
                {
                    friends = list;
                    RefreshList(friendList, friends.Count);
                }
                else
                {
                    Debug.LogError(errMsg);
                }
            }, true);
        }
        void CloseFriend()
        {
            GameEntry.Event.Unsubscribe(OnFriendChange.EventId, HandleFriendChange);
        }

        void HandleFriendChange(object sender, GameEventArgs e)
        {
            OnFriendChange args = e as OnFriendChange;
            if (args.Operation == FriendOperation.Added || args.Operation == FriendOperation.Deleted || args.Operation == FriendOperation.InfoChanged)
            {
                RefreshFriendList();
            }
        }
    }
}

