using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;
using SimpleJSON;

public class UtillManager : SingletonGameObject<UtillManager>
{

#region  Image.

    public Sprite GetSprite(string fileName) {
        var path = string.Concat("Texture/", fileName);
        var sprite = LoadResourceSprite(path);
        return sprite;
    }

    public Sprite LoadResourceSprite(string path) {
        var sprite = Resources.Load<Sprite>(path);
        return sprite;
    }

#endregion Image.

#region Json.

    public List<T> ParseToJson<T>(string path) {
        var json = LoadResourceJson(path);
        if (string.IsNullOrEmpty(json))
            return new List<T>();
        
        return ParseToJsonData<T>(json);
    }

    public string LoadResourceJson(string path) {
        var asset = Resources.Load<TextAsset>(path);
        return asset?.text;
    }

    public List<T> ParseToJsonData<T>(string json) {
        var newList = new List<T>();
        var jsonArr = JSON.Parse(json).AsArray;
        int len = jsonArr.Count;
        for (int i = 0; i < len ; i++) {
            var value = jsonArr[i].AsObject.ToString();
            var data = JsonUtility.FromJson<T>(value);
            newList.Add(data);
        }
        return newList;
    }

#endregion Json.

#region Random.

    public int GetRandomInt(int totalCount)
    {
        return Mathf.RoundToInt(totalCount * Random.Range(0.0f, 1.0f));
    }

#endregion Random.


}
