using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;

public class TableManager : SingletonGameObject<TableManager>
{
    private List<CharacterData> m_ListOfCharacterData;
    private List<SynergyData>   m_ListOfSynergyData;

    public void LoadDefaultResources() {
        m_ListOfCharacterData = UtillManager.Instance.ParseToJson<CharacterData>("Scripts/CHARACTER");
        m_ListOfSynergyData = UtillManager.Instance.ParseToJson<SynergyData>("Scripts/SYNERGY");
    }

#region  CharacterData.

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

#endregion  CharacterData.

#region  SynergyData.

    public SynergyData GetSynergyDataWithIndex(int index)
    {
        return m_ListOfSynergyData.Find(foundData => foundData.INDEX == index);
    }

    public int GetCountOfSynergyActive(int synergyIndex, int count)
    {
        int result = -1;
        var synergy = m_ListOfSynergyData.Find(foundData => foundData.INDEX == synergyIndex);
        if (synergy == null)
            return result;
        
        for (int i = 0; i < synergy.IF.Count ; i++)
        {
            if (synergy.IF[i].COUNT_TYPE == SynergyIFData.kTYPE.IF)
            {
                if (synergy.IF[i].COUNT <= count)
                {
                    result = i;
                }
                else
                {
                    return result;
                }
            }
            else if (synergy.IF[i].COUNT_TYPE == SynergyIFData.kTYPE.IF_NOT)
            {
                if (synergy.IF[i].COUNT >= count)
                {
                    result = i;
                }
                else
                {
                    return result;
                }
            }
        }

        return result;
    }

#endregion SynergyData.
}
