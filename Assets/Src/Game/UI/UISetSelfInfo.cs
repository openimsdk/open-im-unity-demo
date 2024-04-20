using UnityEngine;
using TMPro;
using UnityEngine.UI;
using open_im_sdk;
using Dawn.Game.Event;

namespace Dawn.Game.UI
{

    public class UISetSelfInfo : UGuiForm
    {
        Button backBtn;
        TextMeshProUGUI userId;
        TMP_InputField nickName;
        Button headIconBtn;
        Image headIcon;
        Button saveBtn;
        LocalUser localUser;
        string headIconURL = "";
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            backBtn = GetButton("Panel/top/back");
            userId = GetTextPro("Panel/content/userid/val");
            nickName = GetInputField("Panel/content/nickname/input");
            headIconBtn = GetButton("Panel/content/headicon/icon");
            headIcon = GetImage("Panel/content/headicon/icon");
            saveBtn = GetButton("Panel/save");
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            userId.text = "";
            nickName.text = "";
            headIcon.sprite = null;
            IMSDK.GetSelfUserInfo((localUser, err, errMsg) =>
            {
                this.localUser = localUser;
                if (localUser != null)
                {
                    userId.text = localUser.UserID;
                    nickName.text = localUser.Nickname;
                    SetImage(headIcon, localUser.FaceURL);
                }
                else
                {
                    Debug.LogError(errMsg);
                }
            });

            OnClick(backBtn, () =>
            {
                CloseSelf();
            });

            OnClick(headIconBtn, () =>
            {
                GameEntry.UI.OpenUI("SelectIcon", (OnSelectHeadIcon)OnSelectHeadIcon);
            });

            OnClick(saveBtn, () =>
            {
                if (this.localUser != null)
                {
                    IMSDK.SetSelfInfo((suc, err, errMsg) =>
                    {
                        if (suc)
                        {
                            GameEntry.Event.Fire(OnSelfInfoChange.EventId, new OnSelfInfoChange());
                            GameEntry.UI.Tip("Save Success");
                            CloseSelf();
                        }
                        else
                        {
                            GameEntry.UI.Tip(errMsg);
                        }
                    }, new UserInfo()
                    {
                        UserID = localUser.UserID,
                        Nickname = nickName.text,
                        FaceURL = headIconURL,
                    });
                }
            });
        }
        public void OnSelectHeadIcon(string url)
        {
            headIconURL = url;
            SetImage(headIcon, url);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            userId.text = "";
            nickName.text = "";
        }

    }
}

