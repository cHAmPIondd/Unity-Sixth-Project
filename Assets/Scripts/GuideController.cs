using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
public class GuideController : MonoBehaviour ,IPointerClickHandler{
    [SerializeField]
    private CanvasGroup[] m_GuideWords;
    [SerializeField]
    private GameObject[] m_GuideTriangles;
    [SerializeField]
    private float m_Rate = 0.2f;
    [SerializeField]
    private Transform m_DialogTransform;
    [SerializeField]
    private Image m_WomanImage;
    [SerializeField]
    private Sprite m_WomanImageNewSprite;
    [SerializeField]
    private float m_WomanInitalOffsetX;
    [SerializeField]
    private float m_DialogInitalOffsetX;
    [SerializeField]
    private AudioClip m_ClickClip;
    [SerializeField]
    private AudioClip m_ShowClip;

    private int m_Index;
    private bool m_IsCurrentOver;
    private Text[] m_TextArray;
    private string[] m_StrArray;
    private Vector3 m_WomanTargetPos;
    private Vector3 m_DialogTargetPos;
    void Start()
    {
        m_WomanTargetPos = m_WomanImage.transform.position;
        m_DialogTargetPos = m_DialogTransform.position;
        m_WomanImage.transform.position = m_WomanTargetPos + new Vector3(m_WomanInitalOffsetX, 0 , 0);
        m_DialogTransform.position = m_DialogTargetPos + new Vector3(m_DialogInitalOffsetX, 0 , 0);
    }
    public void StartGuide()
    {
        StartCoroutine(Init());
    }
    private IEnumerator Init()
    {
        m_WomanImage.transform.DOMove(m_WomanTargetPos, 0.5f);
        yield return new WaitForSeconds(0.5f);
        m_DialogTransform.DOMove(m_DialogTargetPos, 0.5f);
        yield return new WaitForSeconds(0.7f);
        m_GuideWords[m_Index].alpha = 1;
        StartCoroutine("ShowText");
        GlobalObjectManager.Instance.SoundManager.Play(m_ShowClip,true);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_GuideWords[m_Index].alpha == 0)
            return;
        GlobalObjectManager.Instance.SoundManager.Play(m_ClickClip);
        if (m_IsCurrentOver)
        {
            if (m_Index >= m_GuideWords.Length - 1)
            {//引导结束
                gameObject.SetActive(false);
                GameManager.Instance.Gold = 20;
                GameManager.Instance.NextTask();
                GlobalObjectManager.Instance.TipController.Next(-1);
            }
            else
            {//翻页
                if (m_Index >= 0)
                    m_GuideWords[m_Index].alpha = 0;
                m_Index++;
                m_GuideWords[m_Index].alpha = 1;
                StartCoroutine("ShowText");
                GlobalObjectManager.Instance.SoundManager.Play(m_ShowClip,true);
                if(m_Index==m_GuideWords.Length-1)
                {
                    m_WomanImage.sprite = m_WomanImageNewSprite;
                }
            }
        }
        else
        {//快速显示
            for (int i = 0; i < m_TextArray.Length; i++)
            {
                m_TextArray[i].text = m_StrArray[i];
            }
            m_IsCurrentOver = true;
            m_GuideTriangles[m_Index].SetActive(true);
            StopCoroutine("ShowText");
            GlobalObjectManager.Instance.SoundManager.Stop(m_ShowClip);
        }
    }
    /// <summary>
    /// 显示过程
    /// </summary>
    private IEnumerator ShowText()
    {
        m_IsCurrentOver = false;
        m_TextArray = m_GuideWords[m_Index].GetComponentsInChildren<Text>();
        m_StrArray = new string[m_TextArray.Length];
        for (int i = 0; i < m_TextArray.Length; i++)
        {
            m_StrArray[i] = m_TextArray[i].text;
            m_TextArray[i].text = "";
        }
        for (int i = 0; i < m_TextArray.Length; i++)
        {
            bool isColorStart = false;
            for (int j = 0; j < m_StrArray[i].Length; j++)
            {
                if (m_StrArray[i][j] == '<')
                {
                    isColorStart = !isColorStart;
                    if(isColorStart)
                    {
                        m_TextArray[i].text += m_StrArray[i].Substring(j, 17);
                        m_TextArray[i].text += "</color>";
                        j += 16;
                        continue;
                    }
                    else
                    {
                        j += 7;
                        continue;
                    }
                }
                if (isColorStart)
                {
                    m_TextArray[i].text = m_TextArray[i].text.Substring(0, m_TextArray[i].text.Length - 8);
                }
                m_TextArray[i].text += m_StrArray[i][j]; 
                if (isColorStart)
                {
                    m_TextArray[i].text += "</color>";
                }
                yield return new WaitForSeconds(m_Rate);
            }
        }
        m_GuideTriangles[m_Index].SetActive(true);
        m_IsCurrentOver = true;
        GlobalObjectManager.Instance.SoundManager.Stop(m_ShowClip);
    }


}
