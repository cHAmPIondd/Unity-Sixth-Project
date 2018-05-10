using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicBoxController : MonoBehaviour {
    [SerializeField]
    private AudioClip[] m_AudioClips;
    private AudioSource m_AudioSource;
    private int m_Index;
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }
    public void Next()
    {
        m_Index++;
        if (m_Index >= m_AudioClips.Length)
            m_Index = 0;
        m_AudioSource.clip = m_AudioClips[m_Index];
        m_AudioSource.Play();
    }
    public void Last()
    {
        m_Index--;
        if (m_Index < 0)
            m_Index = m_AudioClips.Length - 1;
        m_AudioSource.clip = m_AudioClips[m_Index];
        m_AudioSource.Play();
    }
    public void ValueChange(Slider slider)
    {
        m_AudioSource.volume = slider.value;
    }
}
