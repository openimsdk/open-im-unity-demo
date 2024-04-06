using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Dawn
{
    public delegate void UIEventCallBack(PointerEventData eventData);
    public class LuaUIEventCallBack : EventTrigger
    {

        public UIEventCallBack LuaOnPointerEnter;
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if(LuaOnPointerEnter!= null){
                LuaOnPointerEnter(eventData);
            }
        }
        public UIEventCallBack LuaOnPointerClick;
        public override void OnPointerClick(PointerEventData eventData)
        {
            if(LuaOnPointerClick!= null){
                LuaOnPointerClick(eventData);
            }
        }

        public UIEventCallBack LuaOnPointerExit;
        public override void OnPointerExit(PointerEventData eventData)
        {
            if(LuaOnPointerExit!= null){
                LuaOnPointerExit(eventData);
            }
        }
        public UIEventCallBack LuaOnPointerDown;
        public override void OnPointerDown(PointerEventData eventData)
        {
            if(LuaOnPointerDown != null){
                LuaOnPointerDown(eventData);
            }
        }
        public UIEventCallBack LuaOnPointerUp;
        public override void OnPointerUp(PointerEventData eventData)
        {
            if(LuaOnPointerUp != null){
                LuaOnPointerUp(eventData);
            }
        }

        public UIEventCallBack LuaOnBeginDrag;
        public override void OnBeginDrag(PointerEventData eventData)
        {
            if(LuaOnBeginDrag != null){
                LuaOnBeginDrag(eventData);
            }
        }
        public UIEventCallBack LuaOnDrag;
        public override void OnDrag(PointerEventData eventData)
        {
            if(LuaOnDrag != null){
                LuaOnDrag(eventData);
            }
        }
        public UIEventCallBack LuaOnEndDrag;
        public override void OnEndDrag(PointerEventData eventData)
        {
            if (LuaOnEndDrag != null){
                LuaOnEndDrag(eventData);
            }
        }

        public void Clear(){
            LuaOnPointerEnter = null;
            LuaOnPointerClick = null;
            LuaOnPointerExit = null;
            LuaOnPointerDown = null;
            LuaOnPointerUp = null;
            LuaOnBeginDrag = null;
            LuaOnDrag = null;
            LuaOnEndDrag = null;
        }
    }
}

