using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodapParty;

public class UIInGame : NLayer
{
    [SerializeField]
    private Text    m_TextOfTime;

    private float   m_Tic;

    private float   m_RemainTime;

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


}
