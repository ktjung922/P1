using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;

public class IGBaseParticle : NObject
{
    [SerializeField]
    private ParticleSystem m_Particle;

    public override void DisposeObject()
    {
        PoolManager.Instance.Push<IGBaseParticle>(this);
    }

    void Update()
    {
        if (m_Particle.isPlaying)
            return;
        
        DisposeObject();
    }
}
