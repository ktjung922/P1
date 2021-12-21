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
        GameManager.Instance.Push<UIGacha>(Constants.kPREFAB_UI_LOBBY_UIGACHA);
    }
    
    public void OnTouchDeck()
    {
        GameManager.Instance.Push<UIBook>(Constants.kPREFAB_UI_LOBBY_UIBOOK);
    }

    public void OnTouchPlay()
    {
        GameManager.Instance.Push<UIDeck>(Constants.kPREFAB_UI_LOBBY_UIDECK);
    }

    public void OnTouchReset()
    {
        DataManager.Instance.ResetUserData();
    }
}
