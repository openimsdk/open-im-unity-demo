using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Dawn
{
    public class SwipeButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public class SwipeEvent : UnityEvent<float, float> { }
        SwipeEvent m_OnSwipe = new SwipeEvent();
        public SwipeEvent OnSwipe
        {
            get { return m_OnSwipe; }
            set { m_OnSwipe = value; }
        }
        UnityEvent m_OnClick = new UnityEvent();
        public UnityEvent OnClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }
        Vector3 downPosition;
        Vector2 startDragPosition;
        public void OnBeginDrag(PointerEventData eventData)
        {
            startDragPosition = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var delta = eventData.position - startDragPosition;
            OnSwipe.Invoke(delta.x, delta.y);
        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            downPosition = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (Vector2.Distance(downPosition, eventData.position) < 5)
            {
                OnClick.Invoke();
            }
        }
    }

}

