using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;


namespace NodapParty
{
    public class TextManager : SingletonGameObject<TextManager>
    {
        private Dictionary<int, string> m_DicLocalText = new Dictionary<int, string>();

        public override void DisposeObject()
        {
            m_DicLocalText.Clear();
			
            base.DisposeObject();
        }

        /// <summary>
        /// STRING 파일 로드.
        /// </summary>
        public void LoadJson(string json)
        {            
            var arr = JSON.Parse(json).AsArray;
            int len = arr.Count;
            for (int i = 0; i < len; i++)
            {
                var node = arr[i];
				var script = node.Remove("SCRIPT");
				if (script == null)
					continue;
                var str = script.Remove("KOREAN");
                if (str == null)
					continue;

                var data = JsonUtility.FromJson<StringData>(node.ToString());
				data.STRING = str?.Value;
                if (m_DicLocalText.ContainsKey(data.INDEX))
                {
                    Debug.Log("already key : " + data.INDEX);
                    continue;
                }
                m_DicLocalText.Add(data.INDEX, data.STRING);
            }
            arr = null;
        }

        public void LoadLocalTextFile(string path)
        {			
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(path);

            var textAsset = Resources.Load<TextAsset>(sb.ToString());
            
			LoadJson(textAsset.text);
        }

        public string GetText(int index)
        {
            string text;
            if (m_DicLocalText.TryGetValue(index, out text))
                return text;
            
            return string.Empty;
        }
    }
}