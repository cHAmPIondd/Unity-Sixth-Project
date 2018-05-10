using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class RightThreeButton : MonoBehaviour ,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]
    private float m_OffsetX = -10;
    [SerializeField]
    private float m_OffsetTime = 0.2f;
    [SerializeField]
    private GameObject m_RelateGameObject;
    [SerializeField]
    private bool m_IsCloseBackgroundPages = false;
    [SerializeField]
    private AudioClip m_HoverClip;
    [SerializeField]
    private AudioClip m_DisplayClip;

    private RectTransform m_RectTransform;
    private float m_InitialX;
    void Start()
    {
        m_RectTransform=GetComponent<RectTransform>();
        m_InitialX = m_RectTransform.position.x;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GlobalObjectManager.Instance.SoundManager.Play(m_HoverClip);
        m_RectTransform.DOMoveX(m_InitialX + m_OffsetX, m_OffsetTime);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_RectTransform.DOMoveX(m_InitialX, m_OffsetTime);
    }

    public void Click()
    {
        GlobalObjectManager.Instance.SoundManager.Play(m_DisplayClip);
        if (m_IsCloseBackgroundPages)
        {
            GameManager.Instance.CloseBackgroundPages();
        }
        m_RectTransform.DOMoveX(m_InitialX, m_OffsetTime);
        gameObject.SetActive(false);
        m_RelateGameObject.SetActive(true);
    }
    public void ClickBack()
    {
        if (m_IsCloseBackgroundPages)
        {
            GlobalObjectManager.Instance.ShelteringLayer.SetActive(false);
        }
        m_RelateGameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
