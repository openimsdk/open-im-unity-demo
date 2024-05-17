using GameFramework.Fsm;
using GameFramework.Procedure;
using Dawn.Game.Event;
using GameFramework.Event;
using open_im_sdk;
using UnityEngine;
namespace Dawn.Game
{
    public class ProcedureLogin : ProcedureBase
    {
        IFsm<IProcedureManager> procedureOwner;

        int UILoginId = -1;
        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
            this.procedureOwner = procedureOwner;
        }
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Event.Subscribe(OnLoginStatusChange.EventId, HandlerLoginStatusChange);
            GameEntry.Event.Subscribe(OnConnStatusChange.EventId, HandleConnStatusChange);

            UILoginId = -1;
            if (IMSDK.GetLoginStatus() == LoginStatus.Logged)
            {
                ChangeState<ProcedureMain>(procedureOwner);
            }
            else
            {
                UILoginId = GameEntry.UI.OpenUI("Login");
            }
        }

        void HandlerLoginStatusChange(object sender, GameEventArgs eventArgs)
        {
            OnLoginStatusChange args = eventArgs as OnLoginStatusChange;
            if (args.UserStatus == UserStatus.LoginSuc)
            {
            }
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(OnLoginStatusChange.EventId, HandlerLoginStatusChange);
            GameEntry.Event.Unsubscribe(OnConnStatusChange.EventId, HandleConnStatusChange);
            if (UILoginId > 0)
            {
                GameEntry.UI.CloseUIForm(UILoginId);
            }

        }
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }
        protected override void OnDestroy(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        void HandleConnStatusChange(object sender, GameEventArgs e)
        {
            var args = e as OnConnStatusChange;
            if (args.ConnStatus == ConnStatus.KickOffline)
            {
                GameEntry.UI.Tip("KickOffline");
            }
            else if (args.ConnStatus == ConnStatus.ConnFailed)
            {
                GameEntry.UI.Tip("ConnFailed");
            }
            else if (args.ConnStatus == ConnStatus.TokenExpired)
            {
                GameEntry.UI.Tip("TokenExpired");
            }
            else if (args.ConnStatus == ConnStatus.ConnSuc)
            {
                ChangeState<ProcedureMain>(procedureOwner);
            }
        }
    }
}