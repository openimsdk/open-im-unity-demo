using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
                
            });
        }
        public void OnSelectHeadIcon(Sprite sprite)
        {
            headIcon.sprite = sprite;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);

            headIcon.sprite = defaultHeadIcon;
            userId.text = "";
            nickName.text = "";
        }

    }
}

