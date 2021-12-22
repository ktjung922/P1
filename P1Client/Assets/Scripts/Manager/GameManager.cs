
using System;
using UnityEngine;
using System.Collections.Generic;

namespace NodapParty
{
    public class GameManager : SingletonGameObject<GameManager>
    {
        // 최상단 Layer 변경 메서드.
        public delegate void OnChangeTopLayerHandler(NLayer layer);
        /// <summary>
        /// 최상단 Layer 변경시 이벤트.
        /// </summary>
        public static event OnChangeTopLayerHandler         OnChangeTopLayer;
        /// <summary>
        /// 공용 팝업 뒤로가기 이벤트 큐.
        /// </summary>
		private Queue<NLayer>								m_EscapePopupQueue = new Queue<NLayer>();
        // 현재 최상단 Layer.
        private NLayer                                      m_TopLayer;
        // 오브젝트 관리 딕셔너리.
        private Dictionary<Type, NLayer>                    m_Dic = new Dictionary<Type, NLayer>();
        // 런타임 시간 값 (초 단위, 서버시간 + 해당 값으로 현재 시간 획득 용도).
        private float                                       m_Tick;
        // 앱 실행 후 플레이 타임.
        private float                                       m_PlayTime;
        // 앱 Pause 상태 발생시 시간 값.
        private float                                       m_PauseTime;
        // 터치 이펙트 타입.
        private Type                                        m_TouchParticleType;
        // 화면 유형
        private ScreenOrientation                           m_ScreenOrientation;        
        // 화면 최대 비율 (720 * 1560).
        private float                                       kMAXRate = 1.21875f;


    #region Override.

        /// <summary>
        /// 오브젝트 해제.
        /// </summary>
        public override void DisposeObject()
        {
            OnChangeTopLayer = null;
        }

    #endregion Override.

        /// <summary>
        /// 초기화.
        /// </summary>
        public void Initialization(float width, float height, ScreenOrientation orientation = ScreenOrientation.Portrait)
        {
            Application.targetFrameRate = 60;

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        #if UNITY_EDITOR
            Application.runInBackground = true;
        #endif
            InitDefaultScreenSize(width, height, orientation);
        }
        
        // 기본 해상도 지정.
        private void InitDefaultScreenSize(float width, float height, ScreenOrientation orientation)
        {
            m_ScreenOrientation = orientation;
            DeviceScreenSize = new Vector2(Screen.width, Screen.height);
            GameScreenSize = new Vector2(width, height);
            float wRatio = (float)Screen.width / width;
            float hRatio = (float)Screen.height / height;

            if (m_ScreenOrientation == ScreenOrientation.Portrait)
            {
                if (wRatio < hRatio)
                {
                    hRatio = hRatio / wRatio;
                    wRatio = 1.0f;
					
                    // 화면 비율 확인 후 재설정.
                    hRatio = (hRatio > kMAXRate) ? kMAXRate : hRatio;

                    var size = GameScreenSize.y * (hRatio - 1.0f); 
					size = size * 0.5f;

					FixPixel = new Vector2(0.0f, size);
                }

                else if (wRatio > hRatio)
                {
                    wRatio = wRatio / hRatio;
                    hRatio = 1.0f;
                }
                else
                {
                    wRatio = hRatio = 1.0f;
                }
            }
            else
            {
                if (wRatio > hRatio)
                {   
                    wRatio = wRatio / hRatio;
                    hRatio = 1.0f;

                    // 화면 비율 확인 후 재설정.
                    wRatio = (wRatio > kMAXRate) ? kMAXRate : wRatio;
                    
                    var size = GameScreenSize.x * (wRatio - 1.0f); 
                    size = size * 0.5f;

                    FixPixel = new Vector2(size, 0.0f);                
                }
                else
                {
                    wRatio = hRatio = 1.0f;
                }
            }

            Ratio = new Vector2(wRatio, hRatio);
        }

        /// <summary>
        /// 해당 Key의 오브젝트 추가.
        /// SLayer.Initialization 호출.
        /// </summary>
        public void Push<T>(string prefabPath, Action disposeCallback = null, Transform parent = null) where T : NLayer
        {
            var key = typeof(T);
            if (m_Dic.ContainsKey(key))
            {
                return;
            }
            
            var prefab = Resources.Load(prefabPath);
            var obj = Instantiate(prefab, parent) as GameObject;
            if (obj == null)
            {
                return;
            }

            var layer = obj.GetComponent<NLayer>();
            if (layer == null)
            {
                return;
            }

            m_Dic.Add(key, layer);

            layer.Initialization();
            layer.SetDisposeCallback(disposeCallback);

            CheckTopLayer();
        }
        
