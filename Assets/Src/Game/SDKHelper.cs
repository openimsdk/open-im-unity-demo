using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using open_im_sdk;
namespace Dawn.Game
{
    public class SDKHelper : MonoBehaviour
    {
        public PlatformID PlatformID
        {
            get
            {
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    return PlatformID.WindowsPlatformID;
                }
                else if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
                {
                    return PlatformID.OSXPlatformID;
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    return PlatformID.AndroidPlatformID;
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    return PlatformID.IOSPlatformID;
                }
                return PlatformID.None;
            }
        }
        void Start()
        {
            var setting = Setting.Instance;
            var config = new IMConfig()
            {
                PlatformID = (int)PlatformID,
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

