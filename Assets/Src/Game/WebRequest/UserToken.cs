using System;

namespace Dawn.Game
{
    [Serializable]
    public class UserTokenReq
    {
        public string secret;
        public int platformID;
        public string userID;
    }
    [Serializable]
    public class UserToken
    {
        public string token;
        public long expireTimeSeconds;
    }
    [Serializable]
    public class UserTokenRes
    {
        public int errCode;
        public string errMsg;
        public string errDlt;
        public UserToken data;
    }
}

