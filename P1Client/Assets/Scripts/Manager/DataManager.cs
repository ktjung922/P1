using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;
using System.Linq;

public class DataManager : SingletonGameObject<DataManager>
{
    private UserData        m_UserData;

    public UserData         UserData
    {
        get
        {
            return m_UserData;
        }
        set
        {
            m_UserData = value;
            SaveUserData();
        }
    }

    public void SaveUserData()
    {
        PreferenceManager.Instance.Save<UserData>(m_UserData);
    }

    public void LoadUserData()
    {
        m_UserData = PreferenceManager.Instance.Load<UserData>();
    }

    public void AddCardData(List<CharacterData> list)
    {
        list.ForEach(data => 
        {
            if (HasCardDataWithIndex(data.INDEX))
            {
                m_UserData.m_CoinQTY += 10;
                return;
            }

            m_UserData.m_CharacterData.Add(data.INDEX);
        });
        SaveUserData();
    }

    public bool AddCardData(CharacterData data)
    {
        if (HasCardDataWithIndex(data.INDEX))
        {
            m_UserData.m_CoinQTY += 10;
            return true;
        }

        m_UserData.m_CharacterData.Add(data.INDEX);
        SaveUserData();
        return false;
    }

    public bool HasCardDataWithIndex(int Index)
    {
        return m_UserData.m_CharacterData.Any(data => data == Index);
    }

    public List<int> HasAllCharacterCardIndex()
    {
        return m_UserData.m_CharacterData;
    }

    public void ResetUserData()
    {
        m_UserData = null;
        PlayerPrefs.DeleteAll();
        LoadUserData();
    }
}
