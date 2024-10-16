using OpenIM.IMSDK.Unity;
using OpenIM.IMSDK.Unity.Listener;
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
    public class Player
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

        public UserStatus Status = UserStatus.NoLogin;
        public Conn conn;
        public Conversation conversation;
        public FriendShip friend;
        public Group group;
        public AdvancedMsg advancedMsg;
        public BatchMsg batchMsg;
        public User user;
        public CustomBusiness customBusiness;
        private Player()
        {
            conn = new Conn();
            conversation = new Conversation();
            friend = new FriendShip();
            group = new Group();
            advancedMsg = new AdvancedMsg();
            batchMsg = new BatchMsg();
            user = new User();
            customBusiness = new CustomBusiness();
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

        public ListenGroup GetListenGroup()
        {
            return new ListenGroup(
                conn,
                conversation,
                group,
                friend,
                advancedMsg,
                user,
                customBusiness,
                batchMsg
            );
        }

        public void Login(string userId, string token)
        {
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
    }
}
