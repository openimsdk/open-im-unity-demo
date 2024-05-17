//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using SuperScrollView;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Dawn
{
    public abstract class UGuiForm : UIFormLogic
    {
        public const int DepthFactor = 100;
        private static Font s_MainFont = null;
        private Canvas m_CachedCanvas = null;
        // private CanvasGroup m_CanvasGroup = null;
        private List<Canvas> m_CachedCanvasContainer = new List<Canvas>();

        public int OriginalDepth
        {
            get;
            private set;
        }

        public int Depth
        {
            get
            {
                return m_CachedCanvas.sortingOrder;
            }
        }

        public static void SetMainFont(Font mainFont)
        {
            if (mainFont == null)
            {
                Log.Error("Main font is invalid.");
                return;
            }

            s_MainFont = mainFont;
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);
            m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            m_CachedCanvas.overrideSorting = true;
            OriginalDepth = m_CachedCanvas.sortingOrder;



            gameObject.GetOrAddComponent<GraphicRaycaster>();

            // Text[] texts = GetComponentsInChildren<Text>(true);
            // for (int i = 0; i < texts.Length; i++)
            // {
            //     texts[i].font = s_MainFont;
            //     if (!string.IsNullOrEmpty(texts[i].text))
            //     {
            //         texts[i].text = GameEntry.Localization.GetString(texts[i].text);
            //     }
            // }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnRecycle()
#else
        protected internal override void OnRecycle()
#endif
        {
            base.OnRecycle();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            var transform = CachedTransform as RectTransform;
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;
            transform.localPosition = Vector3.zero;
            transform.pivot = new Vector2(0.5f, 0.5f);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            base.OnClose(isShutdown, userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnPause()
#else
        protected internal override void OnPause()
#endif
        {
            base.OnPause();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnResume()
#else
        protected internal override void OnResume()
#endif
        {
            base.OnResume();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnCover()
#else
        protected internal override void OnCover()
#endif
        {
            base.OnCover();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnReveal()
#else
        protected internal override void OnReveal()
#endif
        {
            base.OnReveal();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnRefocus(object userData)
#else
        protected internal override void OnRefocus(object userData)
#endif
        {
            base.OnRefocus(userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
#else
        protected internal override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
#endif
        {
            int oldDepth = Depth;
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            int deltaDepth = UGuiGroupHelper.DepthFactor * uiGroupDepth + DepthFactor * depthInUIGroup - oldDepth + OriginalDepth;
            GetComponentsInChildren(true, m_CachedCanvasContainer);
            for (int i = 0; i < m_CachedCanvasContainer.Count; i++)
            {
                m_CachedCanvasContainer[i].sortingOrder += deltaDepth;
            }

            m_CachedCanvasContainer.Clear();
        }

        public void OnClick(Button btn, UnityAction cb)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(cb);
        }

        public Component GetControl(Type t, string path, Transform parent)
        {
            if (parent == null)
            {
                parent = transform;
            }
            var child = parent.Find(path);
            if (child == null)
            {
                Debug.LogError("not find child:" + parent.name + ":" + path);
                return null;
            }
            else
            {
                var com = child.GetComponent(t);
                if (com == null)
                {
                    Debug.LogError("not find child component:" + parent.name + ":" + path + ":" + t.FullName);
                }
                return com;
            }
        }

        public Image GetImage(string path, RectTransform parent = null)
        {
            return GetControl(typeof(Image), path, parent) as Image;
        }
        public Button GetButton(string path, RectTransform parent = null)
        {
            return GetControl(typeof(Button), path, parent) as Button;
        }
        public TextMeshProUGUI GetTextPro(string path, RectTransform parent = null)
        {
            return GetControl(typeof(TextMeshProUGUI), path, parent) as TextMeshProUGUI;
        }
        public TMP_InputField GetInputField(string path, RectTransform parent = null)
        {
            return GetControl(typeof(TMP_InputField), path, parent) as TMP_InputField;
        }
        public LoopListView2 GetListView(string path, RectTransform parent = null)
        {
            return GetControl(typeof(LoopListView2), path, parent) as LoopListView2;
        }
        public LoopGridView GetGridView(string path, RectTransform parent = null)
        {
            return GetControl(typeof(LoopGridView), path, parent) as LoopGridView;
        }
        public Toggle GetToggle(string path, RectTransform parent = null)
        {
            return GetControl(typeof(Toggle), path, parent) as Toggle;
        }
        public RectTransform GetRectTransform(string path, RectTransform parent = null)
        {
            return GetControl(typeof(RectTransform), path, parent) as RectTransform;
        }

        public void RefreshList(LoopListView2 list, int count, bool resetPos = false)
        {
            if (list.ItemTotalCount != count)
            {
                list.SetListItemCount(count);
            }
            else
            {
                list.RefreshAllShownItem();
            }
            if (resetPos)
            {
                list.MovePanelToItemIndex(0, 0);
            }
        }
        public void RefreshGrid(LoopGridView grid, int count, bool resetPos = false)
        {
            if (grid.ItemTotalCount != count)
            {
                grid.SetListItemCount(count);
            }
            else
            {
                grid.RefreshAllShownItem();
            }
            if (resetPos)
            {
                grid.MovePanelToItemByIndex(0);
            }
        }

        public void SetImage(Image image, string sprite)
        {
            if (image != null && sprite != "")
            {
                GameEntry.SpriteAltas.SetImageSprite(image, sprite);
            }
        }

        public void CloseSelf()
        {
            GameEntry.UI.CloseUIForm(UIForm.SerialId);
        }
    }
}
