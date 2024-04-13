using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using open_im_sdk;
namespace Dawn.Game
{
    public class SDKHelper : MonoBehaviour
    {

        public string wsAddr = "ws://14.29.168.56:20001";
        public string apiAddr = "http://14.29.168.56:20002";
        // string wsAddr = "ws://14.29.213.197:50001";
        // string apiAddr = "http://14.29.213.197:50002";
#if UNITY_EDITOR
        public string dataDir = "./OpenIM";
        public string logDir = "./OpenIM/Logs";
#else
        string dataDir = Application.persistentDataPath + "/OpenIM";
        string logDir = Application.persistentDataPath + "/OpenIM/Logs";
#endif
        void Awake()
        {
            Debug.Log("WsAddr:" + wsAddr);
            Debug.Log("APIAddr:" + apiAddr);
            Debug.Log("PlatformId:" + PlatformID + "(" + PlatformID + ")");
            Debug.Log("WsAddr:" + wsAddr);
            Debug.Log("APIAddr:" + apiAddr);
            Debug.Log("DataDir:" + dataDir);
            Debug.Log("LogDir:" + logDir);
            var config = new IMConfig()
            {
                PlatformID = (int)PlatformID,
                WsAddr = wsAddr,
                ApiAddr = apiAddr,
                DataDir = dataDir,
                LogLevel = 5,
                IsLogStandardOutput = true,
                LogFilePath = logDir,
                IsExternalExtensions = true,
            };
            var res = IMSDK.InitSDK(config, Player.Instance);
            if (!res)
            {
                Debug.Log("InitSDK:" + false);
            }
            Debug.Log("InitSDK:" + res);
        }
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

