using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
    [SerializeField]
    private bool m_AwakePlay = false;
    [SerializeField]
    private bool m_IsLoop = false;
    [SerializeField]
    private Sprite[] m_Sprites;
    [SerializeField]
    private float m_Time = 1f;
    private Image m_Image;
    // Use this for initialization
    void Start()
    {
        m_Image = GetComponent<Image>();
        if (m_AwakePlay)
            StartCoroutine(PlayAnimation());
    }

    public IEnumerator PlayAnimation()
    {
        m_Image = GetComponent<Image>();
        do
        {
            for (int i = 0; i < m_Sprites.Length; i++)
            {
                m_Image.sprite = m_Sprites[i];
                yield return new WaitForSeconds(m_Time / m_Sprites.Length);
            }
        } while (m_IsLoop);
    }
}
