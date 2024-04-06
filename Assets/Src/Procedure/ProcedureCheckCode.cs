using System.Collections.Generic;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using UnityEngine;
using GameFramework.Resource;
using System.IO;
namespace Dawn
{
    public class ProcedureCheckCode: ProcedureBase
    {

        bool mNeedUpdateCode = false;
        bool mCanUpdateCode = false;
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
            GameEntry.BuiltinData.SetLoadingProgress(0.7f,"Check Code...");
            var versionInfo = GameEntry.BuiltinData.VersionInfo;
            mNeedUpdateCode =  versionInfo.InternalCodeVersion != GetCurCodeVersion();
            Debug.Log("InternalCodeVersion " + versionInfo.InternalCodeVersion + "  " + GetCurCodeVersion());
            if(mNeedUpdateCode) {
                GameEntry.Event.Subscribe(DownloadSuccessEventArgs.EventId,OnDownloadSuccess);
                GameEntry.Event.Subscribe(DownloadFailureEventArgs.EventId,OnDownloadFailure);
                var downloadPath = Application.persistentDataPath + "/Lua/LuaVersionInfoCache.xml";
                Debug.Log(downloadPath + " <- " + versionInfo.CodeVersionUri );
                if(File.Exists(downloadPath)){
                    File.Delete(downloadPath);
                }
                GameEntry.Download.AddDownload(downloadPath,versionInfo.CodeVersionUri,this);
            }
        }
        public int GetCurCodeVersion(){
            if (GameEntry.Setting.HasSetting("InternalCodeVersion")){
                return  GameEntry.Setting.GetInt("InternalCodeVersion");
            }else{
                return 0;
            }
        }
        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            if (mNeedUpdateCode){
                GameEntry.Event.Unsubscribe(DownloadSuccessEventArgs.EventId,OnDownloadSuccess);
                GameEntry.Event.Unsubscribe(DownloadFailureEventArgs.EventId,OnDownloadFailure);
            }
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (mNeedUpdateCode){
                if (mCanUpdateCode){
                    ChangeState<ProcedureUpdateCode>(procedureOwner);
                }
            }else{
                ChangeState<ProcedurePreload>(procedureOwner);
            }
        }

        private void OnDownloadSuccess(object sender, GameEventArgs e)
        {
            DownloadSuccessEventArgs args = e as DownloadSuccessEventArgs;
            if (args.UserData != this){
                return;
            }
            Debug.Log(args.DownloadUri + args.DownloadPath);
            mCanUpdateCode = true;
        }

        private void OnDownloadFailure(object sender, GameEventArgs e)
        {
            DownloadFailureEventArgs args = e as DownloadFailureEventArgs;
            if (args.UserData != this){
                return;
            }
            Debug.Log("Download Failuer - >" + args.DownloadUri);
        }
    }
}