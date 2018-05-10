using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UpgradeItem : MonoBehaviour ,IPointerEnterHandler,IPointerExitHandler{
    [SerializeField]
    private bool m_IsZeroLevel;
    [SerializeField]
    private Text m_NameText;
    [SerializeField]
    private GameObject m_TriangleGameObject;
    [SerializeField]
    private Vector2 m_MaxExtendSize;
    [SerializeField]
    private float m_ExtendTime;
    [SerializeField]
    private AudioClip m_ButtonEnterClip;

    private RectTransform m_RectTransform;
    private Vector2 m_InitialSize;
    private int m_Index;
    private Action<int> m_EnterDelegate;
	// Use this for initialization
	void Start () {
        m_RectTransform = GetComponent<RectTransform>();
        m_InitialSize = m_RectTransform.sizeDelta;
	}
    public void Init(string name, int index, Action<int> enterDelegate)
    {
        m_NameText.text = name;
        m_Index = index;
        m_EnterDelegate = enterDelegate ;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GlobalObjectManager.Instance.SoundManager.Play(m_ButtonEnterClip);
        if (!m_IsZeroLevel)
            m_TriangleGameObject.SetActive(true);
        StopCoroutine("Shrink");
        StartCoroutine("Extend");
        m_EnterDelegate.Invoke(m_Index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!m_IsZeroLevel)
            m_TriangleGameObject.SetActive(false);
        StopCoroutine("Extend");
        StartCoroutine("Shrink");
    }
    private IEnumerator Extend()
    {
        while (true)
        {
            m_RectTransform.sizeDelta += (m_MaxExtendSize - m_InitialSize) / m_ExtendTime * Time.deltaTime;
            if (m_RectTransform.sizeDelta.x > m_MaxExtendSize.x)
            {
                m_RectTransform.sizeDelta = m_MaxExtendSize;
                break;
            }
            yield return 0;
        }
    }
    private IEnumerator Shrink()
    {
        while (true)
        {
            m_RectTransform.sizeDelta -= (m_MaxExtendSize - m_InitialSize) / m_ExtendTime * Time.deltaTime;
            if (m_RectTransform.sizeDelta.x < m_InitialSize.x)
            {
                m_RectTransform.sizeDelta = m_InitialSize;
                break;
            }
            yield return 0;
        }
    }
}
