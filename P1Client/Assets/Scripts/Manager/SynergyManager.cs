using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NodapParty;

public class SynergyManager : SingletonGameObject<SynergyManager>
{
    private Dictionary<SynergyUpgradeData.kTYPE, List<Tuple<SynergyData, float>>> m_DicOfSynergy = new Dictionary<SynergyUpgradeData.kTYPE, List<Tuple<SynergyData, float>>>();

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
                var tuple = new Tuple<SynergyData, float>(synergyData, upgradeData.RATE);
                m_DicOfSynergy[upgradeData.TYPE].Add(tuple);
            }
            else
            {
                var newList = new List<Tuple<SynergyData, float>>();
                var tuple = new Tuple<SynergyData, float>(synergyData, upgradeData.RATE);
                newList.Add(tuple);
                m_DicOfSynergy.Add(upgradeData.TYPE, newList);
            }
        }
    }

    public List<Tuple<SynergyData, float>> GetUpgradeSynergyDataWithType(SynergyUpgradeData.kTYPE type)
    {
        if (!m_DicOfSynergy.ContainsKey(type))
            return null;
        
        return m_DicOfSynergy[type];
    }
}
