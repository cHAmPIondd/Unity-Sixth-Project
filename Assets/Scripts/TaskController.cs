using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_TaskButton;
    [SerializeField]
    private Text m_NameText;
    [SerializeField]
    private Image m_PortraitImage;
    [SerializeField]
    private Text m_DescribeText;
    [SerializeField]
    private Text m_PriceText;
    [SerializeField]
    private GameObject m_AcceptGameObject;
    [SerializeField]
    private GameObject m_RefuseGameObject;
    [SerializeField]
    private GameObject m_CloseGameObject;
    [SerializeField]
    private GameObject m_TipGameObject;
    [SerializeField]
    private AudioClip m_TaskDisplayClip;
    [SerializeField]
    private Color32 m_KeyWordColor;
    public int TaskNum { get; private set; }
    public Requirement[] Requirements { get; private set; }
    public string[] RequirementDescribes { get; private set; }
    public Sprite PortraitSprite { get; private set; }

    private string[] m_NameArray;
    private Sprite[] m_SpriteArray;
    private Requirement[] m_AllRequirementArray;
    private List<Requirement> m_AchievableRequirementList;
    private ShrinkComponent m_ShrinkComponent;
	// Use this for initialization
	public void Init () 
    {
        m_NameArray = Resources.Load<NameDatabase>("NameDatabase").NameArray;
        m_SpriteArray = Resources.LoadAll<Sprite>("CustomerPortrait");
        m_AllRequirementArray = Resources.LoadAll<Requirement>("Requirements");
        m_AchievableRequirementList = new List<Requirement>();
        m_ShrinkComponent = GetComponent<ShrinkComponent>();
	}
	public void GenerateTask(int num=1)
    {
        Reset();
        CaculateAchievableRequirementArray();
        GenerateRandomTask(num);
    }
    /// <summary>
    /// 随机生成一个委托
    /// </summary>
    private void GenerateRandomTask(int taskNum = 1)
    {
        GlobalObjectManager.Instance.SoundManager.Play(m_TaskDisplayClip);
        GameManager.Instance.Gold--;
        TaskNum = taskNum;
        GlobalObjectManager.Instance.ShelteringLayer2.SetActive(true);
        m_AcceptGameObject.SetActive(true);
        m_RefuseGameObject.SetActive(true);
        m_CloseGameObject.SetActive(false);
        //设置客户名字
        m_NameText.text = m_NameArray[Random.Range(0, m_NameArray.Length)];
        //设置客户头像
        m_PortraitImage.sprite = m_SpriteArray[Random.Range(0, m_SpriteArray.Length)];
        PortraitSprite = m_PortraitImage.sprite;
        //设置描述和价钱
        m_DescribeText.text = "";
        int allPrice = 0;
        Requirements = new Requirement[taskNum];
        RequirementDescribes = new string[taskNum];
        //复制m_AchievableRequirementList
        var achievableRequirementList = new List<Requirement>();
        for (int i = 0; i < m_AchievableRequirementList.Count; i++)
        {
            achievableRequirementList.Add(m_AchievableRequirementList[i]);
        }
        for (int i = 0; i < taskNum; i++)
        {
            int price = 0;
            int randomIndex=Random.Range(0, achievableRequirementList.Count);
            Requirements[i] = achievableRequirementList[randomIndex];
            achievableRequirementList.RemoveAt(randomIndex);
            //设置描述
            RequirementDescribes[i] = Requirements[i].DescribeList[Random.Range(0, Requirements[i].DescribeList.Length)];
            RequirementDescribes[i] = RequirementDescribes[i].Replace("{", "<color=#" + m_KeyWordColor.r.ToString("x2") + "" + m_KeyWordColor.g.ToString("x2") + "" + m_KeyWordColor.b.ToString("x2") + "" + m_KeyWordColor.a.ToString("x2") + ">");
            RequirementDescribes[i] = RequirementDescribes[i].Replace("}", "</color>");
            if (taskNum!=1)
                m_DescribeText.text += i + 1 + "." + RequirementDescribes[i] + "\n";
            else
                m_DescribeText.text +=  RequirementDescribes[i] + "\n";
            //计算成本
            for (int j = 0; j < Requirements[i].SoftwareList.Length; j++)
            {
                var hardwares = Requirements[i].SoftwareList[j].Software.HardwareList;
                for (int k = 0; k < hardwares.Length; k++)
                {
                    price += hardwares[k].Cost[Requirements[i].SoftwareList[j].Level - 1];
                }
            }
            GameManager.Instance.RequirementCosts[i] = price;
            //加上大脑硬件的钱

            price = (int)(price * Random.Range(0.8f, 1.8f));//加上随机浮动的钱
            GameManager.Instance.RequirementRewards[i] = price;
            allPrice += price;
        }
        m_PriceText.text = allPrice.ToString();
    }
    /// <summary>
    /// 计算玩家当前的软件技能和金币能满足哪些任务要求
    /// </summary>
    private void CaculateAchievableRequirementArray()
    {
        m_AchievableRequirementList.Clear();
        var currentSoftwareArray = GlobalObjectManager.Instance.SoftwareFileController.CurrentSoftwareArray;
        for(int i=0;i<m_AllRequirementArray.Length;i++)//逐个任务
        {
            var softwares=m_AllRequirementArray[i].SoftwareList;
            for(int j=0;j<softwares.Length;j++)//逐个任务的软件
            {
                var gold = GameManager.Instance.Gold;
                int level = 0;
                for (int k = 0; k < currentSoftwareArray.Length; k++)//从解锁的软件中找任务要求的任务
                {
                    //寻找相同软件对应等级,找不到则为0级
                    if (currentSoftwareArray[k].Software.name.Equals(softwares[j].Software.name, System.StringComparison.CurrentCultureIgnoreCase))
                    {
                        level = currentSoftwareArray[k].Level;
                        break;
                    }
                }
                while (level < softwares[j].Level)//等级不够，金币来补
                {
                    gold -= softwares[j].Software.UpgradeCost[level];
                    level++;
                    if (gold < 0)
                    {
                        //金币不足，这个任务无法完成
                        goto BreakAndSkip;
                    }
                }
            }
            //任务符合
            m_AchievableRequirementList.Add(m_AllRequirementArray[i]);
            //任务不符合
        BreakAndSkip: ;
        }
    }
    public void Accept()
    {
        m_AcceptGameObject.SetActive(false);
        m_RefuseGameObject.SetActive(false);
        m_CloseGameObject.SetActive(true);
        m_ShrinkComponent.Shrink();
        m_TipGameObject.SetActive(false);
        GlobalObjectManager.Instance.ShelteringLayer2.SetActive(false);
    }
    private void Reset()
    {
        m_AcceptGameObject.SetActive(true);
        m_RefuseGameObject.SetActive(true);
        m_CloseGameObject.SetActive(false);
        m_ShrinkComponent.Recover();
        m_TipGameObject.SetActive(true);
    }
    public void Refuse()
    {
        GenerateRandomTask();
    }
}
