using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using open_im_sdk;
public enum MenuType
{
    World, Channel, Group, Friend, Search
}


public partial class UIMain : MonoBehaviour
{
    TextMeshProUGUI userId;
    Toggle[] toggles;
    Transform worldTrans;
    Transform channelTrans;
    Transform groupTrans;
    Transform friendTrans;
    Transform searchTrans;
    void Start()
    {
        RegisterUI();
        InitListen();
    }
    void Update()
    {

    }
    void RegisterUI()
    {
        userId = transform.Find("content/left/userId").GetComponent<TextMeshProUGUI>();
        toggles = new Toggle[5];
        toggles[0] = transform.Find("content/left/menu/world").GetComponent<Toggle>();
        toggles[1] = transform.Find("content/left/menu/channel").GetComponent<Toggle>();
        toggles[2] = transform.Find("content/left/menu/group").GetComponent<Toggle>();
        toggles[3] = transform.Find("content/left/menu/friend").GetComponent<Toggle>();
        toggles[4] = transform.Find("content/left/menu/search").GetComponent<Toggle>();
        toggles[0].onValueChanged.AddListener((ison) =>
        {
            worldTrans.gameObject.SetActive(ison);
        });
        toggles[1].onValueChanged.AddListener((ison) =>
        {
            channelTrans.gameObject.SetActive(ison);
        });
        toggles[2].onValueChanged.AddListener((ison) =>
        {
            groupTrans.gameObject.SetActive(ison);
        });
        toggles[3].onValueChanged.AddListener((ison) =>
        {
            friendTrans.gameObject.SetActive(ison);
            if (ison)
            {
                RefreshFriendList();
            }
        });
        toggles[4].onValueChanged.AddListener((ison) =>
        {
            searchTrans.gameObject.SetActive(ison);
        });
        toggles[0].isOn = true;
        worldTrans = transform.Find("content/world").transform;
        channelTrans = transform.Find("content/channel").transform;
        groupTrans = transform.Find("content/group").transform;
        friendTrans = transform.Find("content/friend").transform;
        searchTrans = transform.Find("content/search").transform;
        worldTrans.gameObject.SetActive(true);
        channelTrans.gameObject.SetActive(false);
        groupTrans.gameObject.SetActive(false);
        friendTrans.gameObject.SetActive(false);
        searchTrans.gameObject.SetActive(false);


        userId.text = Player.CurPlayer.UserId;

        InitWorldUI();
        InitChannelUI();
        InitGroupUI();
        InitFriendUI();
        InitSearchUI();
    }
    public void InitListen()
    {
        var dispator = Player.CurPlayer.Dispator;
        dispator.AddListener<LocalConversation>(EventType.OnConversationChange, (converation) =>
        {
            OnConverationChange(converation);
        });
        dispator.AddListener(EventType.OnFirendChange, () =>
        {
            RefreshFriendList();
        });
        dispator.AddListener(EventType.OnFirendRequestChange, () =>
        {
            RefreshFriendRequestList();
        });
        dispator.AddListener(EventType.OnFirendApplicationChange, () =>
        {
            RefreshFriendApplicationList();

        });
        dispator.AddListener(EventType.OnTokenExpired, () =>
        {
            gameObject.SetActive(false);
        });
    }

    public void OnClick(Button btn, UnityAction onClick)
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(onClick);
    }
}
