using System.Collections.Generic;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using GameFramework.FileSystem;
using GameFramework.Resource;
namespace Dawn
{

    public class ProcedurePreload : ProcedureBase
    {
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
            GameEntry.BuiltinData.SetLoadingProgress(0.9f, "Loading Code...");
            GameEntry.SpriteAltas.LoadSpriteAtlas();
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.BuiltinData.ClearLoadingForm();
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!GameEntry.SpriteAltas.IsLoadSpriteAltasDone())
            {
                return;
            }
            ChangeState<Game.ProcedureGame>(procedureOwner);
        }
    }
}