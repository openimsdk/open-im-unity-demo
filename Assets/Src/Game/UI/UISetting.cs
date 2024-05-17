using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperScrollView;
using UnityGameFramework.Runtime;
using System;

namespace Dawn.Game.UI
{

    public class UISetting : UGuiForm
    {
        Button backBtn;
        TMP_InputField wsAddr;
        TMP_InputField apiAddr;
        TMP_InputField dataDir;
        TMP_InputField logDir;
        TMP_InputField logLevel;
        TMP_InputField httpUrl;
        Button saveBtn;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            backBtn = GetButton("Panel/top/back");
            wsAddr = GetInputField("Panel/content/wsaddr/input");
            apiAddr = GetInputField("Panel/content/apiaddr/input");
            dataDir = GetInputField("Panel/content/datadir/input");
            logDir = GetInputField("Panel/content/logdir/input");
            logLevel = GetInputField("Panel/content/loglevel/input");
            httpUrl = GetInputField("Panel/content/httpurl/input");
            saveBtn = GetButton("Panel/save");
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            logLevel.onValueChanged.AddListener((v) =>
            {
                logLevel.text = Math.Clamp(int.Parse(v), 0, 7).ToString();
            });
            var setting = Setting.Instance;
            wsAddr.text = setting.WSAddr;
            apiAddr.text = setting.APIAddr;
            dataDir.text = setting.DataDir;
            logDir.text = setting.LogDir;
            logLevel.text = setting.LogLevel.ToString();
            httpUrl.text = setting.HttpURL;

            OnClick(saveBtn, () =>
            {
                setting.WSAddr = wsAddr.text;
                setting.APIAddr = apiAddr.text;
                setting.DataDir = dataDir.text;
                setting.LogDir = logDir.text;
                setting.LogLevel = uint.Parse(logLevel.text);
                setting.HttpURL = httpUrl.text;
                setting.Save();
            });
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

