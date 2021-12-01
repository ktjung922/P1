using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;


public class UILobby : NLayer
{
    public override void Initialization()
    {
        base.Initialization();

        Debug.Log("테스트");
    }

    public void OnTouch()
    {
        Debug.Log("터치");
    }
}
