using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class AudioClipPlay : MonoBehaviour,IPointerEnterHandler {
    [SerializeField]
    private AudioClip m_AudioClip;
    [SerializeField]
    private AudioClip m_PointerEnterClip;
    public void Play()
    {
        GlobalObjectManager.Instance.SoundManager.Play(m_AudioClip);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GlobalObjectManager.Instance.SoundManager.Play(m_PointerEnterClip);
    }
}
