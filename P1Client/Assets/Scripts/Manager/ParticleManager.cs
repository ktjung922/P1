using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;

public class ParticleManager : SingletonGameObject<ParticleManager>
{
    public enum kPARTICLE
    {
        None,
        PTHIT_1
    }

    public void Init()
    {
        PoolManager.Instance.Create<IGDamageText>(Constants.kPREFAB_INGAME_IG_DAMAGE_TEXT, 1);
        PoolManager.Instance.Create<PTHit_1>(Constants.kPARTICLE_HIT_1, 1);
    }

    public IGBaseParticle CreateParticle(kPARTICLE paticleType, Transform parent, Vector3 pos)
    {
        IGBaseParticle pt;

        switch(paticleType)
        {
            case kPARTICLE.PTHIT_1 : pt = PoolManager.Instance.Pop<PTHit_1>(parent); break;
            default: pt = PoolManager.Instance.Pop<PTHit_1>(parent); break;
        }
        
        pt.CacheTransform.position = pos;
        pt.Show();

        return pt;
    }
}
