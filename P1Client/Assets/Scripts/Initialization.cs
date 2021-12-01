
using UnityEngine;
using NodapParty;

public class Initialization : NObject
{
    public override void DisposeObject()
	{
	}

    private void Awake() {
        GameManager.Instance.Initialization(1280.0f, 720.0f);

        GameManager.Instance.Push<UILobby>(Constants.kPREFAB_UI_LOBBY_UILOBBY);
    }
}
