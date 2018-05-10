using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPartController : MonoBehaviour
{
    [SerializeField]
    private ShrinkComponent[] m_DragSignShrinkComponents;
    [SerializeField]
    private Transform m_MaskTransform;
    [SerializeField]
    private GameObject m_DragPartTrangle;
    [SerializeField]
    private float m_DragPartDropTime=0.5f;
    [SerializeField]
    private AudioClip m_DragSignOnClip;
    [SerializeField]
    private AudioClip m_DragPartDropClip;
    public bool IsPointerInner { get; private set; }
    private int m_CurrentDragSignIndex=-1;
    void OnTriggerEnter2D(Collider2D other)
    {
        IsPointerInner = true;
        if (m_CurrentDragSignIndex != -1)
        {
            m_DragSignShrinkComponents[m_CurrentDragSignIndex].Shrink();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        IsPointerInner = false;
        if (m_CurrentDragSignIndex != -1)
        {
            m_DragSignShrinkComponents[m_CurrentDragSignIndex].Recover();
        }
    }
    public void DragSignOn(int index)
    {
        if (m_CurrentDragSignIndex == -1)
        {
            m_CurrentDragSignIndex = index;
            m_DragSignShrinkComponents[index].gameObject.SetActive(true);
            m_DragPartTrangle.SetActive(false);
            GlobalObjectManager.Instance.SoundManager.Play(m_DragSignOnClip);
        }
    }
    public void DragSignOff(int index)
    {
        m_CurrentDragSignIndex =-1;
        m_DragSignShrinkComponents[index].gameObject.SetActive(false);
        m_DragPartTrangle.SetActive(true);
    }
    public IEnumerator FinishDragPart(DragPart dragPart)
    {
        GlobalObjectManager.Instance.TipController.Next(3);
        if (m_CurrentDragSignIndex!=-1)
            m_DragSignShrinkComponents[m_CurrentDragSignIndex].Recover();
        DragSignOff(dragPart.FinalIndex / 3);
        dragPart.transform.parent=m_MaskTransform;
        dragPart.transform.position = m_MaskTransform.position;
        dragPart.GetComponent<Rigidbody2D>().gravityScale = 1;
        yield return new WaitForSeconds(m_DragPartDropTime);
        GlobalObjectManager.Instance.SoundManager.Play(m_DragPartDropClip);
        GlobalObjectManager.Instance.HardwareController.Reset();
        GlobalObjectManager.Instance.MachineController.AddHardware(dragPart.FinalIndex);
        Destroy(dragPart.gameObject);
    }
}
