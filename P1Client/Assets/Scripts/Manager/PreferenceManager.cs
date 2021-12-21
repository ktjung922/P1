
using System;
using UnityEngine;
using SimpleJSON;


namespace NodapParty
{
    public sealed class PreferenceManager : SingletonObject<PreferenceManager>
    {
        /// <summary>
        /// 해당 타입의 데이터 삽입.
        /// </summary>
        public void SaveString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        /// <summary>
        /// 해당 타입의 데이터 삽입.
        /// </summary>
        public void SaveString(Enum key, string value)
        {
            SaveString(key.ToString(), value);
        }

        /// <summary>
        /// 해당 타입의 데이터 반환.
        /// </summary>
        public string LoadString(string key, string defaultValue)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        /// <summary>
        /// 해당 타입의 데이터 반환.
        /// </summary>
        public string LoadString(Enum key, string defaultValue)
        {
            return LoadString(key.ToString(), defaultValue);
        }

        /// <summary>
        /// 해당 타입의 데이터 삽입.
        /// </summary>
        public void SaveInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        /// <summary>
        /// 해당 타입의 데이터 삽입.
        /// </summary>
        public void SaveInt(Enum key, int value)
        {
            SaveInt(key.ToString(), value);
        }

        /// <summary>
        /// 해당 타입의 데이터 반환.
        /// </summary>
        public int LoadInt(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        /// <summary>
        /// 해당 타입의 데이터 반환.
        /// </summary>
        public int LoadInt(Enum key, int defaultValue)
        {
            return LoadInt(key.ToString(), defaultValue);
        }

        /// <summary>
        /// 해당 Key의 데이터 존재여부 반환.
        /// </summary>
        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        /// <summary>
        /// 해당 Key의 데이터 존재여부 반환.
        /// </summary>
        public bool HasKey(Enum key)
        {
            return HasKey(key.ToString());
        }

        /// <summary>
        /// Serializable가능한 Class 데이터 저장.
        /// </summary>
        public void Save(Enum key, string json, EncryptManager.kTYPE encryptType = EncryptManager.kTYPE.None)
        {
            if (encryptType == EncryptManager.kTYPE.Base64)
            {
                json = EncryptManager.Instance.EncryptBase64(json);
            }

            PlayerPrefs.SetString(key.ToString(), json);
        }

        /// <summary>
        /// Serializable가능한 Class 데이터 저장.
        /// </summary>
        public void Save(string key, string json, EncryptManager.kTYPE encryptType = EncryptManager.kTYPE.None)
        {
            if (encryptType == EncryptManager.kTYPE.Base64)
            {
                json = EncryptManager.Instance.EncryptBase64(json);
            }

            PlayerPrefs.SetString(key, json);
        }

        /// <summary>
        /// Serializable가능한 Class 데이터 저장.
        /// </summary>
        public void Save<T>(T t, EncryptManager.kTYPE encryptType = EncryptManager.kTYPE.None) where T : class
        {
            string json = JsonUtility.ToJson(t);
            if (encryptType == EncryptManager.kTYPE.Base64)
            {
                json = EncryptManager.Instance.EncryptBase64(json);
            }

            Type type = typeof(T);
            PlayerPrefs.SetString(type.Name, json);
        }

        /// <summary>
        /// Serializable가능한 Class 데이터 로드.
        /// </summary>
        public string Load(Enum key, EncryptManager.kTYPE encryptType = EncryptManager.kTYPE.None)
        {
            string json = PlayerPrefs.GetString(key.ToString());
            if (string.IsNullOrEmpty(json))
            {
				json = new JSONObject().ToString();
			}
			else if (encryptType == EncryptManager.kTYPE.Base64)
			{
				json = EncryptManager.Instance.DecryptBase64(json);
			}
			return json;
        }

        /// <summary>
        /// Serializable가능한 Class 데이터 로드.
        /// </summary>
        public string Load(string key, EncryptManager.kTYPE encryptType = EncryptManager.kTYPE.None)
        {
            string json = PlayerPrefs.GetString(key);
            if (string.IsNullOrEmpty(json))
            {
				json = new JSONObject().ToString();
			}
			else if (encryptType == EncryptManager.kTYPE.Base64)
			{
				json = EncryptManager.Instance.DecryptBase64(json);
			}
			return json;
        }

        /// <summary>
        /// Serializable가능한 Class 데이터 로드.
        /// </summary>
        public T Load<T>(EncryptManager.kTYPE encryptType = EncryptManager.kTYPE.None) where T : class, new()
        {
            Type type = typeof(T);
            string json = PlayerPrefs.GetString(type.Name);
            if (string.IsNullOrEmpty(json))
            {
                T t = new T();
                json = JsonUtility.ToJson(t);
                t = null;
            }

            if (encryptType == EncryptManager.kTYPE.Base64)
            {
                json = EncryptManager.Instance.DecryptBase64(json);
            }
            
            return JsonUtility.FromJson<T>(json);
        }

        /// <summary>
        /// 해당 데이터 존재여부 반환.
        /// </summary>
        public bool HasKey<T>()
        {
            Type type = typeof(T);
            return PlayerPrefs.HasKey(type.Name.ToString());
        }
    }
}