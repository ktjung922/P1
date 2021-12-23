using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NodapParty;

public class PlayManager : SingletonGameObject<PlayManager>
{
    private Camera MainCamera;

    private Camera UICamera;    

    private Camera ParticleCamera;

    private List<CharacterData>                     m_ListOfDeckCharacter = new List<CharacterData>();

    private Dictionary<ElementalData.kTYPE, int>    m_DicOfSynergy = new Dictionary<ElementalData.kTYPE, int>();

    private int kKITAE_THREE_INDEX = 12;

    private Dictionary<ElementalData.kTYPE, ElementalData.kTYPE> m_DicOfGiveSynergy = new Dictionary<ElementalData.kTYPE, ElementalData.kTYPE>()
    {
        { ElementalData.kTYPE.SwimmingPool, ElementalData.kTYPE.Water },
        { ElementalData.kTYPE.Attractive, ElementalData.kTYPE.Love }
    };

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

    ///<summery>
    /// 기태3성 조건 체크용, 덱에 포함된 딜러 수 반환.
    ///</summery>
    public int GetCountOfDPSInDeck()
    {
        var result = PlayManager.Instance.Deck.FindAll(data => (data.TYPE == CharacterData.kTYPE.ATTACK || data.TYPE == CharacterData.kTYPE.MAGIC) && data.INDEX != kKITAE_THREE_INDEX).Count;
        return result;
    }

    public void UpdateSynergy(List<ElementalData> list, bool isDelete, int count = 1)
    {
        bool isTarget = false;
        ElementalData target = new ElementalData();
        list.ForEach(data =>
        {
            UpdateSynergy(data.SYNERGY, isDelete, count);

            if (data.isGiveSynergyType())
            {
                target.SYNERGY = data.SYNERGY;
                isTarget = true;
            }
        });
        
        if (!isTarget)
        {
            CheckGiveSynergy(isDelete);
            return;
        }

        CheckGiveSynergy(isDelete, target);
    }

    public void UpdateSynergy(ElementalData.kTYPE type, bool isDelete, int count = 1)
    {
        if (isDelete)
        {
            if (!m_DicOfSynergy.ContainsKey(type))
                return;
                
            if (m_DicOfSynergy[type] == count)
            {
                m_DicOfSynergy.Remove(type);
            }
            else 
            {
                m_DicOfSynergy[type] -= count;
            }
        }
        else
        {
            if (m_DicOfSynergy.ContainsKey(type))
            {
                m_DicOfSynergy[type] += count;
            }
            else
            {
                m_DicOfSynergy.Add(type, count);
            }
        }
    }

    public void CheckGiveSynergy(bool isDelete, ElementalData target = null)
    {
        var curCount = m_ListOfDeckCharacter.Count;
        var preCount = isDelete ? curCount + 1 : curCount - 1;
        if (target == null)
            target = new ElementalData();

        if (target.isGiveSynergyType())
        {
            var count = isDelete ? preCount : curCount;

            UpdateSynergy(m_DicOfGiveSynergy[target.SYNERGY], isDelete, count);
        }

        if (m_DicOfSynergy.ContainsKey(ElementalData.kTYPE.SwimmingPool) && target.SYNERGY != ElementalData.kTYPE.SwimmingPool)
        {
            UpdateSynergy(ElementalData.kTYPE.Water, true, preCount);
            UpdateSynergy(ElementalData.kTYPE.Water, false, curCount);
        }

        if (m_DicOfSynergy.ContainsKey(ElementalData.kTYPE.Attractive) && target.SYNERGY != ElementalData.kTYPE.Attractive)
        {
            UpdateSynergy(ElementalData.kTYPE.Love, true, preCount);
            UpdateSynergy(ElementalData.kTYPE.Love, false, curCount);
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
