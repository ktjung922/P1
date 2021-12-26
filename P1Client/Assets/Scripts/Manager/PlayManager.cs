using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NodapParty;

public class PlayManager : SingletonGameObject<PlayManager>
{
    private Camera MainCamera;

    private Camera UICamera;    

    private Camera ParticleCamera;

    private Transform PlayBG;

    private IGBoss      m_Boss;

    private bool        m_IsAllCharacterInit;

    private List<CharacterData>                     m_ListOfDeckCharacter = new List<CharacterData>();

    private Dictionary<ElementalData.kTYPE, int>    m_DicOfSynergy = new Dictionary<ElementalData.kTYPE, int>();

    private List<IGBaseCharacter>                   m_ListOfCharacterObject = new List<IGBaseCharacter>();

    private Vector3[] m_PosOfCharacters = 
    { 
        new Vector3(-6.5f, 2.5f, 0f), 
        new Vector3(-6.5f, -2.5f, 0f),
        new Vector3(-2.5f, 2.5f, 0f),
        new Vector3(-2.5f, -2.5f, 0f)
    };

    private Vector3 m_PosOfBoss = new Vector3(4f, 0f, 0f);

    private Dictionary<ElementalData.kTYPE, ElementalData.kTYPE> m_DicOfGiveSynergy = new Dictionary<ElementalData.kTYPE, ElementalData.kTYPE>()
    {
        { ElementalData.kTYPE.SwimmingPool, ElementalData.kTYPE.Water },
        { ElementalData.kTYPE.Attractive, ElementalData.kTYPE.Love }
    };

    public void Initialization()
    {
        PlayBG = GameObject.FindWithTag("PlayBG")?.transform;
        PlayBG.gameObject.SetActive(false);

        MainCamera = GameObject.FindWithTag("MainCamera")?.GetComponent<Camera>();
        UICamera = GameObject.FindWithTag("UICamera")?.GetComponent<Camera>();
        ParticleCamera = GameObject.FindWithTag("ParticleCamera")?.GetComponent<Camera>();

        GameManager.Instance.Push<UIInGame>((layer)=>
        {
            layer.Hide();
        },Constants.kPREFAB_INGAME_UI_INGAME);
        PoolManager.Instance.Create<IGBaseCharacter>(Constants.kPREFAB_INGAME_IG_BASE_CHARACTER, 1);
        PoolManager.Instance.Create<IGKyungtae>(Constants.kPREFAB_CHARAcTER_IG_KYUNGTAE, 1);
        PoolManager.Instance.Create<IGBoss>(Constants.kPREFAB_INGAME_IG_BOSS, 1);
    }

#region Deck.

    public void ClearDeckData()
    {
        m_ListOfDeckCharacter.Clear();
        m_DicOfSynergy.Clear();
    }

    ///<summery>
    /// 기태3성 조건 체크용, 덱에 포함된 딜러 수 반환.
    ///</summery>
    public int GetCountOfDPSInDeck()
    {
        var result = PlayManager.Instance.Deck.FindAll(data => (data.TYPE == CharacterData.kTYPE.ATTACK || data.TYPE == CharacterData.kTYPE.MAGIC) && data.INDEX != Constants.kITAE_THREE_INDEX).Count;
        return result;
    }

    public void UpdateSynergy(List<ElementalData> list, bool isDelete, int count = 1)
    {
        bool isTarget = false;
        ElementalData target = new ElementalData();
        list.ForEach(data =>
        {
            UpdateSynergy(data.SYNERGY, isDelete, count);

            if (data.isGiveSynergyType())
            {
                target.SYNERGY = data.SYNERGY;
                isTarget = true;
            }
        });
        
        if (!isTarget)
        {
            CheckGiveSynergy(isDelete);
            return;
        }

        CheckGiveSynergy(isDelete, target);
    }

    public void UpdateSynergy(ElementalData.kTYPE type, bool isDelete, int count = 1)
    {
        if (isDelete)
        {
            if (!m_DicOfSynergy.ContainsKey(type))
                return;
                
            if (m_DicOfSynergy[type] == count)
            {
                m_DicOfSynergy.Remove(type);
            }
            else 
            {
                m_DicOfSynergy[type] -= count;
            }
        }
        else
        {
            if (m_DicOfSynergy.ContainsKey(type))
            {
                m_DicOfSynergy[type] += count;
            }
            else
            {
                m_DicOfSynergy.Add(type, count);
            }
        }
    }

