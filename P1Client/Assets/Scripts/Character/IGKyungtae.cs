using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;

public class IGKyungtae : IGBaseCharacter
{
    public override void DisposeObject()
    {
        base.ResetObject();
        PoolManager.Instance.Push<IGKyungtae>(this);
    }

    
}
