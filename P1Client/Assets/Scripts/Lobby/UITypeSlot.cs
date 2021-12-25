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
        m_ImageOfBG.color = Color.black;
        m_Text.color = Color.white;
        PoolManager.Instance.Push<UITypeSlot>(this);
    }

    public void UpdateUI(int index)
    {
        m_Text.text = TextManager.Instance.GetText(index);
        UpdateColot(index);
        base.Show();
    }

    private void UpdateColot(int index)
    {
        var synergyData = TableManager.Instance.GetSynergyDataWithIndex(index);
        if(synergyData == null)
            return;
        
        Color color;
        if (ColorUtility.TryParseHtmlString(synergyData.COLOR_CODE.BG, out color))
        {
            m_ImageOfBG.color = color;
        }

        if (ColorUtility.TryParseHtmlString(synergyData.COLOR_CODE.TEXT, out color))
        {
            m_Text.color = color;
        }
    }
}
