using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using open_im_sdk;

namespace Dawn.Game.UI
{
    public class SerachResultItem
    {
        public Image Icon;
        public TextMeshProUGUI Name;
        public Button Btn;
    }

    public class UISearch : UGuiForm
    {
        TMP_InputField searchInput;
        Button searchBtn;
        Button backBtn;
        LoopListView2 searchResList;
        RectTransform searchEmpty;
        List<FullUserInfo> searchResultListInfo;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            searchInput = GetInputField("Panel/content/search/input");
            searchBtn = GetButton("Panel/content/search/serch");
            searchResList = GetListView("Panel/content/searchresult/list");
            backBtn = GetButton("Panel/content/search/back");
            searchEmpty = GetRectTransform("Panel/content/searchresult/empty");
            searchResList.InitListView(0, (list, index) =>
            {
                if (index < 0)
                {
                    return null;
                }
                var itemNode = list.NewListViewItem("item");
                if (!itemNode.IsInitHandlerCalled)
                {
                    itemNode.UserObjectData = new SerachResultItem()
                    {
                        Icon = GetImage("icon", itemNode.transform as RectTransform),
                        Name = GetTextPro("name", itemNode.transform as RectTransform),
                        Btn = GetButton("", itemNode.transform as RectTransform),
                    };
                    itemNode.IsInitHandlerCalled = true;
                }
                SerachResultItem item = itemNode.UserObjectData as SerachResultItem;
                FullUserInfo info = searchResultListInfo[index];
                item.Name.text = info.PublicInfo.UserID;
                OnClick(item.Btn, () =>
                {
                    CloseSelf();
                    GameEntry.UI.OpenUI("UserInfo", info.PublicInfo.UserID);
                });
                return itemNode;
            });
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            searchResList.gameObject.SetActive(false);
            searchEmpty.gameObject.SetActive(false);
            searchInput.text = "";
            searchInput.ActivateInputField();

            OnClick(searchBtn, () =>
            {
                if (searchInput.text == "")
                {
                    return;
                }
                IMSDK.GetUsersInfo((list, errCode, errMsg) =>
                {
                    if (list != null && list.Count > 0)
                    {
                        searchResultListInfo = list;
                    }
                    else
                    {
                        Debug.LogError(errCode + ":" + errMsg);
                    }
                    if (searchResultListInfo != null && searchResultListInfo.Count > 0)
                    {
                        searchResList.gameObject.SetActive(true);
                        searchEmpty.gameObject.SetActive(false);
                        RefreshList(searchResList, searchResultListInfo.Count);
                    }
                    else
                    {
                        searchResList.gameObject.SetActive(false);
                        searchEmpty.gameObject.SetActive(true);
                    }
                }, new string[] { searchInput.text });
            });
            OnClick(backBtn, () =>
            {
                CloseSelf();
            });

        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

    }
}

