using GameFramework.Fsm;
using GameFramework.Event;
using GameFramework.Procedure;

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
            uiMainId = GameEntry.UI.OpenUI("Main");
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.UI.CloseUIForm(uiMainId);
            GameEntry.Event.Unsubscribe(Event.OnLogout.EventId, HandleLogOut);
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
    }
}