        /// <summary>
        /// 해당 Key의 오브젝트 추가.
        /// SLayer.Initialization 호출.
        /// </summary>
        public void Push<T>(Action<T> callback, string prefabPath) where T : NLayer
        {
            var key = typeof(T);
            if (m_Dic.ContainsKey(key))
            {
                return;
            }
            
            var prefab = Resources.Load(prefabPath);
            var obj = Instantiate(prefab) as GameObject;
            if (obj == null)
            {
                return;
            }

            var layer = obj.GetComponent<NLayer>();
            if (layer == null)
            {
                return;
            }

            m_Dic.Add(key, layer);

            layer.Initialization();

            callback(obj.GetComponent<T>());
            
            CheckTopLayer();
        }
                
        /// <summary>
        /// 해당 오브젝트 제거.
        /// SLayer.DisposeObject 호출.
        /// </summary>
        public void Pop(NLayer layer)
        {
			var e = m_Dic.GetEnumerator();
			while (e.MoveNext())
			{
				if (e.Current.Value != layer)
					continue;
					
				Pop(e.Current.Key, layer);
				break;
			}
        }
                
        /// <summary>
        /// 해당 오브젝트 제거.
        /// SLayer.DisposeObject 호출.
        /// </summary>
        public void Pop(Type key)
        {
            if (ReferenceEquals(key, null))
            {
                return;
            }

            NLayer layer;
            if (!m_Dic.TryGetValue(key, out layer))
                return;
					
			Pop(key, layer);
        }
        
        /// <summary>
        /// 해당 Key의 오브젝트 제거.
        /// SLayer.DisposeObject 호출.
        /// </summary>
        public void Pop<T>() where T : NLayer
        {
            var key = typeof(T);
            NLayer layer;
            if (!m_Dic.TryGetValue(key, out layer))
            {
                return;
            }
			Pop(key, layer);
        }

		// SLayer.DisposeObject 호출.
		private void Pop(Type key, NLayer layer)
		{	
            m_Dic.Remove(key);
					
            layer.DisposeObject();
			
            CheckTopLayer();

            Destroy(layer.CacheGameObject);

			Resources.UnloadUnusedAssets();
		}
        
        /// <summary>
        /// 등록된 모든 오브젝트에 통지.
        /// </summary>
        public void NotifyAll(Enum notify)
        {
            using (var e = m_Dic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    e.Current.Value.Notify(notify);
                }
            }
        }
        
        /// <summary>
        /// 해당 Key의 오브젝트에 통지.
        /// </summary>
        public void Notify<T>(Enum notify) where T : NLayer
        {
            var key = typeof(T);
            NLayer layer;
            if (m_Dic.TryGetValue(key, out layer))
            {
                layer.Notify(notify);
            }
        }
        
        /// <summary>
        /// 해당 타입의 Layer에 노티를 하여 결과를 돌려받음.
        /// </summary>
        public bool CheckResult<T>(Enum notify) where T : NLayer
        {
            var key = typeof(T);
            NLayer layer;
            if (m_Dic.TryGetValue(key, out layer))
            {
                return layer.CheckResult(notify);
            }
			return false;
        }

        /// <summary>
        /// 해당 Key의 Layer에 오브젝트 전달.
        /// </summary>
        public void SendObject<T>(Enum notify, NObject obj) where T : NObject
        {
            var key = typeof(T);
            NLayer layer;
            if (m_Dic.TryGetValue(key, out layer))
            {
                layer.ReceiveObject(notify, obj);
            }
        }

        /// <summary>
        /// 해당 Key의 Layer에 오브젝트 전달.
        /// </summary>
        public void SendObjectAndCallBack<T>(Enum notify, NObject obj, Action callback) where T : NObject
        {
            var key = typeof(T);
            NLayer layer;
            if (m_Dic.TryGetValue(key, out layer))
            {
                layer.ReceiveObject(notify, obj, callback);
            }
        }

        /// <summary>
        /// 해당 Key의 오브젝트의 SLayer.Show 호출.
        /// </summary>
        public void Show<T>() where T : NLayer
        {
            var key = typeof(T);
            NLayer layer;
            if (m_Dic.TryGetValue(key, out layer))
            {
                layer.Show();
            }
        }
        
        /// <summary>
        /// 해당 Key의 오브젝트의 SLayer.Hide 호출.
        /// </summary>
        public void Hide<T>() where T : NLayer
        {
            var key = typeof(T);
            NLayer layer;
            if (m_Dic.TryGetValue(key, out layer))
            {
                layer.Hide();
            }
        }

        /// <summary>
        /// 해당 Key의 오브젝트 존재여부 반환.
        /// </summary>
        public bool HasKey<T>() where T : NLayer
        {
            var key = typeof(T);
            return m_Dic.ContainsKey(key);
        }

