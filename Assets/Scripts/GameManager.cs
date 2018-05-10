using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {
    public static  GameManager Instance { get; private set; }
    public Robot CurrentRobot { get;private set; }
    public int Gold 
    { 
        get 
        {
            return m_Gold; 
        } 
        set 
        {
            GlobalObjectManager.Instance.GoldChangeAnimation.Play(value-m_Gold);
            m_Gold = value;
            StartCoroutine(GoldChange());
        }
    }
    private int m_Gold;

    public int[] RequirementCosts { get;private set; }
    public int[] RequirementRewards { get; private set; }
    public bool IsDone { get; set; }

    [SerializeField]
    private Text m_GoldText;
    [SerializeField]
    private float m_GoldChangeTime=0.2f;


    void Awake()
    {
        Instance = this;
        RequirementCosts = new int[3];
        RequirementRewards = new int[3];
        GlobalObjectManager.Instance.SoftwareFileController.Init();
        GlobalObjectManager.Instance.HardwareController.Init();
        GlobalObjectManager.Instance.TaskController.Init();
        GlobalObjectManager.Instance.FeedbackController.Init();
    }
	public void GameStart (GameObject mainPage) {
        mainPage.SetActive(false);
	}
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    public void NextTask()
    {
        GlobalObjectManager.Instance.ComputerController.ClearOutput();
        GlobalObjectManager.Instance.MachineController.Reset();
        CurrentRobot = new Robot();
        IsDone = false;
        GlobalObjectManager.Instance.ComputerController.UpdateSoftwareText();
        GlobalObjectManager.Instance.ShelteringLayer.SetActive(false);
        GlobalObjectManager.Instance.TaskButton.Click();
        GlobalObjectManager.Instance.TaskController.GenerateTask();
    }
    public void CloseBackgroundPages()
    {
        GlobalObjectManager.Instance.HardwareDragDrawer.Close();
        GlobalObjectManager.Instance.SoftwareFileButton.ClickBack();
        GlobalObjectManager.Instance.TaskButton.ClickBack();
        GlobalObjectManager.Instance.ShelteringLayer.SetActive(true);
    }
    public void UpdateSoftware(HardwareInstance hardwareInstance)
    {
        for (int i = 0; i < CurrentRobot.SoftwareList.Count; i++)
        {
            var hardwares = CurrentRobot.SoftwareList[i].Software.HardwareList;//需要的硬件
            for (int j = 0; j < hardwares.Length; j++)
            {
                if(hardwares[j].name.Equals(hardwareInstance.Hardware.name,System.StringComparison.CurrentCultureIgnoreCase))
                {
                    if(hardwareInstance.Level<CurrentRobot.SoftwareList[i].Level)
                    {
                        CurrentRobot.SoftwareList.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
        }
        GlobalObjectManager.Instance.ComputerController.UpdateSoftwareText();
    }
    private IEnumerator GoldChange()
    {
        int current =int.Parse(m_GoldText.text);
        while (current!=m_Gold)
        {
            if (current > m_Gold)
                current--;
            else
                current++;
            m_GoldText.text = current + "";
            yield return new WaitForSeconds(m_GoldChangeTime);
        }
    }
}
