using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using UnityGameFramework.Runtime;
using UnityEditor.UI;

namespace Dawn.Game.UI
{

    public class UICreateGroup : UGuiForm
    {
        public Button backBtn;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            backBtn = GetButton("Panel/content/top/back");

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            OnClick(backBtn, () =>
            {
                CloseSelf();
            });
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}