    public void CheckGiveSynergy(bool isDelete, ElementalData target = null)
    {
        var curCount = m_ListOfDeckCharacter.Count;
        var preCount = isDelete ? curCount + 1 : curCount - 1;
        if (target == null)
            target = new ElementalData();

        if (target.isGiveSynergyType())
        {
            var count = isDelete ? preCount : curCount;

            UpdateSynergy(m_DicOfGiveSynergy[target.SYNERGY], isDelete, count);
        }

        if (m_DicOfSynergy.ContainsKey(ElementalData.kTYPE.SwimmingPool) && target.SYNERGY != ElementalData.kTYPE.SwimmingPool)
        {
            UpdateSynergy(ElementalData.kTYPE.Water, true, preCount);
            UpdateSynergy(ElementalData.kTYPE.Water, false, curCount);
        }

        if (m_DicOfSynergy.ContainsKey(ElementalData.kTYPE.Attractive) && target.SYNERGY != ElementalData.kTYPE.Attractive)
        {
            UpdateSynergy(ElementalData.kTYPE.Love, true, preCount);
            UpdateSynergy(ElementalData.kTYPE.Love, false, curCount);
        }
    }

#endregion Deck.

#region Play.

    public void PlayGame()
    {
        GameManager.Instance.Hide<UIDeck>();
        GameManager.Instance.Hide<UILobby>();
        GameManager.Instance.Show<UIInGame>();
        PlayBG.gameObject.SetActive(true);
        m_IsAllCharacterInit = false;

        InitPlay();
    }

    public void EndPlayGame()
    {
        GameManager.Instance.Show<UIDeck>();
        GameManager.Instance.Show<UILobby>();
        GameManager.Instance.Hide<UIInGame>();
        PlayBG.gameObject.SetActive(false);

        DisposePlay();
    }

    public void InitPlay()
    {
        if (m_ListOfDeckCharacter.Count == 0)
        {
            EndPlayGame();
            return;
        }

        m_Boss = PoolManager.Instance.Pop<IGBoss>(PlayBG);
        m_Boss.Init();
        m_Boss.transform.position = m_PosOfBoss;

        for (int i = 0; i < m_ListOfDeckCharacter.Count; i++)
        {
            var obj = PopCharacterPrefabs(m_ListOfDeckCharacter[i]);
            obj.transform.position = m_PosOfCharacters[i];
            obj.InitObject(m_ListOfDeckCharacter[i]);
            m_ListOfCharacterObject.Add(obj);
        }

        StartCoroutine(TimeCheck());
    }

    public void DisposePlay()
    {
        m_Boss.DisposeObject();
        GameManager.Instance.DisposeObjectList(m_ListOfCharacterObject);
        m_ListOfCharacterObject.Clear();
    }

    public IGBaseCharacter PopCharacterPrefabs(CharacterData data)
    {
        IGBaseCharacter result = null;
        switch(data.GROUP)
        {
            case CharacterData.kGROUP.Kyungtae: 
                result = PoolManager.Instance.Pop<IGKyungtae>(PlayBG);
                break;
            default: 
                result = PoolManager.Instance.Pop<IGBaseCharacter>(PlayBG);
                break;
        }
        return result;
    }

    public void AttackBoss(int damage, int charIndex, ParticleManager.kPARTICLE particle = ParticleManager.kPARTICLE.None)
    {
        if (m_Boss == null)
            return;
        
        m_Boss.Demaged(damage, charIndex, particle);
    }

#endregion Play.

#region Time.

    public IEnumerator TimeCheck()
    {
        yield return new WaitUntil(() => 
        {
            var count = 0;
            m_ListOfCharacterObject.ForEach(data => 
            {
                if (data.isEndIdle)
                {
                    count++;
                }
            });

            if (count == m_ListOfCharacterObject.Count)
                return true;
            else
                return false;
        });

        m_IsAllCharacterInit = true;
        GameManager.Instance.Notify<UIInGame>(UIInGame.kNOTIFY.StartTimer);
    }

#endregion Time.

#region Property.

    public List<CharacterData> Deck
    {
        get { return m_ListOfDeckCharacter; }
        set { m_ListOfDeckCharacter = value; }
    }

    public Dictionary<ElementalData.kTYPE, int> Synergy
    {
        get { return m_DicOfSynergy; }
        set { m_DicOfSynergy = value; }
    }

    public bool canTimeFlow 
    { 
        get 
        { 
            if (!m_IsAllCharacterInit)
                return false;
            
            return true;
        } 
    }

#endregion Property.
}
