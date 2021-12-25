using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;

public class IGBoss : NObject
{
    public float m_Defense = 99.9f;

    public Dictionary<int, int> m_DicDamage;

    public override void DisposeObject()
    {
        PoolManager.Instance.Push<IGBoss>(this);
    }

    public void Init()
    {
        m_DicDamage.Clear();
        m_Defense = 99.9f;
    }

    public void Demaged(int damage, int charIndex)
    {
        if (m_DicDamage.ContainsKey(charIndex))
        {
            m_DicDamage[damage] += damage;
        }
        else
        {
            m_DicDamage.Add(charIndex, damage);
        }
    }
}
