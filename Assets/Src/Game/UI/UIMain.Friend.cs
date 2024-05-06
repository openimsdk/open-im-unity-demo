using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using System.Collections.Generic;
using Dawn.Game.Event;
using GameFramework.Event;
using open_im_sdk;

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

        List<FullUserInfo> userInfos;
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
                var info = userInfos[index];
                item.Name.text = info.FriendInfo.Nickname;
                if (info.FriendInfo.FaceURL != "")
                {
                    SetImage(item.Icon, info.FriendInfo.FaceURL);
                }
                OnClick(item.Btn, () =>
                {
                    GameEntry.UI.OpenUI("UserInfo", info.FriendInfo.FriendUserID);
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
                    userInfos = list;
                    RefreshList(friendList, userInfos.Count);
                }
                else
                {
                    Debug.LogError(errMsg);
                }
            });
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

