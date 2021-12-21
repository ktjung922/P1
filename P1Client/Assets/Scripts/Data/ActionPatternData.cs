
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public sealed class ActionPatternData
{
    public enum kPATTERN
    {
        None,
        ATTACK,
        SKILL1,
        SKILL2
    }

    public kPATTERN ACTION;
}
