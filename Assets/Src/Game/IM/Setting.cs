using UnityEngine;

namespace Dawn.Game
{
    public class Setting
    {
        // static bool dev = true;
        static bool dev = false;
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
            if (dev)
            {
                LoadDevSetting();
            }
            else
            {
                LoadReleaseSetting();
            }
        }

        public string WSAddr;
        public string APIAddr;
        public string DataDir;
        public string LogDir;
        public uint LogLevel;

        public string HttpURL;

        void LoadDevSetting()
        {
            WSAddr = GameEntry.Setting.GetString("wsaddr", "ws://192.168.101.9:10001");
            APIAddr = GameEntry.Setting.GetString("apiaddr", "http://192.168.101.9:10002");
            HttpURL = GameEntry.Setting.GetString("httpurl", "http://192.168.101.9:10002");
#if UNITY_EDITOR
            DataDir = GameEntry.Setting.GetString("datadir", "./OpenIM");
            LogDir = GameEntry.Setting.GetString("logdir", "./OpenIM/Logs");
#else
            DataDir = GameEntry.Setting.GetString("datadir",  Application.persistentDataPath + "/OpenIM");
            LogDir = GameEntry.Setting.GetString("logdir", Application.persistentDataPath + "/OpenIM/Logs");
#endif
            LogLevel = (uint)GameEntry.Setting.GetInt("loglevel", 5);
        }

        void LoadReleaseSetting()
        {
            WSAddr = GameEntry.Setting.GetString("wsaddr", "ws://14.29.168.56:10001");
            APIAddr = GameEntry.Setting.GetString("apiaddr", "http://14.29.168.56:10002");
            HttpURL = GameEntry.Setting.GetString("httpurl", "http://14.29.168.56:10002");
#if UNITY_EDITOR
            DataDir = GameEntry.Setting.GetString("datadir", "./OpenIM");
            LogDir = GameEntry.Setting.GetString("logdir", "./OpenIM/Logs");
#else
            DataDir = GameEntry.Setting.GetString("datadir",  Application.persistentDataPath + "/OpenIM");
            LogDir = GameEntry.Setting.GetString("logdir", Application.persistentDataPath + "/OpenIM/Logs");
#endif
            LogLevel = (uint)GameEntry.Setting.GetInt("loglevel", 5);
        }
        public void Save()
        {
            GameEntry.Setting.SetString("wsaddr", WSAddr);
            GameEntry.Setting.SetString("apiaddr", APIAddr);
            GameEntry.Setting.SetString("datadir", DataDir);
            GameEntry.Setting.SetString("logdir", LogDir);
            GameEntry.Setting.SetInt("loglevel", (int)LogLevel);
            GameEntry.Setting.SetString("httpurl", HttpURL);
            GameEntry.Setting.Save();
        }
    }
}

