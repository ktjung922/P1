using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodapParty;
using TMPro;

public class IGDamageText : NObject
{
    [SerializeField]
    private TextMeshPro m_Text;

    [SerializeField]
    private int m_UpY = 4;

    private float m_Tic;

    private Vector3 m_StartPos;

    private Color m_Color;

    public override void DisposeObject()
    {
        m_Tic = 0f;
        m_Text.text = "";
        m_Text.color = m_Color;
        CacheTransform.position = new Vector3(0f, 0f, 0f);
        PoolManager.Instance.Push<IGDamageText>(this);
    }

    private void Update()
    {
        if (m_Tic > 1.1f)
        {
            DisposeObject();
            return;
        }

        m_Tic += Time.deltaTime / 2;

        CacheTransform.position = Vector3.Lerp(m_StartPos, new Vector3(m_StartPos.x, m_StartPos.y + m_UpY, m_StartPos.z), m_Tic);
        Color alpha = m_Color;
        alpha.a = Mathf.Lerp(alpha.a, 0, m_Tic);
        m_Text.color = alpha;
    }

    public void UpdateShow(string text, Vector3 pos, Color color)
    {
        m_Text.text = text;
        m_Text.color = color;

        m_StartPos = pos;
        m_Color = color;

        base.Show();
    }
}
