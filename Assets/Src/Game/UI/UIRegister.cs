using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System;
using System.Text;
using System.Collections;
using Dawn.Game.Event;
namespace Dawn.Game.UI
{

    public class UIRegister : UGuiForm
    {
        Button backBtn;
        TMP_InputField inputField;
        TMP_InputField verificationCode;
        TMP_InputField nickname;
        TMP_InputField password;
        TMP_InputField passwordc;
        Button headIconBtn;
        Image headIcon;
        Button registerBtn;
        Button sendCodeBtn;
        Sprite defaultHeadIcon;

        string selectHeadIcon = "";
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            backBtn = GetButton("Panel/top/back");
            inputField = GetInputField("Panel/content/phoneoremail/input");
            verificationCode = GetInputField("Panel/content/verificationcode/input");
            sendCodeBtn = GetButton("Panel/content/verificationcode/send");
            nickname = GetInputField("Panel/content/nickname/input");
            password = GetInputField("Panel/content/password/input");
            passwordc = GetInputField("Panel/content/passwordc/input");
            headIconBtn = GetButton("Panel/content/headicon/icon");
            headIcon = GetImage("Panel/content/headicon/icon");
            registerBtn = GetButton("Panel/register");

            defaultHeadIcon = headIcon.sprite;
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            selectHeadIcon = "headicon/不知火舞";
            OnClick(backBtn, () =>
            {
                CloseSelf();
            });

            OnClick(headIconBtn, () =>
            {
                GameEntry.UI.OpenUI("SelectIcon", (OnSelectHeadIcon)OnSelectHeadIcon);
            });

            OnClick(sendCodeBtn, () => 
            {
                if (!Tools.IsEmail(inputField.text) && !Tools.IsPhone(inputField.text))
                {
                    GameEntry.UI.Tip("Please enter a valid phone number or email.");
                    return;
                }
                StartCoroutine(SendVerificationCode(inputField.text));
            });

            OnClick(registerBtn, () =>
            {
                if (!Tools.IsEmail(inputField.text) && !Tools.IsPhone(inputField.text))
                {
                    GameEntry.UI.Tip("Please enter a valid phone number or email.");
                    return;
                }
                if (verificationCode.text == string.Empty) 
                {
                    GameEntry.UI.Tip("Please enter the verification code what we sent to your phone or email. ");
                    return;
                }
                if (nickname.text == string.Empty) 
                {
                    GameEntry.UI.Tip("Please enter a nickname for your account. ");
                    return;
                }
                if (password.text == string.Empty) 
                {
                    GameEntry.UI.Tip("Please enter a password for your account. ");
                    return;
                }
                if (!password.text.Equals(passwordc.text))
                {
                    GameEntry.UI.Tip("The passwords entered twice do not match. ");
                    return;
                }
                StartCoroutine(CheckAndRegister(inputField.text, verificationCode.text, nickname.text, password.text));
            });
        }
        public void OnSelectHeadIcon(string url)
        {
            SetImage(headIcon, url);
            selectHeadIcon = url;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            headIcon.sprite = defaultHeadIcon;
            inputField.text = "";
            verificationCode.text = "";
            password.text = "";
            passwordc.text = "";
        }

        IEnumerator SendVerificationCode(string phoneoremail) 
        {
            var parameter = new AccountReq
            {
                platform = (int)Player.PlatformID,
                operationID = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
                usedFor = AccountOperation.Register,
            };
            if (Tools.IsPhone(phoneoremail)) 
            {
                parameter.areaCode = "+86";
                parameter.phoneNumber = phoneoremail;
            }
            if (Tools.IsEmail(phoneoremail))
            {
                parameter.email = phoneoremail;
            }

            var request = Api.CreateRequest<AccountReq>(ChatUrl.VerificationCode_Send, parameter);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var responseJson = request.downloadHandler.text;
                Debug.Log(responseJson);
                var res = JsonUtility.FromJson<Base<string>>(responseJson);
                if (res == null)
                {
                    GameEntry.UI.Tip("Unexpected verification code response.");
                }
                else if (res.errCode != 0)
                {
                    var message = string.IsNullOrEmpty(res.errMsg) ? "Failed to request verification code." : res.errMsg;
                    if (!string.IsNullOrEmpty(res.errDlt))
                    {
                        message = string.Format("{0}: {1}", message, res.errDlt);
                    }
                    GameEntry.UI.Tip(message);
                }
                else
                {
                    GameEntry.UI.Tip("Verification code sent.");
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

        IEnumerator CheckAndRegister(string phoneoremail, string verificationCode, string nickName, string password, string areaCode = "+86")
        {
            var parameter = new AccountReq 
            {
                platform = (int) Player.PlatformID,
                operationID = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
                usedFor = AccountOperation.Register,
                verifyCode = verificationCode,
            };
            if (Tools.IsPhone(phoneoremail))
            {
                parameter.phoneNumber = phoneoremail;
                parameter.areaCode = areaCode;
            }
            if (Tools.IsEmail(phoneoremail))
            {
                parameter.email = phoneoremail;
            }

            var request = Api.CreateRequest<AccountReq>(ChatUrl.VerificationCode_Check, parameter);

            yield return request.SendWebRequest();

            bool checkSuc = false;

            if (request.result == UnityWebRequest.Result.Success)
            {
                var responseJson = request.downloadHandler.text;
                var res = JsonUtility.FromJson<Base<string>>(responseJson);
                if (res == null)
                {
                    GameEntry.UI.Tip("Unexpected verification code response.");
                }
                else if (res.errCode != 0)
                {
                    var message = string.IsNullOrEmpty(res.errMsg) ? "Failed to check verification code." : res.errMsg;
                    if (!string.IsNullOrEmpty(res.errDlt))
                    {
                        message = string.Format("{0}: {1}", message, res.errDlt);
                    }
                    GameEntry.UI.Tip(message);
                }
                else
                {
                    checkSuc = true;
                    GameEntry.UI.Tip("Verification code has been checked.");
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

            if (!checkSuc) 
            {
                yield break;
            }

            parameter = new AccountReq
            {
                verifyCode = verificationCode,
                platform = (int)Player.PlatformID,
                autoLogin = true,
            };
            parameter.user = new UserData
            {
                password = Tools.HashPassword(password),
                nickname = nickName,
            };
            if (Tools.IsPhone(phoneoremail))
            {
                parameter.user.phoneNumber = phoneoremail;
                parameter.user.areaCode = areaCode;
            }
            if (Tools.IsEmail(phoneoremail))
            {
                parameter.user.email = phoneoremail;
            }

            request = Api.CreateRequest<AccountReq>(ChatUrl.Register, parameter);
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                var responseJson = request.downloadHandler.text;
                var res = JsonUtility.FromJson<Base<string>>(responseJson);
                if (res == null)
                {
                    GameEntry.UI.Tip("Unexpected register response.");
                }
                else if (res.errCode != 0)
                {
                    var message = string.IsNullOrEmpty(res.errMsg) ? "Failed to register." : res.errMsg;
                    if (!string.IsNullOrEmpty(res.errDlt))
                    {
                        message = string.Format("{0}: {1}", message, res.errDlt);
                    }
                    GameEntry.UI.Tip(message);
                }
                else
                {
                    GameEntry.Event.Fire(OnRegisterUser.EventId, new OnRegisterUser()
                    {
                        UserID = phoneoremail
                    });
                    CloseSelf();
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
    }
}

