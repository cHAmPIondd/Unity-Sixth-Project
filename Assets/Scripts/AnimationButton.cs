using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class AnimationButton : MonoBehaviour ,IPointerClickHandler{
    [SerializeField]
    private Vector2 m_MoveVector2;
    [SerializeField]
    private float m_Time;
    [SerializeField]
    private AudioClip m_ClickClip;
    private Vector2 m_InitalPos;
    void Start()
    {
        m_MoveVector2 = m_MoveVector2 * Screen.height / 768;
        m_InitalPos = transform.position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalObjectManager.Instance.SoundManager.Play(m_ClickClip);
        transform.DOLocalMove(m_MoveVector2, m_Time).onComplete =()=>
        {
            transform.DOMove(m_InitalPos, m_Time);
        };
    }
}
