using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodapParty
{
    public sealed class PoolObjects
    {
		// 풀링 대상 오브젝트.
		private Queue<NObject>	m_Queue = new Queue<NObject>();

		// 프리팹 경로. (경로 or 오브젝트가 설정 됨)
		private string          m_PrefabPath;

		// 프리팹 오브젝트. (경로 or 오브젝트가 설정 됨)
		private GameObject		m_PrefabObject;

		// 풀 오브젝트 부모.
		private Transform		m_Parent;


		public PoolObjects(Transform parent, string path, int count)
        {
			m_Parent = parent;
			m_PrefabPath = path;

			LoadResource(path, count);
		}

		public PoolObjects(Transform parent, GameObject prefab, int count)
        {
			m_Parent = parent;
			m_PrefabObject = prefab;

			LoadResource(m_PrefabObject, count);
		}

		#region Load Resources.

        // 요청한 수량만큼 프리팹을 생성.
		private void LoadResource(string path, int count)
        {
			if (string.IsNullOrEmpty(path))
				return;
				
			var prefab = Resources.Load<GameObject>(path);
			LoadResource(prefab, count);
		}

        // 요청한 수량만큼 프리팹을 생성.
		private void LoadResource(GameObject prefab, int count)
        {
			for (int i = 0; i <count; i++)
            {
				var obj = GameObject.Instantiate(prefab, m_Parent) as GameObject;
                var nObject = obj.GetComponent<NObject>();
				nObject.Hide();

				m_Queue.Enqueue(nObject);
			}
			prefab = null;
		}

		#endregion Load Resources.

		#region Push.

        /// <summary>
        /// 사용 완료 된 오브젝트 반환.
        /// </summary>
		public void Push(NObject obj)
        {
			obj.Hide();
			obj.CacheTransform.SetParent(m_Parent);
			
			m_Queue.Enqueue(obj);
		}

		#endregion Push.
		
		#region Pop.

        /// <summary>
        /// Generic 타입의 오브젝트 반환.
        /// </summary>
		public T Pop<T>(Transform parent) where T : NObject
        {
			if (m_Queue.Count == 0)
            {
				if (m_PrefabObject == null)
				{
					LoadResource(m_PrefabPath, 1);
				}
				else
				{
					LoadResource(m_PrefabObject, 1);
				}
			}

			var obj = m_Queue.Dequeue();
            var trans = obj.CacheTransform;
            trans.SetParent(parent);
            trans.localPosition = Vector3.zero;
            trans.localScale    = Vector3.one;
            trans.localRotation = Quaternion.Euler(Vector3.zero);

			return obj as T;
		}

        /// <summary>
        /// Generic 타입의 오브젝트 반환.
        /// </summary>
		public NObject Pop(Transform parent)
        {
			if (m_Queue.Count == 0)
            {
				if (m_PrefabObject == null)
				{
					LoadResource(m_PrefabPath, 1);
				}
				else
				{
					LoadResource(m_PrefabObject, 1);
				}
			}

			var obj = m_Queue.Dequeue();
            var trans = obj.CacheTransform;
            trans.SetParent(parent);
            trans.localPosition = Vector3.zero;
            trans.localScale    = Vector3.one;
            trans.localRotation = Quaternion.Euler(Vector3.zero);
			return obj;
		}

		#endregion Pop.

		// 모든 오브젝트 제거.
		public void Clear()
        {
			while (m_Queue.Count != 0)
			{
				var obj = m_Queue.Dequeue();
				GameObject.Destroy(obj.CacheGameObject);
			}
		}
	}
}
