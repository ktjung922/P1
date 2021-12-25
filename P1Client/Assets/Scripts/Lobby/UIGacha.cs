using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodapParty;

public class UIGacha : NLayer
{
    public enum kSTATE
    {
        None,
        Character,
        Jewel
    }

    [SerializeField]
    private Text m_TextOfTitle;

    [SerializeField]
    private Text m_TextOfDesc;

    [SerializeField]
    private Image   m_ImageOfOneIcon;

    [SerializeField]
    private Image   m_ImageOfTenIcon;

    [Header("Tab")]
    [SerializeField]
    private Text    m_TextOfCharacterTab;

    [SerializeField]
    private Image    m_ImageOfCharacterTab;

    [SerializeField]
    private Text    m_TextOfJewelTab;

    [SerializeField]
    private Image   m_ImageOfJewelTab;

    private kSTATE  m_State;

    public override void DisposeObject()
    {
        base.DisposeObject();
    }

    public override void Initialization()
    {
        base.Initialization();

        UpdateUI(kSTATE.Character);
    }

    public override void OnEscapeEvent()
    {
        GameManager.Instance.Pop<UIGacha>();
    }

    public void UpdateUI(kSTATE state)
    {
        if (m_State == state)
            return;
        
        m_State = state;

        switch (m_State)
        {
            case kSTATE.Character:  UpdateCharacterUI();    break;
            case kSTATE.Jewel:      UpdateJewelUI();        break;
        }
    }

    private void UpdateCharacterUI()
    {
        m_TextOfTitle.text = "일반 뽑기";
        m_TextOfDesc.text = "뽑기 설명";
    }

    private void UpdateJewelUI()
    {
        Debug.Log("주엘UI");
    }

    public void OnTouchCharacterTab()
    {
        UpdateUI(kSTATE.Character);
    }

    public void OnTouchJewelTab()
    {
        UpdateUI(kSTATE.Jewel);
    }

    public void OnTouchOne()
    {
        var result = GachaManager.Instance.GetRandomCharacterData();
        var newList = new List<CharacterData>();
        newList.Add(result);
        GameManager.Instance.Push<UIGachaPage>(delegate (UIGachaPage layer) 
        {
            layer.SetResultGachaData(newList);
        }, Constants.kPREFAB_LOBBY_UI_GACHA_PAGE);
        
    }

    public void OnTouchTen()
    {
        var newList = GachaManager.Instance.GetRandomCharacterData10();
        GameManager.Instance.Push<UIGachaPage>(delegate (UIGachaPage layer) 
        {
            layer.SetResultGachaData(newList);
        }, Constants.kPREFAB_LOBBY_UI_GACHA_PAGE);
    }
}
