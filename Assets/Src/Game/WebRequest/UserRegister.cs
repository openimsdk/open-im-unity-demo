using System;

namespace Dawn.Game
{
    [Serializable]
    public class UserRegisterInfo
    {
        public string userID;
        public string nickname;
        public string faceURL;
    }

    public class UserRegisterReq
    {
        public string secret;
        public UserRegisterInfo[] users;
    }

    public class UserRegisterRes
    {
        public int errCode;
        public string errMsg;
        public string errDlt;
    }
}


