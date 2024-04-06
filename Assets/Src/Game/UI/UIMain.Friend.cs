using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using System.Collections.Generic;
using Dawn.Game.Event;
using GameFramework.Event;

namespace Dawn.Game.UI
{
    public class FriendItem
    {
        public Button Btn;
        public Image Bg;
        public Image Icon;
        public TextMeshProUGUI Name;
    }
    public class ChatItem
    {
        public Image Icon;
        public TextMeshProUGUI Message;
    }

    public partial class UIMain
    {
        RectTransform friendRoot;

        Button searchFriendBtn;
        Button newFriendBtn;
        LoopListView2 friendList;
        void InitFriend()
        {
            friendRoot = GetRectTransform("Panel/content/center/friend");
            searchFriendBtn = GetButton("Panel/content/center/friend/search/btn");
            newFriendBtn = GetButton("Panel/content/center/friend/newfriend/btn");
            friendList = GetListView("Panel/content/center/friend/list");
        }

        void OpenFriend()
        {
            OnClick(searchFriendBtn, () =>
            {
                GameEntry.UI.OpenUI("Search", this);
            });
            OnClick(newFriendBtn, () =>
            {
                GameEntry.UI.OpenUI("NewFriend", this);
            });

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
                var info = Player.Instance.FriendShip.FriendList[index];
                item.Name.text = info.FriendUserID;
                OnClick(item.Btn, () =>
                {
                    GameEntry.UI.OpenUI("Chat", info);
                });
                return itemNode;
            });
            RefreshList(friendList, Player.Instance.FriendShip.FriendList.Count);

            GameEntry.Event.Subscribe(OnFriendAdd.EventId, handleFriendAdd);
        }

        void handleFriendAdd(object sender, GameEventArgs gameEventArgs)
        {
            RefreshList(friendList, Player.Instance.FriendShip.FriendList.Count);
        }

        void CloseFriend()
        {
            GameEntry.Event.Unsubscribe(OnFriendAdd.EventId, handleFriendAdd);
        }
    }
}

