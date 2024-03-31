using System.Data;
using open_im_sdk;
using UnityEngine;
public class ChatApp : SingletonMB<ChatApp>
{
    public UILogin UILogin;
    public UIMain UIMain;
    public string WsAddr = "ws://14.29.168.56:20001";
    public string ApiAddr = "http://14.29.168.56:20002";
    // public static string WsAddr = "ws://14.29.213.197:50001";
    // public static string ApiAddr = "http://14.29.213.197:50002";
    [HideInInspector]
    public LocalCacheData LocalCacheData;
    protected override void Awake()
    {
        base.Awake();

        var data = PlayerPrefs.GetString("ChatAppCachaData");
        if (data == "")
        {
            LocalCacheData = new LocalCacheData();
        }
        else
        {
            LocalCacheData = JsonUtility.FromJson<LocalCacheData>(data);
        }

        var suc = Player.CurPlayer.Init(WsAddr, ApiAddr);
        if (suc)
        {
            UILogin.transform.gameObject.SetActive(true);
            UIMain.transform.gameObject.SetActive(false);
        }
        else
        {
            UILogin.transform.gameObject.SetActive(false);
            UIMain.transform.gameObject.SetActive(false);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

    }


    void Start()
    {
        var dispator = Player.CurPlayer.Dispator;
        dispator.AddListener(EventType.OnLoginSuc, () =>
        {
            UILogin.gameObject.SetActive(false);
            UIMain.gameObject.SetActive(true);
        });
        Player.CurPlayer.Dispator.AddListener(EventType.OnTokenExpired, () =>
        {
            Debug.Log("Token Expired");
            UILogin.transform.gameObject.SetActive(true);
            UIMain.transform.gameObject.SetActive(false);
        });
    }
    void Update()
    {
        Player.CurPlayer.Update();
    }



    void OnApplicationQuit()
    {
        Debug.Log("ApplicationQuit");
        SaveLocalData();
        Player.CurPlayer.UnLogin();
        Player.CurPlayer.Destroy();
        PlayerPrefs.Save();
    }
    public void SaveLocalData()
    {
        var data = JsonUtility.ToJson(LocalCacheData);
        Debug.Log("Save ChatAppCachaData = " + data);
        PlayerPrefs.SetString("ChatAppCachaData", data);
    }
}
