using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodapParty;

public class UIGachaPage : NLayer
{
    public enum kNOTIFY
    {
        ChangeAllAnimation
    }

    [SerializeField]
    private Animator            m_Animator;

    [SerializeField]
    private GameObject          m_ObjcetOfPopupUI;

    [SerializeField]
    private Transform           m_TransOfGrid;

    [SerializeField]
    private Button              m_Button;

    private List<CharacterData> m_ListOfCharacterData = new List<CharacterData>();

    private List<UICharacterCardSlot> m_ListOfGachaSlot = new List<UICharacterCardSlot>();

    private bool m_IsOpen;


    public override void DisposeObject()
    {
        GameManager.Instance.DisposeObjectList(m_ListOfGachaSlot);
        base.DisposeObject();
    }

    public override void Initialization()
    {
        base.Initialization();
        m_ObjcetOfPopupUI.SetActive(false);

        PoolManager.Instance.Create<UICharacterCardSlot>(Constants.kPREFAB_UI_LOBBY_UICHARACTER_CARD_SLOT, 1);
    }

    public override void OnEscapeEvent()
    {
        GameManager.Instance.Pop<UIGachaPage>();
    }

    public override void Notify(System.Enum value)
    {
        var noti = (kNOTIFY)Enum.Parse(typeof(kNOTIFY), value.ToString());
        if (noti != kNOTIFY.ChangeAllAnimation)
            return;
        
        ChangeAllAnimation();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < m_ListOfCharacterData.Count; i++)
        {
            var obj = PoolManager.Instance.Pop<UICharacterCardSlot>(m_TransOfGrid);
            var isDuplication = DataManager.Instance.AddCardData(m_ListOfCharacterData[i]);
            obj.UpdateUI(m_ListOfCharacterData[i], isDuplication);
            m_ListOfGachaSlot.Add(obj);
        }
    }

    public void OnTouch()
    {
        if (m_IsOpen)
            return;

        m_IsOpen = true;
        m_Animator.SetTrigger("Open");
        m_ObjcetOfPopupUI.SetActive(true);
        UpdateUI();
    }

    public void SetResultGachaData(List<CharacterData> characterDatas)
    {
        m_ListOfCharacterData = characterDatas;
    }

    public void EndOfOpenAnimation()
    {
        StartCoroutine(GachaCardCreateAnimation());
    }

    private IEnumerator GachaCardCreateAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < m_ListOfCharacterData.Count; i++)
        {
            m_ListOfGachaSlot[i].StartAnimation();
            yield return new WaitUntil(delegate() 
            {
                var v = m_ListOfGachaSlot[i].GetIsStart();
                return v;
            });
        }

        yield return new WaitForSeconds(0.5f);

        MakePopup();
    }

    private void MakePopup()
    {
        GameManager.Instance.Push<UIGachaPopup>(delegate (UIGachaPopup layer) 
        {
            var queue = new Queue<CharacterData>();
            m_ListOfCharacterData.ForEach(data => queue.Enqueue(data));
            layer.UpdateUI(queue);
        }, Constants.kPREFAB_UI_LOBBY_UIGACHA_POPUP);
    }

    private void ChangeAllAnimation()
    {
        m_ListOfGachaSlot.ForEach(slot => slot.StartChange());
        Invoke("ChangeTouchEvent" , 1.1f);
    }

    private void ChangeTouchEvent()
    {
        m_Button.onClick.RemoveAllListeners();
        m_Button.onClick.AddListener(OnEscapeEvent);
    }
}
