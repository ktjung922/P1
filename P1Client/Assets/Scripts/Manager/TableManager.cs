using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;

public class TableManager : SingletonGameObject<TableManager>
{
    private List<CharacterData> m_ListOfCharacterData;

    public void LoadDefaultResources() {
        m_ListOfCharacterData = UtillManager.Instance.ParseToJson<CharacterData>("Scripts/CHARACTER");
    }

    public List<CharacterData> GetCharacterData()
    {
        return m_ListOfCharacterData;
    }

    public CharacterData GetCharacterDataWithIndex(int index)
    {
        return m_ListOfCharacterData.Find(foundData => foundData.INDEX == index);
    }

    public CharacterData GetConvertCharacterOneToTwo(CharacterData characterData)
    {
        if (characterData.GRADE == CharacterData.kGRADE.One)
            return m_ListOfCharacterData.Find(foundData => foundData.GROUP == characterData.GROUP && foundData.GRADE == CharacterData.kGRADE.Two);
        else
            return characterData;
    }
}
