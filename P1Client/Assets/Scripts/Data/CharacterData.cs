
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public sealed class CharacterData
{
    public enum kGRADE
    {
        None,
        One,
        Two,
        Three
    }

    public enum kTYPE
    {
        ATTACK = 150,
        MAGIC = 151,
        BUFFER = 152
    }

    public enum kGROUP
    {
        None,
        Kyungin,
        Hyungtae,
        Taeho,
        Kitae,
        Jongyub,
        Jungi,
        Juhwan,
        Sungjun,
        Jaewon,
        Unsub,
        Kyungtae
    }

    public int INDEX;

    public int DESC_INDEX;

    public kTYPE TYPE;

    public kGROUP GROUP;

    public kGRADE GRADE;

    public int WEIGHT;

    public string CARD_IMG;

    public int DEMAGE;

    public float ATTACK_SPEED;

    public int CRTICAL;

    public int NO_DAP;

    public List<ElementalData> ELEMENTAL;

    public List<ActionPatternData> ACTION_PATTERN;

    public int SPECIAL_MOVE_INDEX;
}
