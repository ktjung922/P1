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
        RemoveDeckCharacter,
        CheckMaxSelect
    }

    [SerializeField]
    private Transform m_TransOfHasCard;

    [SerializeField]
    private Transform m_TransOfDeck;

    [SerializeField]
    private Transform m_TransOfSynergy;

    private List<UICharacterSlot> m_ListOfCharacterSlot = new List<UICharacterSlot>();

    private List<UICharacterDeckSlot> m_ListOfCharacterDeckSlot = new List<UICharacterDeckSlot>();

    private List<UIDeckSynergyInfoSlot> m_ListOfSynergyInfoSlot = new List<UIDeckSynergyInfoSlot>();

    private int kMAX_SELECT = 4;

    public override void DisposeObject()
    {
        GameManager.Instance.DisposeObjectList(m_ListOfCharacterSlot);
        GameManager.Instance.DisposeObjectList(m_ListOfCharacterDeckSlot);
        GameManager.Instance.DisposeObjectList(m_ListOfSynergyInfoSlot);
        PlayManager.Instance.ClearDeckData();
        base.DisposeObject();
    }

    public override void Initialization()
    {
        PoolManager.Instance.Create<UICharacterSlot>(Constants.kPREFAB_UI_LOBBY_UICHARACTER_SLOT, 1);
        PoolManager.Instance.Create<UICharacterDeckSlot>(Constants.kPREFAB_UI_LOBBY_UICHARACTER_DECK_SLOT, 1);
        PoolManager.Instance.Create<UITypeSlot>(Constants.kPREFAB_UI_LOBBY_UITYPE_SLOT, 1);
        PoolManager.Instance.Create<UIDeckSynergyInfoSlot>(Constants.kPREFAB_UI_LOBBY_UIDECK_SYNERGY_INFO_SLOT, 1);
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

        switch(noti)
        {
            case kNOTFY.SelectCharacter: SelectCharacter(obj, callback); break;
            case kNOTFY.RemoveDeckCharacter: RemoveDeckCharacter(obj, callback); break;
        }
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

    private void RemoveDeckCharacter(NObject obj, Action callback)
    {
        var slot = obj as UICharacterDeckSlot;
        var characterData = slot.CharacterData;

        UpdateDeckDataInPlayManager(characterData);
        m_ListOfCharacterSlot.Find(data => data.CharacterData.INDEX == characterData.INDEX).UpdateActive(false);
        m_ListOfCharacterDeckSlot.Remove(slot);
        slot.DisposeObject();
    }

    private void SelectCharacter(NObject obj, Action callback)
    {
        var slot = obj as UICharacterSlot;
        var characterData = slot.CharacterData;

        var result = m_ListOfCharacterDeckSlot.Find(foundData => foundData.CharacterData.INDEX == characterData.INDEX);
        if (result != null)
        {
            m_ListOfCharacterDeckSlot.Remove(result);
            UpdateDeckDataInPlayManager(characterData);
            result.DisposeObject();
            callback?.Invoke();
            return;
        }

        if (m_ListOfCharacterDeckSlot.Find(foundData => foundData.CharacterData.GROUP == characterData.GROUP))
            return;

        var deckObj = PoolManager.Instance.Pop<UICharacterDeckSlot>(m_TransOfDeck);
        deckObj.UpdateUI(characterData);
        m_ListOfCharacterDeckSlot.Add(deckObj);
        UpdateDeckDataInPlayManager(characterData);
        callback?.Invoke();
    }

    private void UpdateDeckDataInPlayManager(CharacterData characterData)
    {
        var deckData = PlayManager.Instance.Deck;
        if (deckData.Contains(characterData))
        {
            deckData.Remove(characterData);
            UpdateSynergyDataInPlayManager(characterData, true);
        }
        else
        {
            deckData.Add(characterData);
            UpdateSynergyDataInPlayManager(characterData, false);
        }
    }

    private void UpdateSynergyDataInPlayManager(CharacterData characterData, bool isDelete)
    {
        var synergyData = PlayManager.Instance.Synergy;
        PlayManager.Instance.UpdateSynergy(characterData.ELEMENTAL, isDelete);

        MakeSynergyInfo();
    }

    private void MakeSynergyInfo()
    {
        GameManager.Instance.DisposeObjectList(m_ListOfSynergyInfoSlot);

        var result = PlayManager.Instance.Synergy;
        foreach (var data in result)
        {
            Debug.Log(data.Key + " / " + data.Value);
            var count = (data.Key == ElementalData.kTYPE.Nothing) ? PlayManager.Instance.GetCountOfDPSInDeck() : data.Value;

            var activeCount = TableManager.Instance.GetCountOfSynergyActive((int)data.Key, count);
            if (activeCount == -1)
                continue;

            var obj = PoolManager.Instance.Pop<UIDeckSynergyInfoSlot>(m_TransOfSynergy);
            obj.UpdateUI((int)data.Key, activeCount);
            m_ListOfSynergyInfoSlot.Add(obj);
        }
    }
}
