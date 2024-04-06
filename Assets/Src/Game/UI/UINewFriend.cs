using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using UnityGameFramework.Runtime;
using open_im_sdk;

namespace Dawn.Game.UI
{
    public class UserRequestItem
    {
        public Image Icon;
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Tip;
        public RectTransform MenuRect;
        public Button AcceptBtn;
        public Button RefuseBtn;
    }
    public class UINewFriend : UGuiForm
    {
        Button backBtn;
        LoopListView2 requestList;
        List<LocalFriendRequest> requestInfoList;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            backBtn = GetButton("Panel/content/top/back");
            requestList = GetListView("Panel/content/list");
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            OnClick(backBtn, () =>
            {
                CloseSelf();
            });
            requestList.InitListView(0, (list, index) =>
            {
                if (index < 0)
                {
                    return null;
                }
                if (requestInfoList.Count <= index)
                {
                    return null;
                }
                LoopListViewItem2 itemNode = list.NewListViewItem("item");
                var info = requestInfoList[index];
                if (!itemNode.IsInitHandlerCalled)
                {
                    var parent = itemNode.transform as RectTransform;
                    itemNode.UserObjectData = new UserRequestItem()
                    {
                        Icon = GetImage("icon", parent),
                        Name = GetTextPro("name", parent),
                        Tip = GetTextPro("tip", parent),
                        MenuRect = GetRectTransform("menu", parent),
                        AcceptBtn = GetButton("menu/accept", parent),
                        RefuseBtn = GetButton("menu/refuse", parent)
                    };
                    itemNode.IsInitHandlerCalled = true;
                }
                var item = itemNode.UserObjectData as UserRequestItem;
                item.Name.text = info.FromUserID;
                if (Player.Instance.FriendShip.HasFriend(info.FromUserID))
                {
                    item.Tip.text = "已添加";
                    item.MenuRect.gameObject.SetActive(false);
                }
                else
                {
                    item.Tip.text = "";
                    item.MenuRect.gameObject.SetActive(true);
                    OnClick(item.AcceptBtn, () =>
                    {
                        IMSDK.AcceptFriendApplication((suc, errCode, errMsg) =>
                        {
                            if (suc)
                            {
                                RefreshList(requestList, requestInfoList.Count);
                            }
                            else
                            {
                                Debug.Log(errCode + ":" + errMsg);
                            }
                        }, new ProcessFriendApplicationParams()
                        {
                            ToUserID = info.FromUserID
                        });
                    });
                    OnClick(item.RefuseBtn, () =>
                    {
                        IMSDK.RefuseFriendApplication((suc, errCode, errMsg) =>
                        {
                            if (suc)
                            {

                            }
                            else
                            {
                                Debug.Log(errCode + ":" + errMsg);
                            }
                        }, new ProcessFriendApplicationParams()
                        {
                            ToUserID = info.FromUserID
                        });
                    });
                }

                return itemNode;
            });

            IMSDK.GetFriendApplicationListAsRecipient((list, errCode, errMsg) =>
            {
                if (list != null)
                {
                    requestInfoList = list;
                    RefreshList(requestList, requestInfoList.Count);
                }
                else
                {
                    Debug.Log(errCode + ":" + errMsg);
                }
            });
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            requestList.Reset();
        }
    }
}

