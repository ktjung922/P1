using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodapParty;

public class UIDeckSynergyInfoSlot : NObject
{
    [SerializeField]
    private Text    m_Title;

    [SerializeField]
    private Text    m_Desc;
    
    public override void DisposeObject()
    {
        PoolManager.Instance.Push<UIDeckSynergyInfoSlot>(this);
    }

    public void UpdateUI(int synergyIndex, int activeIndex)
    {
        m_Title.text = TextManager.Instance.GetText(synergyIndex);
        m_Desc.text = TextManager.Instance.GetSynergyText(synergyIndex, activeIndex);

        base.Show();
    }
}
