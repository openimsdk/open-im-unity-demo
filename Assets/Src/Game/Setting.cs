using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dawn.Game
{
    public class Setting
    {
        static Setting instatnce;
        public static Setting Instance
        {
            get
            {
                if (instatnce == null)
                {
                    instatnce = new Setting();
                }
                return instatnce;
            }
        }

        private Setting()
        {
            WSAddr = GameEntry.Setting.GetString("wsaddr", "ws://14.29.168.56:20001");
            APIAddr = GameEntry.Setting.GetString("apiaddr", "http://14.29.168.56:20002");
#if UNITY_EDITOR
            DataDir = GameEntry.Setting.GetString("datadir", "./OpenIM");
            LogDir = GameEntry.Setting.GetString("logdir", "./OpenIM/Logs");
#else
            DataDir = GameEntry.Setting.GetString("datadir",  Application.persistentDataPath + "/OpenIM");
            LogDir = GameEntry.Setting.GetString("logdir", Application.persistentDataPath + "/OpenIM/Logs");
#endif
            LogLevel = (uint)GameEntry.Setting.GetInt("loglevel", 5);

            RegisterUserURL = GameEntry.Setting.GetString("registeruserlurl", "http://14.29.168.56:20002/user/user_register");
            GetTokenURL = GameEntry.Setting.GetString("gettokenurl", "http://14.29.168.56:20002/auth/user_token");
        }

        public string WSAddr;
        public string APIAddr;
        public string DataDir;
        public string LogDir;
        public uint LogLevel;

        public string RegisterUserURL;
        public string GetTokenURL;

        public void Save()
        {
            GameEntry.Setting.SetString("wsaddr", WSAddr);
            GameEntry.Setting.SetString("apiaddr", APIAddr);
            GameEntry.Setting.SetString("datadir", DataDir);
            GameEntry.Setting.SetString("logdir", LogDir);
            GameEntry.Setting.SetInt("loglevel", (int)LogLevel);
            GameEntry.Setting.SetString("registeruserlurl", RegisterUserURL);
            GameEntry.Setting.SetString("gettokenurl", GetTokenURL);
            GameEntry.Setting.Save();
        }
    }
}

