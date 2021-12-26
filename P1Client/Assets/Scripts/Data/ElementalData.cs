using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public sealed class ElementalData
{
    public enum kTYPE
    {
        None,
        Glasses = 100,
        Eyes,
        Machine,
        Arousal,
        Darkening,
        Love,
        Attractive,
        Electricity,
        Card,
        Party,
        Unstoppable,
        SwimmingPool,
        Water,
        Blood,
        Nothing
    }

    public kTYPE SYNERGY;

    public bool Given;
    
    public bool isGiveSynergyType()
    {
        var result = TableManager.Instance.GetSynergyDataWithIndex((int)SYNERGY)?.UPGRADE?.First();
        if (result == null || result.TYPE != SynergyUpgradeData.kTYPE.GIVE_SYNERGY)
            return false;

        return true;
    }
}
