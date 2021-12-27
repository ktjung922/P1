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
        CHANGE_SKILL_2,
        DEFANSIVE_IGNORE,
        UP_CHARGING_SPECIAL,
        CRITICAL,
        MORE_SPECIAL
    }

    public kTYPE TYPE;

    public float RATE;
}
