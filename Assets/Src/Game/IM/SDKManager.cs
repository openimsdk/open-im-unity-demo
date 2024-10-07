using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenIM.IMSDK.Unity;
namespace Dawn.Game
{
    public class SDKManager : MonoBehaviour
    {

        void Start()
        {
            var setting = Setting.Instance;
            var config = new IMConfig()
            {
                PlatformID = (int)Player.PlatformID,
                WsAddr = setting.WSAddr,
                ApiAddr = setting.APIAddr,
                DataDir = setting.DataDir,
                LogLevel = setting.LogLevel,
                IsLogStandardOutput = true,
                LogFilePath = setting.LogDir,
                IsExternalExtensions = true,
            };
            var res = IMSDK.InitSDK(config, Player.Instance);
            if (!res)
            {
                Debug.Log("InitSDK:" + false);
            }
            Debug.Log("InitSDK:" + res);
        }
        void Update()
        {
            IMSDK.Polling();
        }

        void OnApplicationQuit()
        {
#if !UNITY_EDITOR
            IMSDK.UnInitSDK();
#endif
        }
    }
}

