using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoftwareFileSecondPageController : MonoBehaviour {
    [SerializeField]
    private Text m_ChineseNameText;
    [SerializeField]
    private Text m_EnglishNameText;
    [SerializeField]
    private Text m_HardwareRequirementText;
    [SerializeField]
    private Text m_LevelExplainText;
    public void InitAndOpen(string chineseNameString, string englishNameString, string hardwareRequirementString, string levelExplainString)
    {
        m_ChineseNameText.text = chineseNameString;
        m_EnglishNameText.text = englishNameString;
        m_HardwareRequirementText.text = hardwareRequirementString;
        m_LevelExplainText.text = levelExplainString;
        gameObject.SetActive(true);
    }
}
