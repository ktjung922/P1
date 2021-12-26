using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;

public class SynergyManager : SingletonGameObject<SynergyManager>
{
    private Dictionary<SynergyUpgradeData.kTYPE, List<float>> m_DicOfSynergy = new Dictionary<SynergyUpgradeData.kTYPE, List<float>>();

    public void UpdateSynergy()
    {
        m_DicOfSynergy.Clear();

        var result = PlayManager.Instance.Synergy;
        foreach (var data in result)
        {
            var count = (data.Key == ElementalData.kTYPE.Nothing) ? PlayManager.Instance.GetCountOfDPSInDeck() : data.Value;

            var activeCount = TableManager.Instance.GetCountOfSynergyActive((int)data.Key, count);
            if (activeCount == -1)
                continue;

            var synergyData = TableManager.Instance.GetSynergyDataWithIndex((int)data.Key);
            var upgradeData = synergyData.UPGRADE[activeCount];

            if (m_DicOfSynergy.ContainsKey(upgradeData.TYPE))
            {
                m_DicOfSynergy[upgradeData.TYPE].Add(upgradeData.RATE);
            }
            else
            {
                var newList = new List<float>();
                newList.Add(upgradeData.RATE);
                m_DicOfSynergy.Add(upgradeData.TYPE, newList);
            }
        }
    }

    public List<float> GetUpgradeSynergyDataWithType(SynergyUpgradeData.kTYPE type)
    {
        if (!m_DicOfSynergy.ContainsKey(type))
            return null;
        
        return m_DicOfSynergy[type];
    }
}
