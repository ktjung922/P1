using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;


public class UILobby : NLayer
{
    public override void Initialization()
    {
        base.Initialization();
    }

    public void OnTouchGacha()
    {
        GameManager.Instance.Push<UIGacha>(Constants.kPREFAB_LOBBY_UI_GACHA);
    }
    
    public void OnTouchDeck()
    {
        GameManager.Instance.Push<UIBook>(Constants.kPREFAB_LOBBY_UI_BOOK);
    }

    public void OnTouchPlay()
    {
        GameManager.Instance.Push<UIDeck>(Constants.kPREFAB_LOBBY_UI_DECK);
    }

    public void OnTouchReset()
    {
        DataManager.Instance.ResetUserData();
    }
}
