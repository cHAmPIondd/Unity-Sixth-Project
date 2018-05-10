using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HardwareController : MonoBehaviour {
    public Hardware[] AllHardwareArray { get; private set; }

    [SerializeField]
    private Transform m_CaseBodyTransform;
    [SerializeField]
    private GameObject m_MenuGameObject;
    [SerializeField]
    private Text[] m_NameTexts;
    [SerializeField]
    private Text[] m_Price1Texts;
    [SerializeField]
    private Text[] m_Price2Texts;
    [SerializeField]
    private RectTransform[] m_DoorRectTransforms;
    [SerializeField]
    private GameObject[] m_PartPrefabs;
    [SerializeField]
    private float m_WaitDoorTime = 0.3f;
    [SerializeField]
    private float m_OpenDoorTime=0.3f;
    [SerializeField]
    private AudioClip m_PartDisplayClip;
    [SerializeField]
    private AudioClip m_DoorHoverClip;

    private int m_FixedIndex = -1;
    private int m_SelectIndex = -1;
    private Transform m_CurrentPartTransform;
    public void Init()
    {
        AllHardwareArray = Resources.LoadAll<Hardware>("Hardwares");
    }
	
    public void OnPointEnter(int index)
    {
        if(m_FixedIndex==-1)
        {
            GlobalObjectManager.Instance.SoundManager.Play(m_DoorHoverClip);
            m_MenuGameObject.SetActive(true);
            UpdateInformation(index);
        }
    }
    public void OnPointExit(int index)
    {
        if(m_FixedIndex==-1)
        {
            m_MenuGameObject.SetActive(false);
        }
    }
    public void OnPointClick(int index)
    {
        if (m_FixedIndex == index)
            m_FixedIndex = -1;
        else
        {
            if (m_SelectIndex != -1)//选择另一个小抽屉
            {
                Reset();
            }
            m_FixedIndex = index;
            UpdateInformation(index);
        }
    }
    private void UpdateInformation(int index)
    {
        m_MenuGameObject.SetActive(true);
        for(int i=0;i<3;i++)
        {
            m_NameTexts[i].text=AllHardwareArray[index * 3 + i].Name;
            m_Price1Texts[i].text = AllHardwareArray[index * 3 + i].Cost[0]+"";
            m_Price2Texts[i].text = AllHardwareArray[index * 3 + i].Cost[1]+"";
        }
    }
    public void CloseMenu()
    {
        Reset();
    }
    public void SelectPart(int index)
    {
        GlobalObjectManager.Instance.SoundManager.Play(m_PartDisplayClip);
        StartCoroutine(OpenDoor(index));
    }
    private IEnumerator OpenDoor(int index)
    {
        m_SelectIndex = index;
        var door = m_DoorRectTransforms[m_FixedIndex];
        m_CurrentPartTransform = Instantiate(m_PartPrefabs[m_FixedIndex], m_CaseBodyTransform).GetComponent<RectTransform>();
        m_CurrentPartTransform.SetSiblingIndex(0);
        m_CurrentPartTransform.position = door.position;
        m_CurrentPartTransform.GetComponent<DragPart>().FinalIndex = m_FixedIndex * 3 + m_SelectIndex;
        m_MenuGameObject.SetActive(false);
        yield return new WaitForSeconds(m_WaitDoorTime);
        door.DOLocalMoveX(-door.rect.width, m_OpenDoorTime).onComplete = () => m_CurrentPartTransform.parent = transform;
    }
    public void Reset()
    {
        if (m_FixedIndex != -1)
        {
            var door = m_DoorRectTransforms[m_FixedIndex];
            door.DOLocalMoveX(0, m_OpenDoorTime);
            if (m_CurrentPartTransform != null)
            {
                m_CurrentPartTransform.SetParent(m_CaseBodyTransform);
                m_CurrentPartTransform.SetSiblingIndex(0);
                Destroy(m_CurrentPartTransform.gameObject, m_OpenDoorTime);
            }
        }
        m_MenuGameObject.SetActive(false);
        m_SelectIndex = -1;
        m_FixedIndex = -1;
    }
}
