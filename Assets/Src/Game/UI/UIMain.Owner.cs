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
        void InitOwner()
        {
            ownerRoot = GetRectTransform("Panel/content/center/owner");
            ownerIcon = GetImage("Panel/content/center/owner/top/icon");
            ownerName = GetTextPro("Panel/content/center/owner/top/name");
        }

        void OpenOwner()
        {
            ownerName.text = Player.Instance.UserId;
            
        }
        void CloseOwner()
        {

        }
    }
}

