using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodapParty;
using System;

public class UIInGame : NLayer
{
    public enum kNOTIFY
    {
        StartTimer
    }

    [SerializeField]
    private Text    m_TextOfTime;

    private float   m_Tic;

    private int   m_RemainTime;

    private int kLIMIT_TIME = 60;

    public override void DisposeObject()
    {
        base.DisposeObject();
    }

    public override void Initialization()
    {
        base.Initialization();
    }

    public override void OnEscapeEvent()
    {
        base.OnEscapeEvent();
    }

    public override void Hide()
    {
        m_RemainTime = kLIMIT_TIME;
        m_TextOfTime.text = m_RemainTime.ToString();
        base.Hide();
    }

    public override void Show()
    {
        m_RemainTime = kLIMIT_TIME;
        m_TextOfTime.text = m_RemainTime.ToString();
        base.Show();
    }

    public override void Notify(Enum value)
    {
        var noti = (kNOTIFY)Enum.Parse(typeof(kNOTIFY), value.ToString());
        if (noti != kNOTIFY.StartTimer)
            return;
        
        StartCoroutine(TimeCheck());
    }

    private IEnumerator TimeCheck()
    {
        m_RemainTime = kLIMIT_TIME;
        m_Tic = 0f;

        while (m_RemainTime > 0)
        {
            m_Tic += Time.smoothDeltaTime;
            if (m_Tic > 1f)
            {
                m_Tic -= 1f;
                m_RemainTime -= 1;
                m_TextOfTime.text = m_RemainTime.ToString();
            }
            yield return null;
        }

        PlayManager.Instance.EndPlayGame();
    }
}
