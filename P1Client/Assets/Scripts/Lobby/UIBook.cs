using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodapParty;

public class UIBook : NLayer
{
    [SerializeField]
    private Transform m_TransformOfObtained;

    [SerializeField]
    private Transform m_TransformOfNonObtained;

    [SerializeField]
    private ScrollRect  m_ScrollRect;

    public override void DisposeObject()
    {
        base.DisposeObject();
    }

    public override void Initialization()
    {
        base.Initialization();
        PoolManager.Instance.Create<UICharacterSlot>(Constants.kPREFAB_UI_LOBBY_UICHARACTER_SLOT, 1);

        var list = TableManager.Instance.GetCharacterData();
        list.ForEach(data => 
        {
            var transTemp = DataManager.Instance.HasCardDataWithIndex(data.INDEX)? m_TransformOfObtained : m_TransformOfNonObtained;

            var obj = PoolManager.Instance.Pop<UICharacterSlot>(transTemp);
            obj.UpdateUI(data, false);
        });
    }

    public override void OnEscapeEvent()
    {
        GameManager.Instance.Pop<UIBook>();
    }
}
