using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;

public class GachaManager : SingletonGameObject<GachaManager>
{
    private int m_nTotalOfCharacterWeghit;

    public void LoadDefaultGachaData()
    {
        LoadCharacterData();
    }

    private void LoadCharacterData()
    {
        m_nTotalOfCharacterWeghit = 0;

        TableManager.Instance.GetCharacterData().ForEach(data =>
        {
            m_nTotalOfCharacterWeghit += data.WEIGHT;
        });
    }

    public List<CharacterData> GetRandomCharacterData10()
    {
        var newList = new List<CharacterData>();
        for (int i = 0; i < 10; i++) {
            if (i == 9) {
                newList.Add(TableManager.Instance.GetConvertCharacterOneToTwo(GetRandomCharacterData()));
            }
            else {
                newList.Add(GetRandomCharacterData());
            }
        }

        return newList;
    }

    public CharacterData GetRandomCharacterData()
    {
        var randomCount = UtillManager.Instance.GetRandomInt(m_nTotalOfCharacterWeghit);
        
        var characterData = TableManager.Instance.GetCharacterData();
        if (characterData == null || characterData.Count == 0)
            return null;

        foreach (var character in characterData) {
            if (randomCount <= character.WEIGHT)
                return character;
            
            randomCount -= character.WEIGHT;
        }

        return characterData[-1];
    }

    public bool CheckedRandomRange(float f)
    {
        var trunFloat = Mathf.Floor(f * 1000) * 0.1f;
        int random = Random.Range(0, 1001);
        float fRandom = random * 0.1f;

        if(trunFloat >= fRandom)
            return true;
        else
            return false;
    }
}
