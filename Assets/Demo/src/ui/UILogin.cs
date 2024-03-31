using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using UnityEngine.SocialPlatforms;

public class UserHistoryItem
{
    public Button btn;
    public TextMeshProUGUI text;
}

public class UILogin : MonoBehaviour
{
    TMP_InputField userId;
    TMP_InputField token;
    Button loginBtn;

    Transform historyPanel;
    Button historyBtn;
    LoopListView2 historyList;
    Button closeHistoryBtn;
    void Start()
    {
        userId = transform.Find("userId/input").GetComponent<TMP_InputField>();
        token = transform.Find("token/input").GetComponent<TMP_InputField>();
        loginBtn = transform.Find("login").GetComponent<Button>();
        historyBtn = transform.Find("historybtn").GetComponent<Button>();
        historyPanel = transform.Find("historyPanel");
        historyList = historyPanel.Find("content/list").GetComponent<LoopListView2>();
        closeHistoryBtn = historyPanel.Find("content/close").GetComponent<Button>();

        historyPanel.gameObject.SetActive(false);

        int historyUserCount = ChatApp.GetInstance().LocalCacheData.LocalUserTokens.Count;
        if (historyUserCount > 0)
        {
            var localTokenData = ChatApp.GetInstance().LocalCacheData.LocalUserTokens[historyUserCount - 1];
            userId.text = localTokenData.UserId;
            token.text = localTokenData.Token;
        }
        else
        {
            userId.text = "";
            token.text = "";
        }

        loginBtn.onClick.RemoveAllListeners();
        loginBtn.onClick.AddListener(() =>
        {
            if (userId.text == "" || token.text == "")
            {
                return;
            }
            Player.CurPlayer.Login(userId.text, token.text);
        });
        historyBtn.onClick.RemoveAllListeners();
        historyBtn.onClick.AddListener(() =>
        {
            historyPanel.gameObject.SetActive(true);
            RefreshHistoryList();
        });
        closeHistoryBtn.onClick.RemoveAllListeners();
        closeHistoryBtn.onClick.AddListener(() =>
        {
            historyPanel.gameObject.SetActive(false);
        });

        historyList.InitListView(0, (list, index) =>
        {
            if (index < 0)
            {
                return null;
            }
            LoopListViewItem2 itemNode = null;
            itemNode = list.NewListViewItem("item");
            if (!itemNode.IsInitHandlerCalled)
            {
                itemNode.UserObjectData = new UserHistoryItem()
                {
                    btn = itemNode.transform.GetComponent<Button>(),
                    text = itemNode.transform.Find("userid").GetComponent<TextMeshProUGUI>(),
                };
                itemNode.IsInitHandlerCalled = true;
            }
            var item = itemNode.UserObjectData as UserHistoryItem;
            LocalUserIdToken data = ChatApp.GetInstance().LocalCacheData.LocalUserTokens[index];
            item.text.text = data.UserId;
            item.btn.onClick.RemoveAllListeners();
            item.btn.onClick.AddListener(() =>
            {
                userId.text = data.UserId;
                token.text = data.Token;
                historyPanel.gameObject.SetActive(false);
            });

            return itemNode;
        });
        historyList.SetListItemCount(0);

        
    }

    void RefreshHistoryList()
    {
        historyList.SetListItemCount(ChatApp.GetInstance().LocalCacheData.LocalUserTokens.Count);
        historyList.RefreshAllShownItem();
    }
}
