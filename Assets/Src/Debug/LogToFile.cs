using System;
using UnityEngine;
using System.IO;

namespace Dawn
{
    public class LogToFile : MonoBehaviour
    {
        string mLogPath;
        void Awake()
        {
        }
        void Start()
        {
            string logTime = DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute;
            var logFileName = logTime;
            var dir = GetLogDirPath();
            if (dir != "")
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                mLogPath = string.Format("{0}/{1}yejianlog.txt", dir, logTime);
                Debug.Log("LogFilePath => " + mLogPath);
                CreateLogFile();
            }

        }
        /// <summary>
        /// 获取日志目录
        /// </summary>
        /// <param name="logName"></param>
        private string GetLogDirPath()
        {
            string path = "";
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    {
                        path = string.Format("{0}/log", Application.persistentDataPath);
                    }
                    break;
                case RuntimePlatform.IPhonePlayer:
                    {
                        path = string.Format("{0}/log", Application.persistentDataPath);
                    }
                    break;
                case RuntimePlatform.WindowsPlayer:
                    {
                        path = string.Format("{0}/log", Application.dataPath);
                    }
                    break;
                case RuntimePlatform.WindowsEditor:
                    {
                        path = string.Format("{0}/log", Application.persistentDataPath);
                    }
                    break;
            }
            return path;
        }

        /// <summary>
        /// 根据路径创建日记文件，并注册文件写入的函数。
        /// </summary>
        private void CreateLogFile()
        {
            if (File.Exists(mLogPath))
            {
                File.Delete(mLogPath);
            }
            try
            {
                FileStream fs = File.Create(mLogPath);
                fs.Close();

                if (File.Exists(mLogPath))
                {
                    Debug.Log(string.Format("Create file = {0}", mLogPath));
                }

                /// 注册事件，当Debug调用时，就会调用：
                Application.logMessageReceived += OnLogCallBack;

                OutputSystemInfo();
            }
            catch (System.Exception ex)
            {
                Debug.LogError(string.Format("can't Create file = {0},\n{1}", mLogPath, ex));
            }
        }

        /// <summary>
        /// 日记文件写入的函数
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="stackTrace"></param>
        /// <param name="type"></param>
        private void OnLogCallBack(string condition, string stackTrace, LogType type)
        {
            if (File.Exists(mLogPath))
            {
                var filestream = File.Open(mLogPath, FileMode.Append);
                using (StreamWriter sw = new StreamWriter(filestream))
                //using (StreamWriter sw = File.AppendText(mLogPath))
                {
                    string logStr = string.Empty;
                    switch (type)
                    {
                        case LogType.Log:
                            {
                                logStr = string.Format("{0}：{1}", type, condition);
                            }
                            break;
                        case LogType.Assert:
                        case LogType.Warning:
                        case LogType.Exception:
                        case LogType.Error:
                            {
                                logStr = string.IsNullOrEmpty(stackTrace) ? string.Format("{0}：{1}\n{2}", type, condition, Environment.StackTrace) : string.Format("{0}：{1}{2}", type, condition, stackTrace);
                            }
                            break;
                    }

                    sw.WriteLine(logStr);

                }
                filestream.Close();
            }
            else
            {
                Debug.LogError(string.Format("not Exists File = {0} ！", mLogPath));
            }
        }

        /// <summary>
        /// 输出系统/硬件等一些信息
        /// </summary>
        private void OutputSystemInfo()
        {
            string str2 = string.Format("日志记录开始时间: {0}, 版本: {1}.", DateTime.Now.ToString(), Application.unityVersion);

            string systemInfo = SystemInfo.operatingSystem + " "
                                + SystemInfo.processorType + " " + SystemInfo.processorCount + " "
                                + "存储容量:" + SystemInfo.systemMemorySize + " "
                                + "图形设备: " + SystemInfo.graphicsDeviceName + " 供应商: " + SystemInfo.graphicsDeviceVendor
                                + " 存储容量: " + SystemInfo.graphicsMemorySize + " " + SystemInfo.graphicsDeviceVersion;

            Debug.Log(string.Format("{0}\n{1}", str2, systemInfo));
        }

    }
}

