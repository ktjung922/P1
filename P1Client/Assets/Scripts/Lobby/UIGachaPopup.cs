using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodapParty;

public class UIGachaPopup : NLayer
{
    public enum kANIM_STATE
    {
        None,
        Start
    }

    [SerializeField]
    private Animator    m_Animator;

    [SerializeField]
    private Image       m_ImageOfMain;

    [SerializeField]
    private Text        m_TextOfName;

    [SerializeField]
    private Text        m_TextOfDialoge;

    [SerializeField]
    private GameObject[]    m_ObjectsOfStar;

    private Queue<CharacterData> m_Queue;

    public override void DisposeObject()
    {
        base.DisposeObject();
        GameManager.Instance.Notify<UIGachaPage>(UIGachaPage.kNOTIFY.ChangeAllAnimation);
    }

    public override void Initialization()
    {
        base.Initialization();
    }

    public override void OnEscapeEvent()
    {
        GameManager.Instance.Pop<UIGachaPopup>();
    }

    public void UpdateUI(Queue<CharacterData> queue)
    {
        m_Queue = queue;
        UpdateCharacter();
    }

    public void UpdateCharacter()
    {
        if (m_Queue.Count == 0)
        {
            OnEscapeEvent();
            return;
        }
        var data = m_Queue.Dequeue();

        m_Animator.SetTrigger(kANIM_STATE.Start.ToString());

        UpdateImage(data);
        UpdateInfo(data);
        UpdateStar(data);
    }

    public void UpdateImage(CharacterData data)
    {
        m_ImageOfMain.sprite = UtillManager.Instance.GetSprite(data.CARD_IMG);
    }

    public void UpdateInfo(CharacterData data)
    {
        m_TextOfName.text = TextManager.Instance.GetText(data.INDEX);
        m_TextOfDialoge.text = TextManager.Instance.GetText(data.DESC_INDEX);
    }

    public void UpdateStar(CharacterData data)
    {
        for (int i = 0; i < m_ObjectsOfStar.Length; i++)
        {
            m_ObjectsOfStar[i].SetActive(i < (int)data.GRADE);
        }
    }

    public void OnTouch()
    {
        var info = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime < 1.0f)
            return;

        UpdateCharacter();
    }

    public void OnTouchSkip()
    {
        m_Queue.Clear();
        OnEscapeEvent();
    }
}
