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

    protected int m_Nodap;

    protected CharacterData m_CharacterData;

    protected List<ActionPatternData> m_Actions;

    protected int m_ActionNum;

    protected bool m_IsEndIdle = false;

    private string kATTACK_SPEED_NAME = "Attack_Speed";

    private WaitForSeconds kWAIT = new WaitForSeconds(1f);

    private List<SynergyUpgradeData> m_ListOfBuff = new List<SynergyUpgradeData>();

    public override void DisposeObject()
    {
        ResetObject();
        PoolManager.Instance.Push<IGBaseCharacter>(this);
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateAttackSpeed(2f);
        }
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
        m_CharacterData = characterData;
        m_Actions = m_CharacterData.ACTION_PATTERN;

        SetDamage();
        SetSpeed();
        SetCritical();
        SetNodap();

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
        AttackAction();
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

    public virtual void AttackDamage(int damage, bool isCrit)
    {
        PlayManager.Instance.AttackBoss(damage, m_CharacterData.INDEX, isCrit, ParticleManager.kPARTICLE.PTHIT_1);
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

    public virtual void AttackAction()
    {
        var deal = CalculateDeal();
        var crit = CalculateCritical();

        if (GachaManager.Instance.CheckedRandomRange(crit))
        {
            AttackDamage(deal * 2, true);
        }
        else 
        {
            AttackDamage(deal, false);
        }

        //TODO:: 추가공격.

    }

    public virtual void SkillOneAction()
    {

    }

    public virtual void SkillTwoAction()
    {

    }

    public virtual void SpecialAction()
    {

    }

    public virtual int CalculateDeal()
    {
        float tmp = m_Damage;
        
        // 자가 버프.
        m_ListOfBuff?.ForEach(data => 
        {
            tmp *= (1f + data.RATE);
        });

        //TODO:: 버브 버프 + 값.

        return Mathf.FloorToInt(tmp);
    }

    public virtual float CalculateSpeed()
    {
        return 0;
    }

    public virtual float CalculateCritical()
    {
        return m_Critical;
    }

    protected void SetDamage()
    {
        float tmp = m_CharacterData.DEMAGE;
        SynergyManager.Instance.GetUpgradeSynergyDataWithType(SynergyUpgradeData.kTYPE.ATTACK_POWER)?.ForEach(data => 
        {
            var synergyData = m_CharacterData.ELEMENTAL.Find(synergy => (int)synergy.SYNERGY == data.Item1.INDEX);
            if (synergyData == null)
                return;
            
            tmp *= (1 + data.Item2);
        });
        m_Damage = Mathf.FloorToInt(tmp);
    }

    protected void SetSpeed()
    {
        float tmp = 0f;
        SynergyManager.Instance.GetUpgradeSynergyDataWithType(SynergyUpgradeData.kTYPE.ATTACK_SPEED)?.ForEach(data =>
        {
            var synergyData = m_CharacterData.ELEMENTAL.Find(synergy => (int)synergy.SYNERGY == data.Item1.INDEX);
            if (synergyData == null)
                return;
            
            tmp += data.Item2;
        });
        tmp = m_CharacterData.ATTACK_SPEED * (1 + tmp);

        m_AttackSpeed = Mathf.Floor(tmp * 1000) * 0.001f;
    }

    protected void SetCritical()
    {
        float tmp = 0f;
        SynergyManager.Instance.GetUpgradeSynergyDataWithType(SynergyUpgradeData.kTYPE.CRITICAL)?.ForEach(data =>
        {
            var synergyData = m_CharacterData.ELEMENTAL.Find(synergy => (int)synergy.SYNERGY == data.Item1.INDEX);
            if (synergyData == null)
                return;
            
            tmp += data.Item2;
        });
        tmp = m_CharacterData.CRITICAL * (1 + tmp);

        m_Critical = Mathf.Floor(tmp * 1000) * 0.001f;
    }

    protected void SetNodap()
    {
        int tmp = m_CharacterData.NO_DAP;
        SynergyManager.Instance.GetUpgradeSynergyDataWithType(SynergyUpgradeData.kTYPE.NO_DAP)?.ForEach(data =>
        {
            var synergyData = m_CharacterData.ELEMENTAL.Find(synergy => (int)synergy.SYNERGY == data.Item1.INDEX);
            if (synergyData == null)
                return;
            
            tmp += (int)data.Item2;
        });

        m_Nodap = tmp;
    }

    public bool isEndIdle { get { return m_IsEndIdle; } }
}
