using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SynergyIFData
{
    public enum kTYPE
    {
        NONE,
        IF,
        IF_NOT
    }

    public kTYPE COUNT_TYPE;

    public int COUNT;
}
