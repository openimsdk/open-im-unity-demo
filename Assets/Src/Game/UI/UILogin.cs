using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using Dawn.Game.Event;
using GameFramework.Event;

namespace Dawn.Game.UI
{
    public class UILogin : UGuiForm
    {
        TMP_InputField account;
        TMP_InputField password;
        Button loginBtn;
        Button registerBtn;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            account = GetInputField("Panel/account/input");
            password = GetInputField("Panel/password/input");
            loginBtn = GetButton("Panel/login");
            registerBtn = GetButton("Panel/register");
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            var lastAccount = GameEntry.Setting.GetString("lastAccount", string.Empty);
            account.text = lastAccount;
            password.text = string.Empty;
            OnClick(loginBtn, () =>
            {
                login();
            });
            OnClick(registerBtn, () =>
            {
                GameEntry.UI.OpenUI("Register");
            });

            GameEntry.Event.Subscribe(OnRegisterUser.EventId, HandleRegisterUser);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            GameEntry.Event.Unsubscribe(OnRegisterUser.EventId, HandleRegisterUser);
            GameEntry.Setting.SetString("lastAccount", account.text.Trim());
            GameEntry.Setting.Save();
        }

        void login()
        {
            var phoneoremail = account.text.Trim();
            var pwd = password.text;
            if (phoneoremail == string.Empty)
            {
                GameEntry.UI.Tip("Phone number or email is required.");
                return;
            }
            if (pwd == string.Empty)
            {
                GameEntry.UI.Tip("Password is required.");
                return;
            }
            if (!Tools.IsEmail(phoneoremail) && !Tools.IsPhone(phoneoremail))
            {
                GameEntry.UI.Tip("Please enter a valid phone number or email.");
                return;
            }
            StartCoroutine(LoginWithPassword(phoneoremail, pwd));
        }

        void HandleRegisterUser(object sender, GameEventArgs e)
        {
            var args = e as OnRegisterUser;
            account.text = args.UserID;
        }

        IEnumerator LoginWithPassword(string account, string pwd, string areaCode = "+86")
        {
            var requestPayload = new AccountReq
            {
                password = Tools.HashPassword(pwd),
                usedFor = AccountOperation.Login,
                platform = (int)Player.PlatformID,
                operationID = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString()
            };
            if (!TryFillAccountContact(requestPayload, account, areaCode))
            {
                GameEntry.UI.Tip("Please enter a valid phone number or email.");
                yield break;
            }
            var request = Api.CreateRequest<AccountReq>(ChatUrl.Login, requestPayload);

            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                var responseJson = request.downloadHandler.text;
                Debug.Log(responseJson);
                var res = JsonUtility.FromJson<Base<AccountData>>(responseJson);
                if (res == null)
                {
                    GameEntry.UI.Tip("Unexpected login response.");
                }
                else if (res.errCode != 0)
                {
                    var message = string.IsNullOrEmpty(res.errMsg) ? "Login failed." : res.errMsg;
                    if (!string.IsNullOrEmpty(res.errDlt))
                    {
                        message = string.Format("{0}: {1}", message, res.errDlt);
                    }
                    GameEntry.UI.Tip(message);
                }
                else if (res.data == null || string.IsNullOrEmpty(res.data.userID) || string.IsNullOrEmpty(res.data.imToken))
                {
                    GameEntry.UI.Tip("Login succeeded but no credentials returned.");
                }
                else
                {
                    GameEntry.Setting.SetString("lastAccount", account);
                    GameEntry.Setting.Save();
                    Player.loginCertificate = res.data;
                    Debug.LogWarningFormat("uid: {0} imToken: {1} chatToken: {2}", res.data.userID, res.data.imToken, res.data.chatToken);
                    Player.Instance.Login(res.data.userID, res.data.imToken);
                }
            }
            else
            {
                var err = $"HTTP request failed with status code {request.responseCode}: {request.error}";
                GameEntry.UI.Tip(err);
            }
            request.downloadHandler.Dispose();
            request.uploadHandler.Dispose();
            request.Dispose();
        }

        bool TryFillAccountContact(AccountReq payload, string account, string areaCode)
        {
            if (payload == null)
            {
                return false;
            }

            payload.email = null;
            payload.phoneNumber = null;
            payload.areaCode = null;

            if (Tools.IsEmail(account))
            {
                payload.email = account;
                return true;
            }

            if (Tools.IsPhone(account))
            {
                payload.phoneNumber = account;
                payload.areaCode = string.IsNullOrEmpty(areaCode) ? "+86" : areaCode;
                return true;
            }

            return false;
        }
    }
}
