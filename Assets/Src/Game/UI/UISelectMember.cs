using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using open_im_sdk;

namespace Dawn.Game.UI
{
    public delegate void OnSelectMember(FullUserInfo[] selectUsers);
    public class UISelectMember : UGuiForm
    {
        class Item
        {
            public Button Btn;
            public Image Icon;
            public TextMeshProUGUI Name;
            public RectTransform Select;
        }
        LoopListView2 list;
        Button backBtn;
        Button confirmBtn;
        OnSelectMember onSelectMember;
        Dictionary<int, FullUserInfo> selectMembers;
        List<FullUserInfo> friends;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            backBtn = GetButton("Panel/top/back");
            confirmBtn = GetButton("Panel/top/ok");
            list = GetListView("Panel/content/list");
            list.InitListView(0, (list, index) =>
            {
                if (index < 0) return null;
                var itemNode = list.NewListViewItem("item");
                if (!itemNode.IsInitHandlerCalled)
                {
                    var parent = itemNode.transform as RectTransform;
                    itemNode.UserObjectData = new Item()
                    {
                        Btn = GetButton("", parent),
                        Icon = GetImage("icon", parent),
                        Name = GetTextPro("name", parent),
                        Select = GetRectTransform("default/hot", parent)
                    };
                    itemNode.IsInitHandlerCalled = true;
                }
                var item = itemNode.UserObjectData as Item;
                var fullUserInfo = friends[index];
                bool hasSelect = selectMembers.ContainsKey(index);
                item.Select.gameObject.SetActive(hasSelect);
                SetImage(item.Icon, fullUserInfo.FriendInfo.FaceURL);
                item.Name.text = fullUserInfo.FriendInfo.Nickname;
                OnClick(item.Btn, () =>
                {
                    if (selectMembers.ContainsKey(index))
                    {
                        selectMembers.Remove(index);
                    }
                    else
                    {
                        selectMembers.Add(index, fullUserInfo);
                    }
                    RefreshList(this.list, friends.Count);
                });
                return itemNode;
            });

            selectMembers = new Dictionary<int, FullUserInfo>();
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            if (userData is OnSelectMember)
            {
                onSelectMember = userData as OnSelectMember;
            }
            OnClick(backBtn, () =>
            {
                CloseSelf();
            });
            OnClick(confirmBtn, () =>
            {
                if (selectMembers.Count > 0 && onSelectMember != null)
                {
                    var members = new FullUserInfo[selectMembers.Count];
                    int index = 0;
                    foreach (var member in selectMembers)
                    {
                        members[index] = member.Value;
                        index++;
                    }
                    onSelectMember(members);
                }
                CloseSelf();
            });

            list.SetListItemCount(0);
            IMSDK.GetFriendList((list, err, errMsg) =>
            {
                if (list != null)
                {
                    friends = list;
                    RefreshList(this.list, friends.Count);
                }
                else
                {
                    GameEntry.UI.Tip(errMsg);
                }
            });
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            friends.Clear();
            selectMembers.Clear();
        }

    }
}

