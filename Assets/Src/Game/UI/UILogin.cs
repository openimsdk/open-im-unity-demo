using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using Dawn.Game.Event;
using System.Text;
using GameFramework.Event;

namespace Dawn.Game.UI
{
    public class UILogin : UGuiForm
    {
        TMP_InputField userId;
        TMP_InputField token;
        Button loginBtn;
        Button registerBtn;
        Button requestTokenBtn;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            userId = GetInputField("Panel/userId/input");
            token = GetInputField("Panel/token/input");
            loginBtn = GetButton("Panel/login");
            registerBtn = GetButton("Panel/register");

            requestTokenBtn = GetButton("Panel/token/requesttoken");
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            userId.text = GameEntry.Setting.GetString("lastUserId", "");
            token.text = GameEntry.Setting.GetString("lastUserToken", "");
            OnClick(loginBtn, () =>
            {
                login();
            });
            OnClick(registerBtn, () =>
            {
                GameEntry.UI.OpenUI("Register");
            });

            OnClick(requestTokenBtn, () =>
            {
                StartCoroutine(RefreshToken());
            });

            GameEntry.Event.Subscribe(OnRegisterUser.EventId, HandleRegisterUser);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(OnRegisterUser.EventId, HandleRegisterUser);

            GameEntry.Setting.SetString("lastUserId", userId.text);
            GameEntry.Setting.SetString("lastUserToken", token.text);
            GameEntry.Setting.Save();
        }

        void login()
        {
            if (userId.text == "")
            {
                GameEntry.UI.Tip("UserId is Empty");
                return;
            }
            if (token.text == "")
            {
                GameEntry.UI.Tip("Token is Empty");
                return;
            }
            Player.Instance.Login(userId.text, token.text);
        }

        void HandleRegisterUser(object sender, GameEventArgs e)
        {
            var args = e as OnRegisterUser;
            userId.text = args.UserID;
            StartCoroutine(RefreshToken());
        }

        IEnumerator RefreshToken()
        {
            var url = string.Format("{0}{1}", GameEntry.IM.apiAddr, "/auth/get_user_token");
            Debug.Log(url);
            var userTokenReq = new UserTokenReq()
            {
                secret = "openIM123",
                platformID = (int)Player.PlatformID,
                userID = userId.text,
            };
            var reqStr = JsonUtility.ToJson(userTokenReq);
            Debug.Log(reqStr);
            var bodyData = Encoding.UTF8.GetBytes(reqStr);
            UnityWebRequest www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            DownloadHandler downloadHandler = new DownloadHandlerBuffer();
            www.downloadHandler = downloadHandler;
            www.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            www.SetRequestHeader("operationID", "123456");
            www.SetRequestHeader("token", GameEntry.IM.imAdminToken);
            www.uploadHandler = new UploadHandlerRaw(bodyData);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                var data = www.downloadHandler.text;
                Debug.Log(data);
                var res = JsonUtility.FromJson<UserTokenRes>(data);
                if (res.errCode > 0)
                {
                    GameEntry.UI.Tip(res.errMsg + ":" + res.errDlt);
                }
                else
                {
                    var userToken = res.data;
                    token.text = userToken.token;
                }
            }
            else
            {
                var err = $"HTTP request failed with status code {www.responseCode}: {www.error}";
                GameEntry.UI.Tip(err);
            }
            www.downloadHandler.Dispose();
            www.uploadHandler.Dispose();
            www.Dispose();
        }
    }
}

