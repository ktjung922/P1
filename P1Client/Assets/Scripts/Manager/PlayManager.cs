using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;

public class PlayManager : SingletonGameObject<PlayManager>
{
    private Camera MainCamera;

    private Camera UICamera;    

    private Camera ParticleCamera;

    private List<CharacterData>                     m_ListOfDeckCharacter = new List<CharacterData>();

    private Dictionary<ElementalData.kTYPE, int>    m_DicOfSynergy = new Dictionary<ElementalData.kTYPE, int>();

    public void Initialization()
    {
        MainCamera = GameObject.FindWithTag("MainCamera")?.GetComponent<Camera>();
        UICamera = GameObject.FindWithTag("UICamera")?.GetComponent<Camera>();
        ParticleCamera = GameObject.FindWithTag("ParticleCamera")?.GetComponent<Camera>();
    }

    public void ClearDeckData()
    {
        m_ListOfDeckCharacter.Clear();
        m_DicOfSynergy.Clear();
    }

    public void Test()
    {
        m_ListOfDeckCharacter.ForEach(data => Debug.Log(data.INDEX));

        foreach (var item in m_DicOfSynergy)
        {
            Debug.Log(item.Key + " / " + item.Value);
        }
    }

    public List<CharacterData> Deck
    {
        get { return m_ListOfDeckCharacter; }
        set { m_ListOfDeckCharacter = value; }
    }

    public Dictionary<ElementalData.kTYPE, int> Synergy
    {
        get { return m_DicOfSynergy; }
        set { m_DicOfSynergy = value; }
    }
}
