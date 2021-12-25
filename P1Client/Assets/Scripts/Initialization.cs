
using UnityEngine;
using NodapParty;

public class Initialization : NObject
{
    public override void DisposeObject()
	{
	}

    private void Awake() {
        GameManager.Instance.Initialization(1280.0f, 720.0f);

        // 순서 꼭 지키기.
        TableManager.Instance.LoadDefaultResources();
        GachaManager.Instance.LoadDefaultGachaData();
        DataManager.Instance.LoadUserData();
        TextManager.Instance.LoadLocalTextFile("Scripts/STRING_LOCAL");
        PlayManager.Instance.Initialization();

        GameManager.Instance.Push<UILobby>(Constants.kPREFAB_LOBBY_UI_LOBBY);
    }
}
