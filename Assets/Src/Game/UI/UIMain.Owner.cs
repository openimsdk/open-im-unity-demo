using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using open_im_sdk;
using System.Collections.Generic;

namespace Dawn.Game.UI
{
    public partial class UIMain
    {
        RectTransform ownerRoot;
        Image ownerIcon;
        TextMeshProUGUI ownerName;
        Button setOwnerInfoBtn;
        Button logoutBtn;

        void InitOwner()
        {
            ownerRoot = GetRectTransform("Panel/content/center/owner");
            ownerIcon = GetImage("Panel/content/center/owner/top/icon");
            ownerName = GetTextPro("Panel/content/center/owner/top/name");
            setOwnerInfoBtn = GetButton("Panel/content/center/owner/center/setuserinfo");
            logoutBtn = GetButton("Panel/content/center/owner/center/logout");
        }

        void OpenOwner()
        {
            ownerName.text = Player.Instance.UserId;

            OnClick(logoutBtn, () =>
            {
                IMSDK.Logout((suc, err, errMsg) =>
                {
                    if (suc)
                    {
                        GameEntry.Event.Fire(this, new Event.OnLogout());
                    }
                    else
                    {
                        Debug.Log(err + ":" + errMsg);
                    }
                });
            });
        }
        void CloseOwner()
        {

        }
    }
}

