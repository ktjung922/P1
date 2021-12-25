using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;

public class IGBaseCharacter : NObject
{
    public enum kACTION
    {
        Idle,
        Attack,
        Skill_1,
        Skill_2,
        Special
    }

    [SerializeField]
    private Animator    m_Animator;

    [SerializeField]
    private SpriteRenderer      m_SpriteOfCharacter;

    private kACTION     m_CurrentAction = kACTION.Idle;

    protected int m_Damage;

    protected float m_AttackSpeed;

    protected float m_Critical;

    protected CharacterData m_CharacterData;

    protected List<ActionPatternData> m_Actions;

    protected int m_ActionNum;

    public override void DisposeObject()
    {
        StopAllCoroutines();
        PoolManager.Instance.Push<IGBaseCharacter>();
    }

    public virtual void InitObject(CharacterData characterData)
    {
        m_Damage = characterData.DEMAGE;
        m_AttackSpeed = characterData.ATTACK_SPEED;
        m_Critical = characterData.CRTICAL;

        m_CharacterData = characterData;
        m_Actions = m_CharacterData.ACTION_PATTERN;

        m_SpriteOfCharacter.sprite = UtillManager.Instance.GetSprite(characterData.CARD_IMG);

        base.Show();
        StartCoroutine();
    }

    protected void StartCoroutine()
    {
        StartCoroutine(FSM());
    }

    public virtual IEnumerator FSM()
    {
        yield return null;

        while (true)
        {
            yield return StartCoroutine(m_CurrentAction.ToString());
            m_CurrentAction = (kACTION)m_Actions[m_ActionNum].ACTION;
            PlusActionNum();
        }
    }

    public virtual IEnumerator Idle()
    {
        Debug.Log(m_CharacterData.CARD_IMG + ": Idle");
        yield return new WaitForSeconds(1f);
        m_ActionNum = 0;
    }

    public virtual IEnumerator Attack()
    {
        Debug.Log(m_CharacterData.CARD_IMG + ": Attack");
        yield return new WaitForSeconds(1f);
    }

    public virtual IEnumerator Skill_1()
    {
        Debug.Log(m_CharacterData.CARD_IMG + ": Skill_1");
        yield return new WaitForSeconds(1f);
    }
    
    public virtual IEnumerator Skill_2()
    {
        Debug.Log(m_CharacterData.CARD_IMG + ": Skill_2");
        yield return new WaitForSeconds(1f);
    }

    public virtual IEnumerator Special()
    {
        Debug.Log(m_CharacterData.CARD_IMG + ": Special");
        yield return new WaitForSeconds(1f);
    }

    public virtual int CalculateDeal()
    {
        return 0;
    }

    public virtual float CalculateSpeed()
    {
        return 0;
    }

    public virtual float CalculateCritical()
    {
        return 0;
    }

    public virtual void DamageDealing()
    {

    }

    protected void PlusActionNum(int num = 1)
    {
        m_ActionNum += num;
        if (m_ActionNum >= m_Actions.Count)
        {
            m_ActionNum -= m_Actions.Count;
        }
    }
}
