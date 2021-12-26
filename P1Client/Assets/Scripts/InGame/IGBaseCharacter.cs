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

    public enum kANIM_TRIGGER
    {
        Idle,
        Attack,
        Skill,
        Buff
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

    protected bool m_IsEndIdle = false;

    private string kATTACK_SPEED_NAME = "Attack_Speed";

    private WaitForSeconds kWAIT = new WaitForSeconds(1f);

    public override void DisposeObject()
    {
        ResetObject();
        PoolManager.Instance.Push<IGBaseCharacter>(this);
    }

    public virtual void ResetObject()
    {
        StopAllCoroutines();
        m_Animator.Rebind();
        m_Animator.SetFloat(kATTACK_SPEED_NAME, 1.0f);
        m_CurrentAction = kACTION.Idle;
        m_IsEndIdle = false;
        
        foreach (var item in m_Animator.parameters)
        {
            if (item.type == AnimatorControllerParameterType.Trigger)
            {
                m_Animator.ResetTrigger(item.name);
            }
        }
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
        if (m_Actions == null || m_Actions.Count == 0)
            return;

        UpdateAttackSpeed(m_AttackSpeed);
        
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

            yield return new WaitUntil(() => { if (PlayManager.Instance.canTimeFlow) return true; return false; } );
            m_CurrentAction = (kACTION)m_Actions[m_ActionNum].ACTION;
            PlusActionNum();
        }
    }

    public virtual IEnumerator Idle()
    {
        m_Animator.SetTrigger(kANIM_TRIGGER.Idle.ToString());
        
        Debug.Log(m_CharacterData.CARD_IMG + ": Idle");
        yield return new WaitForSeconds(1f);
        m_ActionNum = 0;
        m_IsEndIdle = true;
    }

    public virtual IEnumerator Attack()
    {
        m_Animator.SetTrigger(kANIM_TRIGGER.Attack.ToString());
        Debug.Log(m_CharacterData.CARD_IMG + ": Attack");
        yield return kWAIT;
        AttackDamage(m_Damage);
        yield return kWAIT;
        m_Animator.ResetTrigger(kANIM_TRIGGER.Attack.ToString());
    }

    public virtual IEnumerator Skill_1()
    {
        m_Animator.SetTrigger(kANIM_TRIGGER.Skill.ToString());
        Debug.Log(m_CharacterData.CARD_IMG + ": Skill_1");
        yield return kWAIT;

        yield return kWAIT;
        m_Animator.ResetTrigger(kANIM_TRIGGER.Skill.ToString());
    }
    
    public virtual IEnumerator Skill_2()
    {
        m_Animator.SetTrigger(kANIM_TRIGGER.Buff.ToString());
        Debug.Log(m_CharacterData.CARD_IMG + ": Skill_2");
        yield return kWAIT;

        yield return kWAIT;
        m_Animator.ResetTrigger(kANIM_TRIGGER.Buff.ToString());
    }

    public virtual IEnumerator Special()
    {
        m_Animator.SetTrigger(kANIM_TRIGGER.Skill.ToString());
        Debug.Log(m_CharacterData.CARD_IMG + ": Special");
        yield return kWAIT;

        yield return kWAIT;
        m_Animator.ResetTrigger(kANIM_TRIGGER.Skill.ToString());
    }

    public virtual void AttackDamage(int damage)
    {
        PlayManager.Instance.AttackBoss(damage, m_CharacterData.INDEX, ParticleManager.kPARTICLE.PTHIT_1);
    }

    public virtual void UpdateAttackSpeed(float speed)
    {
        m_Animator.SetFloat(kATTACK_SPEED_NAME, speed);
        kWAIT = new WaitForSeconds(0.5f / speed);
    }

    protected void PlusActionNum(int num = 1)
    {
        m_ActionNum += num;
        if (m_ActionNum >= m_Actions.Count)
        {
            m_ActionNum -= m_Actions.Count;
        }
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

    public bool isEndIdle { get { return m_IsEndIdle; } }
}
