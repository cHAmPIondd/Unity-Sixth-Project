using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPage : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    private Vector2 m_Offset;

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_Offset = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(eventData.pressPosition);
        transform.SetSiblingIndex(transform.parent.childCount-1);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(eventData.position) + m_Offset;
    }

}
