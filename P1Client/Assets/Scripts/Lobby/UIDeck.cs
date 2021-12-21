using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;
using System;

public class UIDeck : NLayer
{
    public enum kNOTFY
    {
        SelectCharacter,
        CheckMaxSelect
    }

    [SerializeField]
    private Transform m_TransOfHasCard;

    [SerializeField]
    private Transform m_TransOfDeck;

    private List<UICharacterSlot> m_ListOfCharacterSlot = new List<UICharacterSlot>();

    private List<UICharacterDeckSlot> m_ListOfCharacterDeckSlot = new List<UICharacterDeckSlot>();

    private int kMAX_SELECT = 4;

    private Dictionary<ElementalData.kTYPE, int> m_DicOfSynergy;

    public override void DisposeObject()
    {
        GameManager.Instance.DisposeObjectList(m_ListOfCharacterSlot);
        base.DisposeObject();
    }

    public override void Initialization()
    {
        PoolManager.Instance.Create<UICharacterSlot>(Constants.kPREFAB_UI_LOBBY_UICHARACTER_SLOT, 1);
        PoolManager.Instance.Create<UICharacterDeckSlot>(Constants.kPREFAB_UI_LOBBY_UICHARACTER_DECK_SLOT, 1);
        PoolManager.Instance.Create<UITypeSlot>(Constants.kPREFAB_UI_LOBBY_UITYPE_SLOT, 1);
        base.Initialization();

        MakeCharacterSlot();
    }

    public override void OnEscapeEvent()
    {
        GameManager.Instance.Pop<UIDeck>();
    }

    public override void ReceiveObject(Enum value, NObject obj, Action callback = null)
    {
        var noti = (kNOTFY)Enum.Parse(typeof(kNOTFY), value.ToString());
        if (noti != kNOTFY.SelectCharacter)
            return;
                  
        SelectCharacter(obj, callback);
    }

    public override bool CheckResult(Enum value)
    {
        var noti = (kNOTFY)Enum.Parse(typeof(kNOTFY), value.ToString());
        if (noti != kNOTFY.CheckMaxSelect)
            return false;
        
        if (m_ListOfCharacterDeckSlot.Count >= kMAX_SELECT)
            return false;
        
        return true;
    }

    public void MakeCharacterSlot()
    {
        var hasCharacterIndex = DataManager.Instance.HasAllCharacterCardIndex();
        hasCharacterIndex.Sort(delegate(int a, int b) 
        {
            if (a > b)      return 1;
            else if (a < b) return -1;
            else                        return 0;
        });
        hasCharacterIndex.ForEach(index => 
        {
            var characterData = TableManager.Instance.GetCharacterDataWithIndex(index);
            var obj = PoolManager.Instance.Pop<UICharacterSlot>(m_TransOfHasCard);
            obj.UpdateUI(characterData, true);
            m_ListOfCharacterSlot.Add(obj);
        });
    }

    private void SelectCharacter(NObject obj, Action callback)
    {
        var slot = obj as UICharacterSlot;
        var characterData = slot.CharacterData;

        var result = m_ListOfCharacterDeckSlot.Find(foundData => foundData.CharacterData.INDEX == characterData.INDEX);
        if (result != null)
        {
            m_ListOfCharacterDeckSlot.Remove(result);
            result.DisposeObject();
            callback?.Invoke();
            return;
        }
        Debug.Log(characterData.GROUP);
        if (m_ListOfCharacterDeckSlot.Find(foundData => foundData.CharacterData.GROUP == characterData.GROUP))
            return;

        var deckObj = PoolManager.Instance.Pop<UICharacterDeckSlot>(m_TransOfDeck);
        deckObj.UpdateUI(characterData);
        m_ListOfCharacterDeckSlot.Add(deckObj);
        callback?.Invoke();
        GetAllSynergy();
    }

    private void GetAllSynergy()
    {
        List<ElementalData> newList = new List<ElementalData>();
        m_ListOfCharacterDeckSlot.ForEach(slot => 
        {
            newList.AddRange(slot.CharacterData.ELEMENTAL);
        });

        newList.ForEach(data => 
        {
            Debug.Log(data.SYNERGY.ToString());
        });
    }
}
