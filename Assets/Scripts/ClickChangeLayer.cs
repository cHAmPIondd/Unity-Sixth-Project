using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickChangeLayer : MonoBehaviour, IPointerDownHandler
{
    public void OnEnable()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }
}
