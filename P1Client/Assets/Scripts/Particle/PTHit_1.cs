using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;

public class PTHit_1 : IGBaseParticle
{
    public override void DisposeObject()
    {
        PoolManager.Instance.Push<PTHit_1>(this);
    }
}
