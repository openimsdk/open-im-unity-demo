using GameFramework.Fsm;
using GameFramework.Event;
using GameFramework.Procedure;
using Dawn.Game.Event;

namespace Dawn.Game
{
    public class ProcedureMain : ProcedureBase
    {
        IFsm<IProcedureManager> procedureOwner;
        int uiMainId;


        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
            this.procedureOwner = procedureOwner;
        }
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Event.Subscribe(Event.OnLogout.EventId, HandleLogOut);
            GameEntry.Event.Subscribe(Event.OnConnStatusChange.EventId, HandleConnStatusChange);
            uiMainId = GameEntry.UI.OpenUI("Main");
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.UI.CloseUIForm(uiMainId);
            GameEntry.Event.Unsubscribe(Event.OnLogout.EventId, HandleLogOut);
            GameEntry.Event.Unsubscribe(Event.OnConnStatusChange.EventId, HandleConnStatusChange);
        }
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        }
        protected override void OnDestroy(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        void HandleLogOut(object sender, GameEventArgs e)
        {
            ChangeState<ProcedureLogin>(this.procedureOwner);
        }

        void HandleConnStatusChange(object sender, GameEventArgs e)
        {
            var args = e as OnConnStatusChange;
            if (args.ConnStatus == ConnStatus.KickOffline)
            {
                GameEntry.UI.Tip("KickOffline");
                ChangeState<ProcedureLogin>(this.procedureOwner);
            }
            else if (args.ConnStatus == ConnStatus.ConnFailed)
            {
                GameEntry.UI.Tip("ConnFailed");
                ChangeState<ProcedureLogin>(this.procedureOwner);
            }
            else if (args.ConnStatus == ConnStatus.TokenExpired)
            {
                GameEntry.UI.Tip("TokenExpired");
                ChangeState<ProcedureLogin>(this.procedureOwner);
            }
        }
    }
}