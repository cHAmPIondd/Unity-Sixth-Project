 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FeedbackController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_RewardGameObject;
    [SerializeField]
    private GameObject m_RewardFX;
    [SerializeField]
    private Text m_RewardText;
    [SerializeField]
    private float m_RewardScaleTime=0.5f;
    [SerializeField]
    private LayoutElement m_BottomElement;
    [SerializeField]
    private LayoutElement[] m_RequirementBoxElements;
    [SerializeField]
    private Image[] m_Images;
    [SerializeField]
    private GameObject[] m_FinishSigns;
    [SerializeField]
    private LayoutElement[] m_TextLayoutElements;
    [SerializeField]
    private Text[] m_TaskTexts;
    [SerializeField]
    private Text m_FeedbackText;
    [SerializeField]
    private GameObject[] m_ButtonArray;
    [SerializeField]
    private Image m_PortraitImage;
    [SerializeField]
    private float m_MaxExtendHeight = 70;
    [SerializeField]
    private float m_ExtendRate = 4;
    [SerializeField]
    private AudioClip m_ButtonEnterClip;
    [SerializeField]
    private AudioClip m_SubmitClip;
    [SerializeField]
    private AudioClip m_GetMoneyClip;

    private RectTransform m_RectTransform;
    private Rect m_InitialRect;
    private float m_CurrentBasicHeight;
    private float[] m_CurrentExtendHeights;
    private Color m_InitialColor;
	// Use this for initialization
	public void Init () 
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_InitialRect = new Rect(m_RectTransform.position.x,m_RectTransform.position.y,m_RectTransform.sizeDelta.x,m_RectTransform.sizeDelta.y);
        m_InitialColor = m_Images[0].color;
        m_CurrentExtendHeights = new float[m_TextLayoutElements.Length];
	}
    void Update()
    {
        float totalExtendHeight = m_CurrentBasicHeight;
        for (int i = 0; i < GlobalObjectManager.Instance.TaskController.TaskNum; i++)
        {//计算背景框高度
            totalExtendHeight += m_CurrentExtendHeights[i];
            m_TextLayoutElements[i].minHeight = m_CurrentExtendHeights[i];
        }
      //  m_RectTransform.position = new Vector2(m_InitialRect.x, m_InitialRect.y - totalExtendHeight / 2);
        m_RectTransform.sizeDelta = new Vector2(m_InitialRect.width,  totalExtendHeight);
    }
    public IEnumerator SubmitTask()
    {
        Reset();
        var taskController = GlobalObjectManager.Instance.TaskController;
        m_CurrentBasicHeight = taskController.TaskNum * m_RequirementBoxElements[0].minHeight + m_InitialRect.height;
        var robotSoftwares = GameManager.Instance.CurrentRobot.SoftwareList;
        m_PortraitImage.sprite = taskController.PortraitSprite;
        for (int i = 0; i < taskController.TaskNum; i++)
        {
            //打开说明按钮
            m_RequirementBoxElements[i].gameObject.SetActive(true);
            //设置要求说明
            m_TaskTexts[i].text = taskController.RequirementDescribes[i];
        }
        int reward = 0;
        for (int i = 0; i < taskController.TaskNum; i++)
        {
            //判断要求是否已完成或找到对应反馈
            var requirementSoftwares = taskController.Requirements[i].SoftwareList;
            int[] levels = new int[requirementSoftwares.Length];
            for (int j = 0; j < requirementSoftwares.Length; j++)
            {//提取所要求软件robot里对应的level
                for (int k = 0; k < robotSoftwares.Count; k++)
                {
                    if (requirementSoftwares[j].Software.name.Equals(robotSoftwares[k].Software.name, System.StringComparison.CurrentCultureIgnoreCase))
                    {
                        levels[j] = robotSoftwares[k].Level;
                    }
                }
            }
            var feedbackList = taskController.Requirements[i].FeedbackList;
            for (int j = 0; j < feedbackList.Length; j++)//第j个反馈，逐个看符不符合
            {
                var codes = feedbackList[j].EachSoftwareLevel.Split(',');
                for (int k = 0; k < requirementSoftwares.Length; k++)//（1,1,1,1）第k个软件
                {
                    if (codes[k] == "x")
                    {
                        continue;
                    }
                    else
                    {
                        if (codes[k][0] == '!')
                        {
                            if (int.Parse(codes[k].Substring(1, codes[k].Length - 1)) != levels[k])
                            {
                                continue;
                            }
                        }
                        else
                        {
                            if (int.Parse(codes[k]) == levels[k])
                            {
                                continue;
                            }
                        }
                    }
                    goto BreakAndSkip;//这个反馈不符合
                }
                //符合
                if (m_FeedbackText.text == "")
                    m_FeedbackText.text = "“" + feedbackList[j].FeedbackText[Random.Range(0, feedbackList[j].FeedbackText.Length)] + "”";
                reward += (int)(feedbackList[j].FinishPercentage * GameManager.Instance.RequirementRewards[i]);
                goto BreakAndSkip2;//没完成
            //不符合
            BreakAndSkip: ;
            }
            //这个任务找不到反馈，所以已完成
            m_FinishSigns[i].SetActive(true);
            m_TaskTexts[i].GetComponent<TextLine>().CreateLink();
            reward += GameManager.Instance.RequirementRewards[i];
        //没完成
        BreakAndSkip2: ;
        }
        //所有任务完成
        if (reward > GameManager.Instance.CurrentRobot.Cost || m_FeedbackText.text == "")
        {
            if (m_FeedbackText.text == "")
            {
                m_FeedbackText.text = "“你真是我见过的最完美，最厉害的AI设计师！！！”";
            }
            m_ButtonArray[2].SetActive(true);
        }
        else
        {
            m_ButtonArray[0].SetActive(true);
            m_ButtonArray[1].SetActive(true);
        }
        m_RewardText.text = "获得" + reward + "元！";
        GameManager.Instance.CloseBackgroundPages();
        GlobalObjectManager.Instance.SoundManager.Play(m_SubmitClip);
        m_RewardGameObject.SetActive(true);
        m_RewardGameObject.transform.localScale = Vector3.zero;
        m_RewardGameObject.transform.DOScale(Vector3.one, m_RewardScaleTime).onComplete = () =>Instantiate(m_RewardFX, m_RewardGameObject.transform);
        yield return new WaitForSeconds(m_SubmitClip.length/2);
        GlobalObjectManager.Instance.SoundManager.Play(m_GetMoneyClip);
        GameManager.Instance.Gold += reward;
    }
    public void ClickOpenFeedback()
    {
        m_RewardGameObject.SetActive(false);
        gameObject.SetActive(true);
    }
    public void EnterRequirementButton(int index)
    {
        GlobalObjectManager.Instance.SoundManager.Play(m_ButtonEnterClip);
        StartCoroutine(Extend(index));
        m_Images[index].color = m_InitialColor*0.8f;
    }
    public void ExitRequirementButton(int index)
    {
        StartCoroutine(Shrink(index));
        m_Images[index].color = m_InitialColor;
    }
    public void ClickNextTask()
    {
        Reset();
        gameObject.SetActive(false);
        m_RewardGameObject.SetActive(false);
        GameManager.Instance.NextTask();
    }
    public void ClickImproveAI()
    {
        Reset();
        gameObject.SetActive(false);
        GlobalObjectManager.Instance.ShelteringLayer.SetActive(false);
        GameManager.Instance.IsDone = false;
    }
    private void Reset()
    {
        for(int i=0;i<m_CurrentExtendHeights.Length;i++)
        {
            m_CurrentExtendHeights[i] = 0;
            m_TextLayoutElements[i].minHeight = 0;
            m_RequirementBoxElements[i].gameObject.SetActive(false);
            m_FinishSigns[i].SetActive(false);
            m_ButtonArray[i].SetActive(false);
            m_TaskTexts[i].GetComponent<TextLine>().DestroyLine();
        }
        m_FeedbackText.text = "";
    }
    private IEnumerator Extend(int index)
    {
        float currentExtendHeight=0;
        while (currentExtendHeight<m_MaxExtendHeight)
        {
            currentExtendHeight += m_ExtendRate;
            m_CurrentExtendHeights[index] += m_ExtendRate;
            yield return 0;
        }
    }
    private IEnumerator Shrink(int index)
    {
        float currentShrinkHeight = m_MaxExtendHeight;
        while (currentShrinkHeight > 0)
        {
            currentShrinkHeight -= m_ExtendRate;
            m_CurrentExtendHeights[index] -= m_ExtendRate;
            yield return 0;
        }
    }
}
