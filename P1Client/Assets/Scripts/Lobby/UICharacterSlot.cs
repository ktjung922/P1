using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodapParty;

public class UICharacterSlot : NObject
{
    [SerializeField]
    private Image           m_ImageOfCharacter;

    [SerializeField]
    private Text            m_TextOfName;

    [SerializeField]
    private GameObject      m_ObjectOfBlack;

    [SerializeField]
    private GameObject[]    m_ObjectsOfStar;
    
    [SerializeField]
    private GameObject      m_RootOfName;

    [SerializeField]
    private GameObject      m_ObjectOfActive;

    private CharacterData   m_CharacterData;

    private bool            m_IsActive;

    public override void DisposeObject()
    {
        PoolManager.Instance.Push<UICharacterCardSlot>();
    }

    public void UpdateUI(CharacterData data, bool isDeck)
    {
        m_CharacterData = data;

        var has = DataManager.Instance.HasCardDataWithIndex(m_CharacterData.INDEX);
        m_ObjectOfBlack.SetActive(!has);
        m_RootOfName.SetActive(!isDeck);

        m_ImageOfCharacter.sprite = UtillManager.Instance.GetSprite(m_CharacterData.CARD_IMG);
        UpdateStar();
        UpdateName();
        UpdateActive(true);

        base.Show();
    }

    private void UpdateStar()
    {
        for (int i = 0; i < m_ObjectsOfStar.Length; i++)
        {
            m_ObjectsOfStar[i].SetActive(i < (int)m_CharacterData.GRADE);
        }
    }

    private void UpdateName()
    {
        m_TextOfName.text = TextManager.Instance.GetText(m_CharacterData.INDEX);
    }

    public void UpdateActive(bool isInit)
    {
        if (!isInit) 
            m_IsActive = !m_IsActive;
        
        m_ObjectOfActive.SetActive(m_IsActive);
    }

    public void OnTouch()
    {
        if (!GameManager.Instance.CheckResult<UIDeck>(UIDeck.kNOTFY.CheckMaxSelect) && !m_IsActive)
            return;
        
        GameManager.Instance.SendObjectAndCallBack<UIDeck>(UIDeck.kNOTFY.SelectCharacter, this, delegate() 
        {
            UpdateActive(false);
        });
    }

    public CharacterData CharacterData
    {
        get { return m_CharacterData; }
    }
}
