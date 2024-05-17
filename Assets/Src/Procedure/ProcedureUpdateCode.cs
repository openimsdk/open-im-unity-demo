using System.Collections.Generic;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using System.Xml;
using System.IO;
using UnityEngine;
namespace Dawn
{
    public class LuaFileInfo
    {
        // 文件名
        public string Path;
        // 文件标识
        public int CRC32;
        public LuaFileInfo()
        {
            Path = "";
            CRC32 = 0;
        }
        public LuaFileInfo(string path, int crc)
        {
            Path = path;
            CRC32 = crc;
        }
        public override string ToString()
        {
            return Path + ":" + ":" + CRC32;
        }
    }
    public class ProcedureUpdateCode : ProcedureBase
    {
        string LuaListInfoPathCache = Application.persistentDataPath + "/Lua/LuaVersionInfoCache.xml";
        string LuaListInfoPath = Application.persistentDataPath + "/Lua/LuaVersionInfo.xml";
        List<string> mNeedDownloadCodeList = new List<string>();
        int mNeedDownLoadCount = 0;

        Dictionary<string, LuaFileInfo> mCacheCodeDic = new Dictionary<string, LuaFileInfo>();
        Dictionary<string, LuaFileInfo> mCodeDic = new Dictionary<string, LuaFileInfo>();

        private UpdateResourceForm m_UpdateResourceForm = null;

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }
        protected override void OnDestroy(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.BuiltinData.SetLoadingProgress(0.8f,"Update Code...");
            GameEntry.Event.Subscribe(DownloadSuccessEventArgs.EventId, OnDownloadSuccess);
            GameEntry.Event.Subscribe(DownloadFailureEventArgs.EventId, OnDownloadFailure);
            m_UpdateResourceForm = null;
            mCacheCodeDic.Clear();
            mCodeDic.Clear();
            SetCodeListInfo(mCacheCodeDic, LuaListInfoPathCache);
            SetCodeListInfo(mCodeDic, LuaListInfoPath);
            bool suc = false;
            LuaFileInfo temp = null;
            foreach (KeyValuePair<string, LuaFileInfo> kv in mCacheCodeDic)
            {
                suc = mCodeDic.TryGetValue(kv.Key, out temp);
                if (suc)
                {
                    if (temp.CRC32 != kv.Value.CRC32)
                    {
                        mNeedDownloadCodeList.Add(kv.Value.Path);
                    }
                }
                else
                {
                    mNeedDownloadCodeList.Add(kv.Value.Path);
                }
            }
            mNeedDownLoadCount = mNeedDownloadCodeList.Count;
            Debug.Log("Need Update Code File = " + mNeedDownLoadCount);
            if (mNeedDownLoadCount > 0)
            {
                var updateCodePrefix = GameEntry.BuiltinData.VersionInfo.UpdateCodePrefixUri;
                foreach (string path in mNeedDownloadCodeList)
                {
                    var downloadPath = Application.persistentDataPath + "/Lua/" + path;
                    var downloadUrl = updateCodePrefix + "/Lua/" + path;
                    GameEntry.Download.AddDownload(downloadPath, downloadUrl, this);
                }
                if (m_UpdateResourceForm == null)
                {
                    m_UpdateResourceForm = Object.Instantiate(GameEntry.BuiltinData.UpdateResourceFormTemplate);
                }
            }
        }

        private void SetCodeListInfo(Dictionary<string, LuaFileInfo> dic, string filePath)
        {
            if (File.Exists(filePath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNode codeList = doc.SelectSingleNode("List");
                XmlNodeList list = codeList.ChildNodes;
                XmlNode xmlNode = null;
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    xmlNode = list.Item(i);
                    if (xmlNode.Name != "XLua")
                    {
                        continue;
                    }
                    string path = xmlNode.Attributes.GetNamedItem("Path").Value;
                    string Crc32 = xmlNode.Attributes.GetNamedItem("Crc32").Value;
                    dic.Add(path, new LuaFileInfo(path, int.Parse(Crc32)));
                }
            }
            else
            {
                // TODO 没有Lua描述文件
            }
        }
        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            if (m_UpdateResourceForm != null)
            {
                Object.Destroy(m_UpdateResourceForm.gameObject);
                m_UpdateResourceForm = null;
            }

            GameEntry.Event.Unsubscribe(DownloadSuccessEventArgs.EventId, OnDownloadSuccess);
            GameEntry.Event.Unsubscribe(DownloadFailureEventArgs.EventId, OnDownloadFailure);

            // 文件重命名
            if (File.Exists(LuaListInfoPath))
            {
                File.Delete(LuaListInfoPath);
            }
            if (File.Exists(LuaListInfoPathCache))
            {
                File.Move(LuaListInfoPathCache, LuaListInfoPath);
            }
            if (mNeedDownLoadCount <= 0)
            {
                GameEntry.Setting.SetInt("InternalCodeVersion", GameEntry.BuiltinData.VersionInfo.InternalCodeVersion);
            }

        }
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (mNeedDownLoadCount <= 0)
            {
                ChangeState<ProcedurePreload>(procedureOwner);
            }
        }

        private void OnDownloadSuccess(object sender, GameEventArgs e)
        {
            DownloadSuccessEventArgs args = e as DownloadSuccessEventArgs;
            if (args.UserData != this)
            {
                return;
            }
            Debug.Log(args.DownloadUri + " -> " + args.DownloadPath + "----- " + mNeedDownLoadCount);
            mNeedDownLoadCount--;
            if (m_UpdateResourceForm != null){
                float ratio = 1.0f - (mNeedDownLoadCount* 1.0f / mNeedDownloadCodeList.Count * 1.0f);
                Debug.Log("ratio = " + ratio);
                var txt = Utility.Text.Format("Update Lua Code ：{0}/{1}",mNeedDownLoadCount,mNeedDownloadCodeList.Count);
                m_UpdateResourceForm.SetProgress(ratio, txt);
            }
        }

        private void OnDownloadFailure(object sender, GameEventArgs e)
        {
            DownloadFailureEventArgs args = e as DownloadFailureEventArgs;
            if (args.UserData != this)
            {
                return;
            }
            Debug.LogError("Download Fauil:" + args.DownloadUri + " -> " + args.DownloadPath);
        }
    }
}