
using UnityEngine;

namespace NodapParty
{
    public abstract class SingletonGameObject<T> : NObject where T : Component
    {
        private static T m_Instance;

        public static T Instance
        {
			get
            {
				if (object.ReferenceEquals(m_Instance, null))
                {
					m_Instance = GameObject.FindObjectOfType<T>();
					if (object.ReferenceEquals(m_Instance, null))
					{
						GameObject obj = new GameObject(typeof(T).ToString());
						m_Instance = obj.AddComponent<T>();

						if (Application.isPlaying)
						{
							DontDestroyOnLoad(obj);
						}
					}
				}
				return m_Instance;
			}
		}

		public override void DisposeObject()
		{
			m_Instance = null;
			DestroyImmediate(CacheGameObject);
		}
	}
}

