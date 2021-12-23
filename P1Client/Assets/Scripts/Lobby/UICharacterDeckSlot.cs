using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodapParty;

public class UICharacterDeckSlot : NObject
{
    [SerializeField]
    private Image           m_ImageOfCharacter;

    [SerializeField]
    private GameObject[]    m_ObjectsOfStar;

    [SerializeField]
    private Transform       m_TransOfType;

    [SerializeField]
    private Transform       m_TransOfElemental;

    private CharacterData   m_CharacterData;

    private List<UITypeSlot>    m_ListOfTypeSlot = new List<UITypeSlot>();

    public override void DisposeObject()
    {
        GameManager.Instance.DisposeObjectList(m_ListOfTypeSlot);
        PoolManager.Instance.Push<UICharacterDeckSlot>(this);
    }

    public void UpdateUI(CharacterData data)
    {
        m_CharacterData = data;

        m_ImageOfCharacter.sprite = UtillManager.Instance.GetSprite(m_CharacterData.CARD_IMG);
        UpdateStar();
        UpdateType();
        UpdateElemental();

        base.Show();
    }

    private void UpdateStar()
    {
        for (int i = 0; i < m_ObjectsOfStar.Length; i++)
        {
            m_ObjectsOfStar[i].SetActive(i < (int)m_CharacterData.GRADE);
        }
    }
    
    private void UpdateType()
    {
        var obj = PoolManager.Instance.Pop<UITypeSlot>(m_TransOfType);
        obj.UpdateUI((int)m_CharacterData.TYPE);
        m_ListOfTypeSlot.Add(obj);
    }

    private void UpdateElemental()
    {
        m_CharacterData.ELEMENTAL.ForEach(data => 
        {
            var obj = PoolManager.Instance.Pop<UITypeSlot>(m_TransOfElemental);
            obj.UpdateUI((int)data.SYNERGY);
            m_ListOfTypeSlot.Add(obj);
        });        
    }

    public void OnTouch()
    {
        GameManager.Instance.SendObject<UIDeck>(UIDeck.kNOTFY.RemoveDeckCharacter, this);
    }

    public CharacterData CharacterData
    {
        get { return m_CharacterData; }
    }
}
