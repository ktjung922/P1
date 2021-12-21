using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;
using UnityEngine.UI;

public class UICharacterCardSlot : NObject
{
    public enum kANIM_STATE
    {
        None,
        Start,
        Change
    }

    [SerializeField]
    private  Image  m_ImageOfBack;

    [SerializeField]
    private  Image  m_ImageOfFront;

    [SerializeField]
    private GameObject  m_ObjectOfImageRoot;

    [SerializeField]
    private GameObject  m_ObjectOfCoin;

    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private GameObject[]    m_ObjectsOfStar;

    private CharacterData m_CharacterData;

    private bool            m_IsStart = false;

    private string          kCARD_STRING_TEMP = "Card_0";

    public override void DisposeObject()
    {
        m_IsStart = false;
        m_ImageOfBack.sprite = null;
        m_CharacterData = null;
        PoolManager.Instance.Push<UICharacterCardSlot>(this);
    }

    public override void Show()
    {
        m_ObjectOfImageRoot.SetActive(false);

        base.Show();
    }

    public void UpdateUI(CharacterData characterData, bool isDuplication)
    {
        m_CharacterData = characterData;

        var cardBackSpriteName = string.Concat(kCARD_STRING_TEMP, (int)characterData.GRADE);

        m_ImageOfBack.sprite = UtillManager.Instance.GetSprite(cardBackSpriteName);
        m_ImageOfFront.sprite = UtillManager.Instance.GetSprite(m_CharacterData.CARD_IMG);
        m_ObjectOfCoin.SetActive(isDuplication);
        UpdateStar(m_CharacterData);
        Show();
    }

    public void StartAnimation()
    {
        m_ObjectOfImageRoot.SetActive(true);
        m_Animator.SetTrigger(kANIM_STATE.Start.ToString());
    }

    private void EndOfStartAnimation()
    {
        m_IsStart = true;
    }

    public bool GetIsStart()
    {
        return m_IsStart;
    }
    
    public void StartChange()
    {
        m_Animator.SetTrigger(kANIM_STATE.Change.ToString());
    }

    private void UpdateStar(CharacterData data)
    {
        for (int i = 0; i < m_ObjectsOfStar.Length; i++)
        {
            m_ObjectsOfStar[i].SetActive(i < (int)data.GRADE);
        }
    }
}
