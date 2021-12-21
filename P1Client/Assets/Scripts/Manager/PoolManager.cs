using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace NodapParty
{
    public class PoolManager : SingletonGameObject<PoolManager>
    {
        private Dictionary<Type, PoolObjects>   m_DicOfObject = new Dictionary<Type, PoolObjects>();

        public override void DisposeObject()
        {
            Clear();

            base.DisposeObject();
        }

        public void Create<T>(string prefabPath, int count) where T : NObject
        {
            Type type = typeof(T);
			if (m_DicOfObject.ContainsKey(type))
				return;

			var poolObjects = new PoolObjects(CacheTransform, prefabPath, count);
			m_DicOfObject.Add(type, poolObjects);
		}

        public void Create<T>(GameObject prefab, int count) where T : NObject
        {
            Type type = typeof(T);
			if (m_DicOfObject.ContainsKey(type))
				return;

			var poolObjects = new PoolObjects(CacheTransform, prefab, count);
			m_DicOfObject.Add(type, poolObjects);
		}

        public void Push<T>(params NObject[] objs) where T : NObject
        {
            Type type = typeof(T);
			PoolObjects poolObjects;
			if (!m_DicOfObject.TryGetValue(type, out poolObjects))
            {
				return;
			}

			int len = objs.Length;
			for (int i = 0; i < len; i++)
            {
				poolObjects.Push(objs[i]);
			}
		}

        public NObject Pop(Type type, Transform parent = null)
        {
			PoolObjects poolObjects;
			if (!m_DicOfObject.TryGetValue(type, out poolObjects))
            {
				return null;
			}

			return poolObjects.Pop<NObject>(parent);
		}
        
        public T Pop<T>() where T : NObject
        {
            Type type = typeof(T);
			PoolObjects poolObjects;
			if (!m_DicOfObject.TryGetValue(type, out poolObjects))
            {
				return default(T);
			}

			return poolObjects.Pop<T>(null);
		}

		public T Pop<T>(Transform parent) where T : NObject
        {
            Type type = typeof(T);
			PoolObjects poolObjects;
			if (!m_DicOfObject.TryGetValue(type, out poolObjects))
            {
				return default(T);
			}

			return poolObjects.Pop<T>(parent);
		}
        
		public bool HasKey<T>() where T : NObject
        {
            Type type = typeof(T);
			return m_DicOfObject.ContainsKey(type);
		}

        public void Clear<T>() where T : NObject
        {
            Type type = typeof(T);
			PoolObjects poolObjects;
			if (!m_DicOfObject.TryGetValue(type, out poolObjects))
            {
				return;
			}

			poolObjects.Clear();
			poolObjects = null;

			m_DicOfObject.Remove(type);
		}

        public void Clear()
        {
            var e = m_DicOfObject.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.Value.Clear();
            }
            m_DicOfObject.Clear();
        }
    }
}

