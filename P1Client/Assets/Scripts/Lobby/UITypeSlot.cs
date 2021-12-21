using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodapParty;

public class UITypeSlot : NObject
{
    [SerializeField]
    private Image   m_ImageOfBG;

    [SerializeField]
    private Text    m_Text;

    public override void DisposeObject()
    {
        PoolManager.Instance.Push<UITypeSlot>(this);
    }

    public void UpdateUI(int index)
    {
        m_Text.text = TextManager.Instance.GetText(index);
        base.Show();
    }
}
