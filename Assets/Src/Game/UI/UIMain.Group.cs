using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using System.Collections.Generic;
using GameFramework.Event;
using open_im_sdk;

namespace Dawn.Game.UI
{
    public class GroupItem
    {
        public Button Btn;
        public Image Bg;
        public Image Icon;
        public TextMeshProUGUI Name;
    }
    public partial class UIMain
    {
        RectTransform groupRoot;
        Button createGroupBtn;
        LoopListView2 groupList;
        List<LocalGroup> localGroups;
        void InitGroup()
        {
            groupRoot = GetRectTransform("Panel/content/center/group");
            createGroupBtn = GetButton("Panel/content/center/group/create");
            groupList = GetListView("Panel/content/center/group/list");
            groupList.InitListView(0, (list, index) =>
            {
                if (index < 0) return null;
                if (localGroups.Count <= index) return null;
                LoopListViewItem2 itemNode = null;
                var info = localGroups[index];
                if (!itemNode.IsInitHandlerCalled)
                {
                    itemNode.UserObjectData = new GroupItem()
                    {
                        Icon = itemNode.transform.Find("icon").GetComponent<Image>(),
                        Name = itemNode.transform.Find("name").GetComponent<TextMeshProUGUI>(),
                        Bg = itemNode.transform.Find("bg").GetComponent<Image>(),
                        Btn = itemNode.transform.GetComponent<Button>(),
                    };
                    itemNode.IsInitHandlerCalled = true;
                }
                GroupItem item = itemNode.UserObjectData as GroupItem;
                item.Name.text = info.GroupName;
                OnClick(item.Btn, () =>
                {
                    IMSDK.GetOneConversation((conversation, err, errMsg) =>
                    {
                        if (conversation != null)
                        {
                            GameEntry.UI.OpenUI("Chat", conversation);
                        }
                        else
                        {
                            Debug.LogError(err + ":" + errMsg);
                        }
                    }, (int)ConversationType.Group, info.GroupID);
                });
                return itemNode;
            });
        }

        void OpenGroup()
        {
            OnClick(createGroupBtn, () =>
            {
                GameEntry.UI.OpenUI("CreateGroup");
            });

            IMSDK.GetJoinedGroupList((list, err, errMsg) =>
            {
                if (list != null)
                {
                    localGroups = list;
                    RefreshList(groupList, localGroups.Count);
                }
                else
                {
                    Debug.Log(errMsg);
                }
            });
        }

        void CloseGroup()
        {

        }
    }
}


