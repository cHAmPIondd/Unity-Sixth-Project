using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragPart : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IBeginDragHandler,IDragHandler,IEndDragHandler{
    public int FinalIndex { get; set; }
    [SerializeField,Range(0,5)]
    private int m_Index;

    private Vector2 m_Offset;
    private Vector2 m_InitialPos;
    void Start()
    {
        m_InitialPos = transform.position;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GlobalObjectManager.Instance.DragPartController.DragSignOn(m_Index);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!eventData.dragging)
            GlobalObjectManager.Instance.DragPartController.DragSignOff(m_Index);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        GlobalObjectManager.Instance.SoftwareFileController.GetComponent<CanvasGroup>().alpha=0;
        GlobalObjectManager.Instance.TaskController.GetComponent<CanvasGroup>().alpha=0;
        m_Offset = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(eventData.pressPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(eventData.position) + m_Offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GlobalObjectManager.Instance.SoftwareFileController.GetComponent<CanvasGroup>().alpha = 1;
        GlobalObjectManager.Instance.TaskController.GetComponent<CanvasGroup>().alpha = 1;
        var hardware=GlobalObjectManager.Instance.HardwareController.AllHardwareArray[FinalIndex];
        var hardwareList = GameManager.Instance.CurrentRobot.HardwareList;
        for (int i = 0; i < hardwareList.Count; i++)
        {
            if (hardwareList[i].Hardware == hardware)
            {
                transform.position = m_InitialPos;
                return;
            }
        }
        if(GlobalObjectManager.Instance.DragPartController.IsPointerInner&&GameManager.Instance.CurrentRobot.HardwareList.Count<16)
        {
            StartCoroutine(GlobalObjectManager.Instance.DragPartController.FinishDragPart(this));
        }
        else
        {
            transform.position = m_InitialPos;
        }
    }

}
