using GameFramework;
using GameFramework.Procedure;
using GameFramework.Event;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Dawn
{
    public class ProcedureCheckVersion : ProcedureBase
    {
        private bool m_CheckVersionComplete = false;
        private bool m_NeedUpdateVersion = false;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.BuiltinData.SetLoadingProgress(0.2f,"Check Version...");
            m_CheckVersionComplete = false;
            m_NeedUpdateVersion = false;

            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);

            // 向服务器请求版本信息
            string versionUrl = Utility.Text.Format(GameEntry.BuiltinData.CheckVersionUrl + "?platform={0}", GetPlatformPath());
            Log.Info("Req VersionUrl =  " + versionUrl);
            GameEntry.WebRequest.AddWebRequest(versionUrl,this);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_CheckVersionComplete)
            {
                return;
            }
            var versionInfo = GameEntry.BuiltinData.VersionInfo;
            if (m_NeedUpdateVersion)
            {
                ChangeState<ProcedureUpdateVersion>(procedureOwner);
            }
            else
            {
                ChangeState<ProcedureCheckResources>(procedureOwner);
            }
        }

        private void GotoUpdateApp(object userData)
        {
            string url = null;
// #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
//             // url = GameEntry.BuiltinData.BuildInfo.WindowsAppUrl;
// #elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
//             url = GameEntry.BuiltinData.BuildInfo.MacOSAppUrl;
// #elif UNITY_IOS
//             url = GameEntry.BuiltinData.BuildInfo.IOSAppUrl;
// #elif UNITY_ANDROID
//             url = GameEntry.BuiltinData.BuildInfo.AndroidAppUrl;
// #endif
            if (!string.IsNullOrEmpty(url))
            {
                Application.OpenURL(url);
            }
        }

        private void OnWebRequestSuccess(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs ne = (WebRequestSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            // 解析版本信息
            byte[] versionInfoBytes = ne.GetWebResponseBytes();
            string versionInfoString = Utility.Converter.GetString(versionInfoBytes);
            Debug.Log(versionInfoString);
            GameEntry.BuiltinData.VersionInfo = Utility.Json.ToObject<VersionInfo>(versionInfoString);
            VersionInfo versionInfo = GameEntry.BuiltinData.VersionInfo;
            if (versionInfo == null)
            {
                Log.Error("Parse VersionInfo failure.");
                UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit);
                return;
            }
            Log.Info("Latest game version is '{0} ({1})', local game version is '{2} ({3})'.", versionInfo.LatestGameVersion, versionInfo.InternalGameVersion.ToString(), Version.GameVersion, Version.InternalGameVersion.ToString());

            if (versionInfo.ForceUpdateGame)
            {
                // TODO 需要强制更新游戏应用
                UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit);
                return;
            }

            // 设置资源更新下载地址
            GameEntry.Resource.UpdatePrefixUri = Utility.Path.GetRegularPath(versionInfo.UpdatePrefixUri);

            m_CheckVersionComplete = true;
            m_NeedUpdateVersion = GameEntry.Resource.CheckVersionList(versionInfo.InternalResourceVersion) == CheckVersionListResult.NeedUpdate;
        }

        private void OnWebRequestFailure(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs ne = (WebRequestFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }
            Log.Warning("Check version failure, error message is '{0}'.", ne.ErrorMessage);
            // TODO
        }

        private string GetPlatformPath()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    return "Windows";

                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    return "MacOS";

                case RuntimePlatform.IPhonePlayer:
                    return "IOS";

                case RuntimePlatform.Android:
                    return "Android";

                default:
                    throw new System.NotSupportedException(Utility.Text.Format("Platform '{0}' is not supported.", Application.platform.ToString()));
            }
        }
    }
}
