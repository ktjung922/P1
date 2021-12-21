
using System;
using UnityEngine;
using UnityEngine.UI;

namespace NodapParty
{
    [RequireComponent(typeof(Canvas)), RequireComponent(typeof(CanvasScaler)), RequireComponent (typeof(GraphicRaycaster))]
    public class NLayer : NObject
    {
        private Canvas m_Canvas;

        protected Action m_ActionOfDispose;

        public int Depth {
            get; set;
        }

        public bool IsEscapeEvent {
            get; set;
        }

        public bool IsTopCheckable {
            get; set;
        }

        public virtual void OnEscapeEvent() {

        }

        public virtual void Notify(Enum value) {

        }
        
        public virtual bool CheckResult(Enum value)
        {
			return false;
		}

        public virtual void ReceiveObject(Enum value, NObject obj, Action callback = null) {

        }

        public virtual void Initialization () {
            IsEscapeEvent = true;
            IsTopCheckable = true;

            UpdateDepth();
        }

        public override void DisposeObject() {    
            m_ActionOfDispose?.Invoke();
            m_ActionOfDispose = null;
        }

        public override void Show() {
            base.Show();
            GameManager.Instance.CheckTopLayer();
        }

        public override void Hide() {
            base.Hide();
            GameManager.Instance.CheckTopLayer();
        }

        protected void UpdateDepth() {
            m_Canvas = CacheGameObject.GetComponent<Canvas>();
            if (m_Canvas != null)
            {
                m_Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                Depth = m_Canvas.sortingOrder;
            }
        }

        /// <summary>
        /// 오브젝트 반환시 콜백 지정.
        /// </summary>
        public void SetDisposeCallback(Action callback) {
            m_ActionOfDispose = callback;
        }
    }
}
