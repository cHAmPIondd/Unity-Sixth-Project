using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class TipController : MonoBehaviour ,IPointerEnterHandler,IPointerExitHandler{
    public int TipIndex { get; set; }
    [SerializeField]
    private GameObject[] m_Tips;
    [SerializeField]
    private float m_OpenOffsetX = -213;
    [SerializeField]
    private float m_HideOffsetX = -60;
    [SerializeField]
    private float m_OpenTime = 0.7f;
    [SerializeField]
    private float m_WaitTime = 3f;
    [SerializeField]
    private AudioClip m_OpenClip;

    private Vector2 m_InitialPos;
    private bool m_IsOpen;
	// Use this for initialization
	void Start () {
        TipIndex = -1;
        m_InitialPos = transform.position;
	}
    public void Next(int index)
    {
        if (index == TipIndex)
        {
            if (TipIndex >= m_Tips.Length - 1)
            {
                TipIndex++;
                StopCoroutine("AutoClose");
                Close();
                return;
            }
            if (TipIndex>=0)
                m_Tips[TipIndex].SetActive(false);
            TipIndex++;
            m_Tips[TipIndex].SetActive(true);
            Open();
            StopCoroutine("AutoClose");
            StartCoroutine("AutoClose");

        }
    }
    public void Open()
    {
        m_IsOpen = true;
        transform.DOMoveX(m_InitialPos.x + m_OpenOffsetX, m_OpenTime);
        GlobalObjectManager.Instance.SoundManager.Play(m_OpenClip);
    }
    public void Hide()
    {
        m_IsOpen = false;
        transform.DOMoveX(m_InitialPos.x + m_HideOffsetX, m_OpenTime);
    }
    public void Close()
    {
        m_IsOpen = false;
        transform.DOMoveX(m_InitialPos.x , m_OpenTime);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopCoroutine("AutoClose");
        if(!m_IsOpen)
        {
            Open();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine("AutoClose");
        StartCoroutine("AutoClose");
    }
    private IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(m_WaitTime);
        Hide();
    }

}
