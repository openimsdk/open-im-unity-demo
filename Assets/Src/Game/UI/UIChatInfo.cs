using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using UnityGameFramework.Runtime;

namespace Dawn.Game.UI
{

    public class UIChatInfo : UGuiForm
    {
        Button backBtn;
        Button searchHistory;
        Button clearHistory;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            backBtn = GetButton("Panel/content/top/back");
            searchHistory = GetButton("Panel/content/center/searchhistory/btn");
            clearHistory = GetButton("Panel/content/center/clearhistory/btn");
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            OnClick(backBtn, () =>
            {
                CloseSelf();
            });
            OnClick(searchHistory, () => { });
            OnClick(clearHistory, () => { });
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}

