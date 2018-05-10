using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class DragDrawer : MonoBehaviour,IPointerEnterHandler, IDragHandler ,IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform m_HardwareCaseTransform;
 //   [SerializeField]
  //  private float m_AutoDragTime = 1;
    [SerializeField]
    private float m_Gravity = 0.1f;
    [SerializeField]
    private float m_Width = 200;

    private RectTransform m_RectTransform;
    private float m_OpenX;
    private float m_CloseX;
    private float m_OffsetX;
    private float m_LastPosX;
    void Start()
    {
        m_OpenX = m_HardwareCaseTransform.position.x;
        m_CloseX = m_OpenX-m_Width;
        m_HardwareCaseTransform.position = new Vector2(m_CloseX, m_HardwareCaseTransform.position.y);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        m_OffsetX = m_HardwareCaseTransform.position.x -  Camera.main.ScreenToWorldPoint(eventData.pressPosition).x;
        GlobalObjectManager.Instance.HardwareController.CloseMenu();
    }
    public void OnDrag(PointerEventData eventData)
    {
        m_LastPosX = m_HardwareCaseTransform.position.x;
        var curX = Mathf.Clamp(Camera.main.ScreenToWorldPoint(eventData.position).x + m_OffsetX, m_CloseX, m_OpenX);
        m_HardwareCaseTransform.position = new Vector2(curX, m_HardwareCaseTransform.position.y);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(AutoMove());
        /*
        if(Mathf.Abs(m_HardwareCaseTransform.position.x-m_OpenX)>Mathf.Abs(m_HardwareCaseTransform.position.x-m_CloseX))
        {
            m_AutoDragTween=m_HardwareCaseTransform.DOMoveX(m_CloseX, m_AutoDragTime);
        }
        else
        {
            m_AutoDragTween=m_HardwareCaseTransform.DOMoveX(m_OpenX, m_AutoDragTime);
        }*/
    }
    private IEnumerator AutoMove()
    {
        var rate = (m_HardwareCaseTransform.position.x - m_LastPosX);
        if (m_OpenX - m_HardwareCaseTransform.position.x<70)
        {
            rate = 8;
        }
        while(true)
        {
            var curX = m_HardwareCaseTransform.position.x + rate;
            if (curX < m_CloseX || curX > m_OpenX)
            {
                m_HardwareCaseTransform.position = new Vector2(Mathf.Clamp(curX, m_CloseX, m_OpenX), m_HardwareCaseTransform.position.y);
                yield break;
            }
            m_HardwareCaseTransform.position = new Vector2(curX, m_HardwareCaseTransform.position.y);
            yield return 0;
            rate -= m_Gravity * Time.deltaTime;
        }
    }
    private IEnumerator CloseProcess()
    {
        var rate = -1f;
        while (true)
        {
            var curX = m_HardwareCaseTransform.position.x + rate;
            if (curX < m_CloseX || curX > m_OpenX)
            {
                m_HardwareCaseTransform.position = new Vector2(Mathf.Clamp(curX, m_CloseX, m_OpenX), m_HardwareCaseTransform.position.y);
                yield break;
            }
            m_HardwareCaseTransform.position = new Vector2(curX, m_HardwareCaseTransform.position.y);
            yield return 0;
            rate -= m_Gravity * Time.deltaTime;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void Close()
    {
        StartCoroutine(CloseProcess());
        GlobalObjectManager.Instance.HardwareController.CloseMenu();
    }
}