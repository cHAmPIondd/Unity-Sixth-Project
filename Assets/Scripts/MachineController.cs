using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineController : MonoBehaviour {
    [SerializeField]
    private Text m_CostText;
    [SerializeField]
    private Text[] m_NameTexts;
    [SerializeField]
    private Text[] m_LevelTexts;
    public void AddHardware(int index)
    {
        var hardwareList=GameManager.Instance.CurrentRobot.HardwareList;
        var hardware=GlobalObjectManager.Instance.HardwareController.AllHardwareArray[index];
        hardwareList.Add(new HardwareInstance(hardware,1));
        m_NameTexts[hardwareList.Count - 1].text = hardware.Name;
        m_LevelTexts[hardwareList.Count - 1].text = 1+"";
        m_NameTexts[hardwareList.Count - 1].gameObject.SetActive(true);
        GameManager.Instance.CurrentRobot.Cost+=hardware.Cost[0];
        m_CostText.text = GameManager.Instance.CurrentRobot.Cost+"";
    }
    public void RemoveHardware(int index)
    {
        var hardwareList = GameManager.Instance.CurrentRobot.HardwareList;
        var hardware = hardwareList[index].Hardware;
        m_NameTexts[hardwareList.Count - 1].gameObject.SetActive(false);
        GameManager.Instance.CurrentRobot.Cost -= hardware.Cost[hardwareList[index].Level-1];
        m_CostText.text = GameManager.Instance.CurrentRobot.Cost + "";
        hardwareList.RemoveAt(index);
        for (int i = index; i < hardwareList.Count; i++)
        {
            m_NameTexts[i].text = m_NameTexts[i + 1].text;
            m_LevelTexts[i].text = m_LevelTexts[i + 1].text;
        }
        GameManager.Instance.UpdateSoftware(new HardwareInstance(hardware,0));
    }
    public void UpdateHardware(int level)
    {
        GlobalObjectManager.Instance.TipController.Next(4);
        int index=GameManager.Instance.CurrentRobot.HardwareList.Count-1;
        if (index >= 0)
        {
            var hardwareInstance = GameManager.Instance.CurrentRobot.HardwareList[index];
            m_NameTexts[index].text = hardwareInstance.Hardware.Name;
            m_LevelTexts[index].text = level+"";
            GameManager.Instance.CurrentRobot.Cost -= hardwareInstance.Hardware.Cost[hardwareInstance.Level-1];
            hardwareInstance.Level = level;
            GameManager.Instance.CurrentRobot.Cost += hardwareInstance.Hardware.Cost[hardwareInstance.Level-1];
            m_CostText.text = GameManager.Instance.CurrentRobot.Cost + "";
            GameManager.Instance.UpdateSoftware(hardwareInstance);
        }
    }
    public void Reset()
    {
        for(int i=0;i<m_NameTexts.Length;i++)
        {
            m_NameTexts[i].gameObject.SetActive(false);
        }
        m_CostText.text = "0";
    }
}
