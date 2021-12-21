using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SynergyUpgradeData
{
    public enum kTYPE
    {
        NONE,
        ATTACK_POWER,
        ATTACK_SPEED,
        NO_DAP,
        DEMAGE,
        ARMOR_BREAK,
        ADDITIONAL_DEMAGE,
        GIVE_SYNERGY,
        CHABGE_Skill_2,
    }

    public kTYPE TYPE;

    public float RATE;
}
