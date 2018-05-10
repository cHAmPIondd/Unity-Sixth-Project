using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragButton : MonoBehaviour, IDragHandler, IPointerEnterHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private Transform m_ButtonTransform;
    [SerializeField]
    private Transform m_ContentTransform;

    private Vector2 m_ButtonPosition;
    private float m_MaxSqrtMagnitude;
    private Vector2 m_ButtonOffset;
    private Vector2 m_ContentOffset;
    private bool m_IsOpen;
	// Use this for initialization
	void Start () {
        m_ButtonPosition = m_ButtonTransform.position;
        m_MaxSqrtMagnitude = Screen.width*Screen.width/100;
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (m_IsOpen)
        {
            m_ContentOffset = (Vector2)m_ContentTransform.position - eventData.pressPosition;
             m_ButtonOffset = Vector2.zero;
        }
        else
        {
            m_ButtonOffset = (Vector2)m_ButtonTransform.position - eventData.pressPosition;
            m_ContentOffset = Vector2.zero;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (Vector2.SqrMagnitude(eventData.position - m_ButtonPosition) > m_MaxSqrtMagnitude)
        {
            if (!m_IsOpen)
            {
                m_IsOpen = true;
                m_ButtonTransform.gameObject.SetActive(false);
                m_ContentTransform.gameObject.SetActive(true);
            }
        }
        else
        {
            if(m_IsOpen)
            {
                m_IsOpen = false;
                m_ButtonTransform.gameObject.SetActive(true);
                m_ContentTransform.gameObject.SetActive(false);
            }
        }
        m_ButtonTransform.position = eventData.position + m_ButtonOffset;
        m_ContentTransform.position = eventData.position + m_ContentOffset;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!m_IsOpen && Vector2.SqrMagnitude(eventData.position - m_ButtonPosition) < m_MaxSqrtMagnitude)
        {
            m_ButtonTransform.position = m_ButtonPosition;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void ClickClose()
    {
        m_IsOpen = false;
        m_ButtonTransform.gameObject.SetActive(true);
        m_ContentTransform.gameObject.SetActive(false);
        m_ButtonTransform.position = m_ButtonPosition;
    }
}
