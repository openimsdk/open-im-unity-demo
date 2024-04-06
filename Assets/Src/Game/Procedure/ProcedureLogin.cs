﻿using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Procedure;
using Dawn.Game.Event;
using GameFramework.Event;
using System;
namespace Dawn.Game
{
    public class ProcedureLogin : ProcedureBase
    {
        IFsm<IProcedureManager> procedureOwner;

        int UILoginId;
        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
            this.procedureOwner = procedureOwner;
        }
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            UILoginId = GameEntry.UI.OpenUI("Login");
            GameEntry.Event.Subscribe(OnLoginStatusChange.EventId, HandlerLoginStatusChange);
        }

        void HandlerLoginStatusChange(object sender, GameEventArgs eventArgs)
        {
            OnLoginStatusChange args = eventArgs as OnLoginStatusChange;
            if (args.UserStatus == UserStatus.LoginSuc)
            {
                ChangeState<ProcedureMain>(procedureOwner);
            }
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.UI.CloseUIForm(UILoginId);

            GameEntry.Event.Unsubscribe(OnLoginStatusChange.EventId, HandlerLoginStatusChange);
        }
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

        }
        protected override void OnDestroy(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }
    }
}