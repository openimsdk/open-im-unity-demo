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
    public class ChatItem
    {
        public Image Icon;
        public TextMeshProUGUI Message;
    }

    public partial class UIMain
    {
        RectTransform friendRoot;

        Button newFriendBtn;
        LoopListView2 friendList;

        List<FullUserInfo> userInfos;
        void InitFriend()
        {
            friendRoot = GetRectTransform("Panel/content/center/friend");
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
                OnClick(item.Btn, () =>
                {
                    GameEntry.UI.OpenUI("UserInfo", info.FriendInfo.FriendUserID);
                });
                return itemNode;
            });
        }

        void OpenFriend()
        {
            OnClick(newFriendBtn, () =>
            {
                GameEntry.UI.OpenUI("NewFriend", this);
            });

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
        }
    }
}

