using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    private AudioSource[] m_AudioSources;
    void Start()
    {
        m_AudioSources=GetComponents<AudioSource>();
    }
    public void Play(AudioClip clip,bool isLoop=false)
    {
        for(int i=0;i<m_AudioSources.Length;i++)
        {
            if(!m_AudioSources[i].isPlaying)
            {
                m_AudioSources[i].clip = clip;
                m_AudioSources[i].loop = isLoop;
                m_AudioSources[i].Play(); 
                break; 
            }
        }
    }
    public void Stop(AudioClip clip)
    {
        for (int i = 0; i < m_AudioSources.Length; i++)
        {
            if (m_AudioSources[i].clip==clip)
            {
                m_AudioSources[i].Stop();
                break;
            }
        }
    }
}
