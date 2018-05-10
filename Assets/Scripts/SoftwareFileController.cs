using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoftwareFileController : MonoBehaviour {
    public SoftwareInstance[] CurrentSoftwareArray { get; private set; }
    [SerializeField]
    private GameObject m_FirstPageGameObject;
    [SerializeField]
    private SoftwareFileSecondPageController m_SoftwareFileSecondPageController;
    [SerializeField]
    private Text[] m_ChineseTexts;
    [SerializeField]
    private Text[] m_EnglishTexts;
    [SerializeField]
    private Text[] m_LevelTexts;
    [SerializeField]
    private AudioClip m_UpgradeSuccessClip;
    [SerializeField]
    private AudioClip m_UpgradeInvalidClip;
    private int m_CurrentIndex;
    public void Init()
    {
        var allSoftwareArray = Resources.LoadAll<Software>("Softwares");
        CurrentSoftwareArray = new SoftwareInstance[allSoftwareArray.Length];
        for (int i = 0; i < CurrentSoftwareArray.Length; i++)
            CurrentSoftwareArray[i] = new SoftwareInstance(allSoftwareArray[i], 0);
        CurrentSoftwareArray[0].Level = 1;
        CurrentSoftwareArray[2].Level = 1;
        CurrentSoftwareArray[6].Level = 1;
        CurrentSoftwareArray[8].Level = 1;
        CurrentSoftwareArray[11].Level = 1;
        SetText();
    }
    public void SetText()
    {
        for(int i=0;i<CurrentSoftwareArray.Length;i++)
        {
            m_ChineseTexts[i].text = CurrentSoftwareArray[i].Software.Name;
            m_EnglishTexts[i].text = CurrentSoftwareArray[i].Software.name ;
            m_LevelTexts[i].text = CurrentSoftwareArray[i].Level+"";
        }
    }
    public void ClickSecondPage(int index)
    {
        m_CurrentIndex = index;
        var software = GlobalObjectManager.Instance.SoftwareFileController.CurrentSoftwareArray[index];
        string chineseNameString, englishNameString, hardwareRequirementString, levelExplainString;
        chineseNameString = software.Software.Name;
        englishNameString = software.Software.name+" "+software.Level;

        hardwareRequirementString = "";
        for (int i = 0; i < software.Software.HardwareList.Length; i++)
        {
            hardwareRequirementString += (i != 0 ? "," : "") + software.Software.HardwareList[i].Name;
        }
        levelExplainString = "等级一：" + software.Software.Explain[0] + "\n等级二：" + software.Software.Explain[1];
        m_SoftwareFileSecondPageController.InitAndOpen(chineseNameString, englishNameString, hardwareRequirementString, levelExplainString);
        m_FirstPageGameObject.SetActive(false);
    }
    public void ClickBack()
    {
        m_FirstPageGameObject.SetActive(true);
        m_SoftwareFileSecondPageController.gameObject.SetActive(false);
    }
    public void Upgrade()
    {
        var currentSoftwareArray = GlobalObjectManager.Instance.SoftwareFileController.CurrentSoftwareArray;
        int level = currentSoftwareArray[m_CurrentIndex].Level;
        if (level<2&&currentSoftwareArray[m_CurrentIndex].Software.UpgradeCost[level] <= GameManager.Instance.Gold)
        {
            currentSoftwareArray[m_CurrentIndex].Level++;
            GameManager.Instance.Gold -= currentSoftwareArray[m_CurrentIndex].Software.UpgradeCost[level];
            GlobalObjectManager.Instance.SoundManager.Play(m_UpgradeSuccessClip);
            SetText();
            ClickSecondPage(m_CurrentIndex);
        }
        else
        {
            GlobalObjectManager.Instance.SoundManager.Play(m_UpgradeInvalidClip);
        }
    }
}
