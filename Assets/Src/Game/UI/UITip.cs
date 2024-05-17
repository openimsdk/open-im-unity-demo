using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using UnityGameFramework.Runtime;
using System.Xml;

namespace Dawn.Game.UI
{

    public class UITip : UGuiForm
    {
        static float TipDuration = 1.5f;
        RectTransform tipRect;
        TextMeshProUGUI tipInfo;


        float showTipTimer;
        bool updateTip = false;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            tipRect = GetRectTransform("Panel/tip");
            tipInfo = GetTextPro("Panel/tip/val");
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            var tip = userData as string;
            ShowTip(tip);
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (updateTip)
            {
                showTipTimer -= elapseSeconds;
                if (showTipTimer <= 0)
                {
                    tipInfo.text = "";
                    tipRect.gameObject.SetActive(false);
                    updateTip = false;

                }
            }
        }
        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        public void ShowTip(string tip)
        {
            tipRect.gameObject.SetActive(true);
            tipInfo.text = tip;
            showTipTimer = TipDuration;
            updateTip = true;
        }
    }
}

