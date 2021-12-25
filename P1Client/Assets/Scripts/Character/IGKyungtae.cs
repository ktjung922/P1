using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;

public class IGKyungtae : IGBaseCharacter
{
    public override void DisposeObject()
    {
        StopAllCoroutines();
        PoolManager.Instance.Push<IGBaseCharacter>();
    }

    public override void InitObject(CharacterData characterData)
    {
        Debug.Log("go");
        base.InitObject(characterData);
    }

    public override IEnumerator Skill_1()
    {
        return base.Skill_1();
    }

    public override IEnumerator Skill_2()
    {
        return base.Skill_2();
    }

    public override IEnumerator Special()
    {
        return base.Special();
    }
}
