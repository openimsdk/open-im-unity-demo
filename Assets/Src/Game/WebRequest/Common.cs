using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Dawn.Game
{
    [Serializable]
    public class AccountReq
    {
        public int platform;
        public string operationID;
        public AccountOperation usedFor;
        public string email;
        public string phoneNumber;
        public string areaCode;
        public string verifyCode;
        public string password;
        public bool autoLogin;
        public UserData user;
    }

    [Serializable]
    public class UserData
    {
        public string nickname;
        public string password;
        public string areaCode;
        public string phoneNumber;
        public string email;
    }

    [Serializable]
    public enum AccountOperation : int
    {
        Register = 1,
        Reset = 2,
        Login = 3,
    }

    [Serializable]
    public class AccountData
    {
        public string nickname = "";
        public string faceURL = "";
        public string userID = "";
        public string imToken = "";
        public string chatToken = "";
        public bool allowAddFriend;
        public bool allowBeep;
        public bool allowVibration;
        public int globalRecvMsgOpt;
    }

    [Serializable]
    public class Base<T>
    {
        public int errCode;
        public string errMsg;
        public string errDlt;
        public T data;
    }

    public static class ChatUrl 
    {
        public const string Login = "/account/login";

        public const string Register = "/account/register";

        public const string VerificationCode_Send = "/account/code/send";

        public const string VerificationCode_Check = "/account/code/verify";
    }

    public static class Api
    {
        public static UnityWebRequest  CreateRequest<T>(string requestUrl, T payload)
        {
            var url = string.Format("{0}{1}", GameEntry.IM.authAddr, requestUrl);
            var json = JsonUtility.ToJson(payload);
            var body = Encoding.UTF8.GetBytes(json);

            var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST)
            {
                downloadHandler = new DownloadHandlerBuffer(),
                uploadHandler = new UploadHandlerRaw(body)
            };

            request.SetRequestHeader("Content-Type", "application/json; charset=utf-8");
            request.SetRequestHeader("token", "");
            request.SetRequestHeader("operationID", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString());
            return request;
        }
    }
}
