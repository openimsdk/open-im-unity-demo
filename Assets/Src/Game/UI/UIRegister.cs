using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System.Text;
using System.Collections;
using Dawn.Game.Event;
namespace Dawn.Game.UI
{

    public class UIRegister : UGuiForm
    {
        Button backBtn;
        TMP_InputField userId;
        TMP_InputField nickName;
        Button headIconBtn;
        Image headIcon;
        Button registerBtn;
        Sprite defaultHeadIcon;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            backBtn = GetButton("Panel/top/back");
            userId = GetInputField("Panel/content/userid/input");
            nickName = GetInputField("Panel/content/nickname/input");
            headIconBtn = GetButton("Panel/content/headicon/icon");
            headIcon = GetImage("Panel/content/headicon/icon");
            registerBtn = GetButton("Panel/register");

            defaultHeadIcon = headIcon.sprite;
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            OnClick(backBtn, () =>
            {
                CloseSelf();
            });

            OnClick(headIconBtn, () =>
            {
                GameEntry.UI.OpenUI("SelectIcon", (OnSelectHeadIcon)OnSelectHeadIcon);
            });

            OnClick(registerBtn, () =>
            {
                if (userId.text == "")
                {
                    GameEntry.UI.Tip("UserId Is Empty");
                    return;
                }
                StartCoroutine(RegisterUser());
            });
        }
        public void OnSelectHeadIcon(string url)
        {
            SetImage(headIcon, url);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            headIcon.sprite = defaultHeadIcon;
            userId.text = "";
            nickName.text = "";
        }

        IEnumerator RegisterUser()
        {
            var url = string.Format("{0}{1}", Setting.Instance.HttpURL, "/user/user_register");
            Debug.Log(url);
            var userRegisterReq = new UserRegisterReq()
            {
                secret = "openIM123",
                users = new UserRegisterInfo[1]{
                        new UserRegisterInfo(){
                            userID = userId.text,
                            nickname = nickName.text,
                            faceURL = headIcon.sprite.name,
                        },
                    },
            };
            var bodyData = Encoding.UTF8.GetBytes(JsonUtility.ToJson(userRegisterReq));
            UnityWebRequest www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            DownloadHandler downloadHandler = new DownloadHandlerBuffer();
            www.downloadHandler = downloadHandler;
            www.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            www.SetRequestHeader("operationID", "111111");
            www.uploadHandler = new UploadHandlerRaw(bodyData);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
            {
                var data = www.downloadHandler.text;
                Debug.Log(data);
                var res = JsonUtility.FromJson<UserRegisterRes>(data);
                if (res.errCode > 0)
                {
                    GameEntry.UI.Tip(res.errMsg + ":" + res.errDlt);
                }
                else
                {
                    GameEntry.Event.Fire(OnRegisterUser.EventId, new OnRegisterUser()
                    {
                        UserID = userId.text
                    });
                    CloseSelf();
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