        /// <summary>
        /// 모든 오브젝트를 제거한다.
        /// </summary>
        public void Clear(Action callback)
        {   
            var e = m_Dic.GetEnumerator();
            while (e.MoveNext())
            {
                var layer = e.Current.Value;
                if (layer == null)
                    continue;

                layer.DisposeObject();

                Destroy(layer.CacheGameObject);
            }
            m_Dic.Clear();
            m_TopLayer = null;

			Resources.UnloadUnusedAssets();
			
            callback();
        }

		/// <summary>
		/// T 타입의 UI의 최상단 Layer여부 반환.
		/// </summary>
		public bool IsTop<T>() where T : NLayer
		{
			NLayer layer = GetTopLayer();
			if (layer == null)
				return false;
			
			Type type = typeof(T);
            return (type.Equals(layer.GetType())) ? true : false;
		}

		/// <summary>
		/// T 타입의 UI의 활성화 여부 반환.
		/// </summary>
		public bool IsActive<T>() where T : NLayer
		{
			Type type = typeof(T);
			if (!m_Dic.ContainsKey(type))
				return false;
				
			return m_Dic[type].CacheGameObject.activeSelf;
		}

        /// <summary>
        /// 최상단 레이어 확인.
        /// </summary>
        public void CheckTopLayer()
        {
            if (OnChangeTopLayer == null)
                return;

            var topLayer = GetTopLayer();
            if (m_TopLayer == topLayer)
                return;

            m_TopLayer = topLayer;
            OnChangeTopLayer(m_TopLayer);
        }

        /// <summary>
        /// SLayer 파괴.
        /// </summary>
        public void DestroySLayer(NLayer layer)
        {
            layer.DisposeObject();
            DestroyImmediate(layer);
        }

        /// <summary>
        /// SObject 반환.
        /// </summary>
        public void DisposeObjectList<T>(List<T> list) where T : NObject
        {
			if (list == null)
				return;
			
			list.ForEach(obj => obj.DisposeObject());
			list.Clear();
        }

		/// <summary>
		/// 팝업 layer 추가.
		/// </summary>
		public void AddPopup(NLayer layer)
		{
			m_EscapePopupQueue.Enqueue(layer);
		}

		/// <summary>
		/// 팝업 layer 삭제.
		/// </summary>
		public void RemovePopup()
		{
			m_EscapePopupQueue.Dequeue();
		}

		/// <summary>
		/// 팝업 활성화 여부 반환.
		/// </summary>
		public bool IsActivePopup()
		{
			return (m_EscapePopupQueue.Count == 0) ? false : true;
		}

        /// <summary>
        /// 기준 해상도 대비 현재 해상도 비율.
        /// </summary>
        public Vector2 Ratio
        {
            get; set;
        }

        /// <summary>
        /// 기준 해상도 대비 현재 해상도 차이값(/2).
        /// </summary>
        public Vector2 FixPixel
        {
            get; set;
        }

        /// <summary>
        /// 단말기 해상도 크기 반환.
        /// </summary>
        public Vector2 DeviceScreenSize
        {
            get; set;
        }

        /// <summary>
        /// 게임 해상도 크기 반환.
        public Vector2 GameScreenSize
        {
            get; set;
        }

        /// <summary>
        /// 화면 방향 반환.
        /// </summary>
        public ScreenOrientation ScreenOrientation
        {
            get 
            {
                return m_ScreenOrientation;
            }
        }

        /// <summary>
        /// 최상단 Layer 반환.
        /// </summary>
        public NLayer GetTopLayer()
        {
			NLayer layer = null;
            var e = m_Dic.GetEnumerator();
            while (e.MoveNext())
            {
                var tmp = e.Current.Value as NLayer;
				if (!tmp.CacheGameObject.activeSelf)
					continue;

                if (!tmp.IsTopCheckable)
                    continue;

				if (layer == null || layer.Depth < tmp.Depth)
				{
					layer = tmp;
				}
            }
            return layer;
        }

        /// <summary>
        /// 해당 Key의 오브젝트 존재여부와 Generic Component 반환.
        /// </summary>
        public bool TryGetObject<T>(out T t) where T : NLayer
        {
            var key = typeof(T);
            NLayer layer;
            if (m_Dic.TryGetValue(key, out layer))
            {
                t = layer.GetComponent<T>();
                return true;
            }

            t = default(T);
            return false;
        }


        /// <summary>
        /// 해당 Key의 오브젝트 존재여부와 Generic Component 반환.
        /// </summary>
        public T GetObject<T>() where T : NLayer
        {
            var key = typeof(T);
            NLayer layer;
            if (m_Dic.TryGetValue(key, out layer))
            {
                return layer.GetComponent<T>();
            }
            return default(T);
        }  
    }
}
