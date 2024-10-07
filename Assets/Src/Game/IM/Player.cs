using OpenIM.IMSDK.Unity;
using UnityEngine;

namespace Dawn.Game
{
    public enum UserStatus
    {
        NoLogin, Logining, LoginSuc, LoginFailed
    }
    public enum ConnStatus
    {
        Empty, OnConnecting, ConnSuc, ConnFailed, KickOffline, TokenExpired
    }
    public class Player : IConnCallBack
    {
        static Player instance;
        public static Player Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Player();
                }
                return instance;
            }
        }
        public Conversation Conversation;
        public FriendShip FriendShip;
        public Group Group;
        public AdvancedMsg AdvancedMsg;
        public BatchMsg BatchMsg;
        public User User;
        public CustomBusiness CustomBusiness;
        public UserStatus Status = UserStatus.NoLogin;
        private Player()
        {
            Conversation = new Conversation();
            FriendShip = new FriendShip();
            Group = new Group();
            AdvancedMsg = new AdvancedMsg();
            BatchMsg = new BatchMsg();
            User = new User();
            CustomBusiness = new CustomBusiness();

            IMSDK.SetConversationListener(Conversation);
            IMSDK.SetGroupListener(Group);
            IMSDK.SetFriendShipListener(FriendShip);
            IMSDK.SetAdvancedMsgListener(AdvancedMsg);
            IMSDK.SetBatchMsgListener(BatchMsg);
            IMSDK.SetUserListener(User);
            IMSDK.SetCustomBusinessListener(CustomBusiness);
        }
        public void OnConnecting()
        {
            GameEntry.Event.FireNow(this, new Event.OnConnStatusChange()
            {
                ConnStatus = ConnStatus.OnConnecting
            });
        }

        public void OnConnectSuccess()
        {
            GameEntry.Event.FireNow(this, new Event.OnConnStatusChange()
            {
                ConnStatus = ConnStatus.ConnSuc
            });
        }
        public void OnConnectFailed(int errCode, string errMsg)
        {
            GameEntry.Event.FireNow(this, new Event.OnConnStatusChange()
            {
                ConnStatus = ConnStatus.ConnFailed
            });
        }
        public void OnKickedOffline()
        {
            GameEntry.Event.FireNow(this, new Event.OnConnStatusChange()
            {
                ConnStatus = ConnStatus.KickOffline
            });
        }
        public void OnUserTokenExpired()
        {
            GameEntry.Event.FireNow(this, new Event.OnConnStatusChange()
            {
                ConnStatus = ConnStatus.TokenExpired
            });
        }

        public void Login(string userId, string token)
        {
            Debug.Log("Login:" + userId + " " + token);
            IMSDK.Login(userId, token, (suc, errCode, errMsg) =>
            {
                if (suc)
                {
                    OnLoginSuc(userId, token);
                }
                else
                {
                    Debug.LogError(errCode + " " + errMsg);
                    Status = UserStatus.LoginFailed;
                    GameEntry.Event.Fire(this, new Event.OnLoginStatusChange()
                    {
                        UserStatus = UserStatus.LoginFailed
                    });
                }
            });
        }
        public void OnLoginSuc(string userId, string token)
        {
            Status = UserStatus.LoginSuc;
            GameEntry.Event.Fire(this, new Event.OnLoginStatusChange()
            {
                UserStatus = UserStatus.LoginSuc
            });
        }

        public static PlatformID PlatformID
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
    }
}
