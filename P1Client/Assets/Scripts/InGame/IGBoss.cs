using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;

public class IGBoss : NObject
{
    [SerializeField]
    private Transform m_DemagePos;

    public float m_Defense = 99.9f;

    public Dictionary<int, int> m_DicDamage = new Dictionary<int, int>();

    private List<IGBaseParticle> m_ListOfParticle = new List<IGBaseParticle>();

    private List<IGDamageText>  m_ListOfDamageText = new List<IGDamageText>();

    public override void DisposeObject()
    {
        GameManager.Instance.DisposeObjectList(m_ListOfParticle);
        GameManager.Instance.DisposeObjectList(m_ListOfDamageText);
        PoolManager.Instance.Push<IGBoss>(this);
    }

    public void Init()
    {
        m_DicDamage.Clear();
        m_Defense = 99.9f;
        base.Show();
    }

    public void Demaged(int damage, int charIndex, ParticleManager.kPARTICLE paticle = ParticleManager.kPARTICLE.None)
    {
        if (paticle != ParticleManager.kPARTICLE.None)
        {
            var RandomX = Random.Range(CacheTransform.position.x - 1f, CacheTransform.position.x + 1f);
            var RandomY = Random.Range(CacheTransform.position.y - 2.5f, CacheTransform.position.y + 2.5f);

            var RandomPos = new Vector3(RandomX, RandomY, 0);

            var pt = ParticleManager.Instance.CreateParticle(paticle, this.CacheTransform, RandomPos);
            var text = PoolManager.Instance.Pop<IGDamageText>(m_DemagePos);
            text.UpdateShow(damage.ToString(), m_DemagePos.position, Color.white);

            m_ListOfParticle.Add(pt);
            m_ListOfDamageText.Add(text);
        }

        if (m_DicDamage.ContainsKey(charIndex))
        {
            m_DicDamage[charIndex] += damage;
        }
        else
        {
            m_DicDamage.Add(charIndex, damage);
        }

        Debug.Log(string.Format("Boss::Attack damaged: {0} And Accmulate damaged: {1} to Character: {2}", damage, m_DicDamage[charIndex], charIndex));
    }
}
