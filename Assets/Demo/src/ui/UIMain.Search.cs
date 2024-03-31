using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using open_im_sdk;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;

public class SerachResultItem
{
    public Image Icon;
    public TextMeshProUGUI Name;
    public Button Add;
}

public class FriendRequestItem
{
    public Image Icon;
    public TextMeshProUGUI Name;
}
public class FriendApplicationItem
{
    public Image Icon;
    public TextMeshProUGUI Name;
    public Button Agree;
    public Button Refuse;
}

public partial class UIMain
{
    Transform searchRoot;
    TMP_InputField searchUserId;
    Button searchBtn;
    Button clearSearchBtn;
    List<FullUserInfoWithCache> searchResultListInfo;
    Transform searchRes;
    LoopListView2 searchResList;
    LoopListView2 friendRequestList;
    LoopListView2 friendApplictionList;
    void InitSearchUI()
    {
        searchRoot = transform.Find("content/search");
        searchUserId = searchRoot.Find("search/userid").GetComponent<TMP_InputField>();
        searchBtn = searchRoot.Find("search/btn").GetComponent<Button>();
        clearSearchBtn = searchRoot.Find("search/clear").GetComponent<Button>();
        searchRes = searchRoot.Find("searchresult");
        searchResList = searchRoot.Find("searchresult/list").GetComponent<LoopListView2>();
        friendRequestList = searchRoot.Find("friendrequestlist").GetComponent<LoopListView2>();
        friendApplictionList = searchRoot.Find("friendapplictionlist").GetComponent<LoopListView2>();

        OnClick(searchBtn, () =>
        {
            if (searchUserId.text == "")
            {
                return;
            }
            IMSDK.GetUsersInfoWithCache((list, errCode, errMsg) =>
            {
                if (list != null && list.Count > 0)
                {
                    searchResultListInfo = list;
                    searchRes.gameObject.SetActive(true);
                    searchResList.SetListItemCount(searchResultListInfo.Count);
                }
            }, new string[] { searchUserId.text }, "");
        });
        OnClick(clearSearchBtn, () =>
        {
            searchResultListInfo = null;
            searchUserId.text = "";
            searchRes.gameObject.SetActive(false);
        });
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
                    Icon = itemNode.transform.Find("icon").GetComponent<Image>(),
                    Name = itemNode.transform.Find("name").GetComponent<TextMeshProUGUI>(),
                    Add = itemNode.transform.Find("add").GetComponent<Button>(),
                };
                itemNode.IsInitHandlerCalled = true;
            }
            SerachResultItem item = itemNode.UserObjectData as SerachResultItem;
            FullUserInfoWithCache info = searchResultListInfo[index];
            item.Name.text = info.PublicInfo.UserID;
            OnClick(item.Add, () =>
            {
                IMSDK.AddFriend((suc, errCoce, errMsg) =>
                {

                }, new ApplyToAddFriendReq()
                {
                    FromUserID = Player.CurPlayer.UserId,
                    ToUserID = info.PublicInfo.UserID,
                    ReqMsg = "",
                    Ex = "",
                });
            });
            return itemNode;
        });
        searchResList.SetListItemCount(0);
        searchRes.gameObject.SetActive(false);

        friendRequestList.InitListView(0, (list, index) =>
        {
            if (index < 0)
            {
                return null;
            }
            var itemNode = list.NewListViewItem("item");
            if (!itemNode.IsInitHandlerCalled)
            {
                itemNode.UserObjectData = new FriendRequestItem()
                {
                    Icon = itemNode.transform.Find("icon").GetComponent<Image>(),
                    Name = itemNode.transform.Find("name").GetComponent<TextMeshProUGUI>(),
                };
                itemNode.IsInitHandlerCalled = true;
            }
            FriendRequestItem item = itemNode.UserObjectData as FriendRequestItem;
            LocalFriendRequest info = Player.CurPlayer.Friend.RequestList[index];
            item.Name.text = info.ToUserID;
            return itemNode;
        });

        friendRequestList.SetListItemCount(Player.CurPlayer.Friend.RequestList.Count);

        friendApplictionList.InitListView(0, (list, index) =>
        {
            if (index < 0)
            {
                return null;
            }
            var itemNode = list.NewListViewItem("item");
            if (!itemNode.IsInitHandlerCalled)
            {
                itemNode.UserObjectData = new FriendApplicationItem()
                {
                    Icon = itemNode.transform.Find("icon").GetComponent<Image>(),
                    Name = itemNode.transform.Find("name").GetComponent<TextMeshProUGUI>(),
                    Agree = itemNode.transform.Find("agree").GetComponent<Button>(),
                    Refuse = itemNode.transform.Find("refuse").GetComponent<Button>(),
                };
                itemNode.IsInitHandlerCalled = true;
            }
            FriendApplicationItem item = itemNode.UserObjectData as FriendApplicationItem;
            LocalFriendRequest info = Player.CurPlayer.Friend.ApplicationList[index];
            item.Name.text = info.FromUserID;
            OnClick(item.Agree, () =>
            {
                IMSDK.AcceptFriendApplication((suc, err, msg) =>
                {

                }, new ProcessFriendApplicationParams()
                {
                    ToUserID = info.FromUserID,
                    HandleMsg = "",
                });
            });
            OnClick(item.Refuse, () =>
            {
                IMSDK.RefuseFriendApplication((suc, err, msg) =>
                {

                }, new ProcessFriendApplicationParams()
                {
                    ToUserID = info.FromUserID,
                    HandleMsg = "",
                });
            });
            return itemNode;
        });
        friendApplictionList.SetListItemCount(Player.CurPlayer.Friend.ApplicationList.Count);
    }

    void RefreshFriendRequestList()
    {
        friendRequestList.SetListItemCount(Player.CurPlayer.Friend.RequestList.Count);
        friendRequestList.RefreshAllShownItem();
    }
    void RefreshFriendApplicationList()
    {
        friendApplictionList.SetListItemCount(Player.CurPlayer.Friend.ApplicationList.Count);
        friendApplictionList.RefreshAllShownItem();
    }
}