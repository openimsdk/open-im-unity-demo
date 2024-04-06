using System.IO;
using GameFramework.FileSystem;
using UnityEngine;
using GameFramework;
using GameFramework.Procedure;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Dawn
{
    public class ProcedureDownloadResource : ProcedureBase
    {
        public string LuaURL = Utility.Path.GetRemotePath(Path.Combine(Application.streamingAssetsPath, "Lua.dat")); 

        private bool m_DownloadResourcesComplete = false;
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);

            m_DownloadResourcesComplete = false;
            GameEntry.WebRequest.AddWebRequest(LuaURL,this);
        }

        private void OnWebRequestSuccess(object sender, GameEventArgs e){
            WebRequestSuccessEventArgs ne = (WebRequestSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }
            byte[] data = ne.GetWebResponseBytes();
            Debug.Log(string.Format("DownloadResource From {0} ",LuaURL));
            var fs = GameEntry.FileSystem.LoadFileSystem(LuaURL,FileSystemAccess.Read,data);
            m_DownloadResourcesComplete = true;
        }

        private void OnWebRequestFailure(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs ne = (WebRequestFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }
            Debug.Log(ne.WebRequestUri + " failed");
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_DownloadResourcesComplete)
            {
                return;
            }
            ChangeState<ProcedurePreload>(procedureOwner);
        }
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
        }
    }
}
