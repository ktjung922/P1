
using UnityEngine;

namespace NodapParty
{
    public abstract class NObject : MonoBehaviour
    {
        private GameObject  m_CacheGameObject;

        private Transform   m_CacheTransform;

        public GameObject CacheGameObject {
            get {
                if (object.ReferenceEquals(m_CacheGameObject, null)) {
                    m_CacheGameObject = gameObject;
                }

                return m_CacheGameObject;
            }
        }

        public Transform CacheTransform {
            get {
                if (object.ReferenceEquals(m_CacheTransform, null)) {
                    m_CacheTransform = transform;
                }

                return m_CacheTransform;
            }
        }
        
        public abstract void DisposeObject();

        public virtual void Show() {
            CacheGameObject.SetActive(true);
        }

        public virtual void Hide() {
            CacheGameObject.SetActive(false);
        }
    }
}


