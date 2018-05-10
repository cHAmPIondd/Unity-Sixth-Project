using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ShrinkComponent : MonoBehaviour {

    [SerializeField]
    private Vector2 m_ShrinkSize;
    [SerializeField]
    private float m_ShrinkTime = 0.5f;

    private RectTransform m_RectTransform;
    private Vector2 m_InitialSize;
    private Tween m_Tween;
	// Use this for initialization
	void Start () {
        m_RectTransform = GetComponent<RectTransform>();
        m_InitialSize = m_RectTransform.sizeDelta;
	}
	public void Shrink()
    {
        if (m_RectTransform == null)
        {
            m_RectTransform = GetComponent<RectTransform>();
            m_InitialSize = m_RectTransform.sizeDelta;
        }
        if (m_Tween != null)
            m_Tween.Kill();
        m_Tween=DOTween.To(() => m_RectTransform.sizeDelta, (x) => m_RectTransform.sizeDelta = x,
            m_InitialSize-m_ShrinkSize, m_ShrinkTime);
    }
    public void Recover(DG.Tweening.Core.TweenCallback onComplete = null)
    {
        if (m_RectTransform == null)
        {
            m_RectTransform = GetComponent<RectTransform>();
            m_InitialSize = m_RectTransform.sizeDelta;
        }
        if (m_Tween != null)
            m_Tween.Kill();
        m_Tween=DOTween.To(() => m_RectTransform.sizeDelta, (x) => m_RectTransform.sizeDelta = x,
            m_InitialSize, m_ShrinkTime).OnComplete(onComplete);
    }
}
