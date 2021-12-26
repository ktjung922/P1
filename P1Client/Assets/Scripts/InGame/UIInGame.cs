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
        base.Hide();
    }

    public override void Show()
    {
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
        m_RemainTime = 10;
        m_Tic = 0f;
        m_TextOfTime.text = m_RemainTime.ToString();

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
