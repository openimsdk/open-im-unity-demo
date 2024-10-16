using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenIM.IMSDK.Unity;
using UnityGameFramework.Runtime;
using System.IO;
namespace Dawn.Game
{
    public class IMComponent : GameFrameworkComponent
    {
        public string wsAddr = "ws://192.168.101.9:10001";
        public string apiAddr = "http://192.168.101.9:10002";
        public string dataDir = "./OpenIM";
        public string logDir = "./OpenIM/Logs";
        public uint logLevel = 5;
        public string imAdminToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySUQiOiJpbUFkbWluIiwiUGxhdGZvcm1JRCI6MTAsImV4cCI6MTczNjgxNjM0OCwibmJmIjoxNzI5MDQwMDQ4LCJpYXQiOjE3MjkwNDAzNDh9.ZWbe_KqfRBQyiyN6kt-DQFoWpp2UJwdhgJMMJiYMeKA";
        void Start()
        {
            var config = new IMConfig()
            {
                PlatformID = (int)Player.PlatformID,
                WsAddr = wsAddr,
                ApiAddr = apiAddr,
                DataDir = Path.Combine(Application.persistentDataPath, dataDir),
                LogFilePath = Path.Combine(Application.persistentDataPath, logDir),
                LogLevel = logLevel,
                IsLogStandardOutput = true,
                IsExternalExtensions = true,
            };
            var res = IMSDK.InitSDK(config, Player.Instance.GetListenGroup());
            if (!res)
            {
                Debug.Log("InitSDK:" + false);
            }
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

