using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SynergyData
{
    public enum kTARGET_TYPE
    {
        NONE,
        NOMAL,
        ALL,
        ONLY_ME
    }

    public int INDEX;

    public int STRING_INDEX;

    public kTARGET_TYPE TARGET;

    public ColorData COLOR_CODE;

    public List<SynergyIFData> IF;

    public List<SynergyUpgradeData> UPGRADE;
}
