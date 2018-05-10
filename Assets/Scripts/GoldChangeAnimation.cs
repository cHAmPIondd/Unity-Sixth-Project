using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
public class GoldChangeAnimation : MonoBehaviour {
    [SerializeField]
    private float m_AnimationOffsetY=10;
    [SerializeField]
    private float m_Time = 1;
    [SerializeField]
    private GameObject m_TextPrefab;
    public void Play(int num)
    {
        var text=Instantiate(m_TextPrefab, transform).GetComponent<Text>();
        text.text =(num>0?"+":"") +num + "";
        text.transform.DOMoveY(transform.position.y + m_AnimationOffsetY, m_Time);
        DOTween.ToAlpha(() => text.color, (x) => text.color = x, 0, m_Time);
    }
}
