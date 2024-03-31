using open_im_sdk;
using UnityEngine;
public enum UserStatus
{
    NoLogin, Logining, LoginSuc, LoginFailed
}

public class Player : IConnCallBack
{
    public string UserId;
    public string Token;
    private static Player curPlayer;
    public static Player CurPlayer
    {
        get
        {
            if (curPlayer == null)
            {
                curPlayer = new Player();
            }
            return curPlayer;
        }
    }
    private Player()
    {
        Dispator = new EventDispator();
    }
    public EventDispator Dispator;
    Conversation conversation;
    public Conversation Conversation
    {
        get
        {
            return conversation;
        }
    }
    FriendShip friendship;
    public FriendShip Friend
    {
        get
        {
            return friendship;
        }
    }
    Group group;
    public Group Group
    {
        get
        {
            return group;
        }
    }
    public UserStatus Status = UserStatus.NoLogin;

    public PlatformID PlatformID
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                return PlatformID.WindowsPlatformID;
            }
            else if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
            {
                return PlatformID.OSXPlatformID;
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                return PlatformID.AndroidPlatformID;
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return PlatformID.IOSPlatformID;
            }
            return PlatformID.None;
        }
    }

    public bool Init(string wsAddr, string apiAddr)
    {
#if UNITY_EDITOR
        string dataDir = "./OpenIM";
        string logDir = "./OpenIM/Logs";
#else
        string dataDir = Application.persistentDataPath + "/OpenIM";
        string logDir = Application.persistentDataPath + "/OpenIM/Logs";
#endif
        Debug.Log("PlatformId:" + PlatformID + "  " + (int)PlatformID);
        Debug.Log("WsAddr:" + wsAddr);
        Debug.Log("APIAddr:" + apiAddr);
        Debug.Log("DataDir:" + dataDir);
        Debug.Log("LogDir:" + logDir);
        var config = new IMConfig()
        {
            PlatformID = (int)PlatformID,
            WsAddr = wsAddr,
            ApiAddr = apiAddr,
            DataDir = dataDir,
            LogLevel = 5,
            IsLogStandardOutput = true,
            LogFilePath = logDir,
            IsExternalExtensions = true,
        };
        var res = IMSDK.InitSDK(config, this);
        if (!res)
        {
            Debug.Log("InitSDK:" + false);
            return false;
        }
        Debug.Log("InitSDK:" + res);
        conversation = new Conversation();
        friendship = new FriendShip();
        group = new Group();

        IMSDK.SetConversationListener(conversation);
        IMSDK.SetFriendShipListener(friendship);
        IMSDK.SetGroupListener(group);
        return true;
    }
    public void Destroy()
    {
        Debug.Log("Player:Destroy");
        IMSDK.UnInitSDK();
    }
    public void Update()
    {
        IMSDK.Polling();
    }
    public void OnConnecting()
    {
        Dispator.Broadcast(EventType.OnConnecting);
    }

    public void OnConnectSuccess()
    {
        Dispator.Broadcast(EventType.OnConnectSuccess);
    }

    public void OnConnectFailed(int errCode, string errMsg)
    {
        Dispator.Broadcast(EventType.OnConnectFailed, errCode, errMsg);
    }

    public void OnKickedOffline()
    {
        Dispator.Broadcast(EventType.OnKickedOffline);
    }

    public void OnUserTokenExpired()
    {
        Dispator.Broadcast(EventType.OnTokenExpired);
    }

    public void Login(string userId, string token)
    {
        Debug.Log("UserId:" + userId);
        Debug.Log("Token:" + token);
        IMSDK.Login(userId, token, (suc, errCode, errMsg) =>
        {
            if (suc)
            {
                OnLoginSuc(userId, token);
            }
        });
    }

    public void OnLoginSuc(string userId, string token)
    {
        Debug.Log("Login Suc");
        UserId = userId;
        Token = token;
        InitData();
        Dispator.Broadcast(EventType.OnLoginSuc);

        SaveUserToken();

    }

    public void SaveUserToken()
    {
        var localCacheData = ChatApp.GetInstance().LocalCacheData;
        for (int i = 0; i < localCacheData.LocalUserTokens.Count; i++)
        {
            if (localCacheData.LocalUserTokens[i].UserId == UserId)
            {
                localCacheData.LocalUserTokens.RemoveAt(i);
                break;
            }
        }
        localCacheData.LocalUserTokens.Add(new LocalUserIdToken()
        {
            UserId = UserId,
            Token = Token,
        });
        ChatApp.GetInstance().SaveLocalData();
    }

    public void UnLogin()
    {
        Debug.Log("Call UnLogin");
        IMSDK.Logout((suc, errCode, errMsg) =>
        {
            if (suc)
            {
                Debug.Log("Call UnLogin:" + true);
            }
            else
            {
                Debug.Log("Call UnLogin:" + errCode + errMsg);
            }
        });
    }

    public void InitData()
    {
        IMSDK.GetFriendList((list, err, msg) =>
        {
            if (list != null)
            {
                Debug.Log("Friend Count : " + list.Count);
                foreach (var info in list)
                {
                    Friend.AddLocalFriend(info.FriendInfo);
                }
                Dispator.Broadcast(EventType.OnFirendChange);
            }
            else
            {
                Debug.LogError(err + ":" + msg);
            }
        });
        IMSDK.GetFriendApplicationListAsApplicant((list, err, msg) =>
        {
            if (list != null)
            {
                Debug.Log("Friend Request Count : " + list.Count);
                Friend.RequestList.AddRange(list);
                Dispator.Broadcast(EventType.OnFirendRequestChange);
            }
            else
            {
                Debug.LogError(err + ":" + msg);
            }
        });
        IMSDK.GetFriendApplicationListAsRecipient((list, err, msg) =>
        {
            if (list != null)
            {
                Debug.Log("Friend Application Count : " + list.Count);
                Friend.ApplicationList.AddRange(list);
                Dispator.Broadcast(EventType.OnFirendApplicationChange);
            }
            else
            {
                Debug.LogError(err + ":" + msg);
            }
        });
    }
}