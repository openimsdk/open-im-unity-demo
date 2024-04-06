using open_im_sdk;
using UnityEngine;

namespace Dawn.Game
{
    public enum UserStatus
    {
        NoLogin, Logining, LoginSuc, LoginFailed
    }

    public class Player : IConnCallBack
    {
        public string UserId;
        public string Token;
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

            });
        }
        public void OnConnectSuccess()
        {
            GameEntry.Event.FireNow(this, new Event.OnConnStatusChange()
            {

            });
        }
        public void OnConnectFailed(int errCode, string errMsg)
        {
            GameEntry.Event.FireNow(this, new Event.OnConnStatusChange()
            {

            });
        }
        public void OnKickedOffline()
        {
            GameEntry.Event.FireNow(this, new Event.OnConnStatusChange()
            {

            });
        }
        public void OnUserTokenExpired()
        {
            GameEntry.Event.FireNow(this, new Event.OnConnStatusChange()
            {

            });
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
            UserId = userId;
            Token = token;
            Status = UserStatus.LoginSuc;
            LoadData();

            GameEntry.Event.Fire(this, new Event.OnLoginStatusChange()
            {
                UserStatus = UserStatus.LoginSuc
            });
        }
        public void UnLogin()
        {
            IMSDK.Logout((suc, errCode, errMsg) =>
            {
                if (suc)
                {
                }
                else
                {
                }
            });
        }

        public void LoadData()
        {
            IMSDK.GetFriendList((list, err, msg) =>
            {
                if (list != null)
                {
                    foreach (var info in list)
                    {
                        FriendShip.AddLocalFriend(info.FriendInfo);
                    }
                    GameEntry.Event.Fire(this, new Event.OnFriendAdd()
                    {
                        List = list
                    });
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
                    FriendShip.RequestList.AddRange(list);
                    // Dispator.Broadcast(EventType.OnFirendRequestChange);
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
                    FriendShip.ApplicationList.AddRange(list);
                    // Dispator.Broadcast(EventType.OnFirendApplicationChange);
                }
                else
                {
                    Debug.LogError(err + ":" + msg);
                }
            });
        }
    }
}