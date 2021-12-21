using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public sealed class ElementalData
{
    public enum kTYPE
    {
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
}
