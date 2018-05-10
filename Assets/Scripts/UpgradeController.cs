using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour {
    [SerializeField]
    private Text m_UpgradeInformationName;
    [SerializeField]
    private Text m_UpgradeInformationLevel;
    [SerializeField]
    private Text m_UpgradeInformationPriceTitle;
    [SerializeField]
    private GameObject m_UpgradeInformationPriceSign;
    [SerializeField]
    private Text m_UpgradeInformationPrice;
    [SerializeField]
    private Text m_UpgradeInformationExplain;
    [SerializeField]
    private GameObject m_UpgradeButton;
    [SerializeField]
    private GameObject[] m_UpgradeItemPrefabs;
    [SerializeField]
    private RectTransform[] m_InstantiatePos;
    [SerializeField]
    private float m_ItemHeight = 50f;
    [SerializeField]
    private ShrinkComponent m_ShrinkComponent;
    [SerializeField]
    private AudioClip m_SuccessClip;
    [SerializeField]
    private AudioClip m_InvalidClip;

    private int m_CurrentOpenIndex;
    private GameObject[] m_UpgradeItemArray;
    void Start()
    {
        var currentSoftwareArray = GlobalObjectManager.Instance.SoftwareFileController.CurrentSoftwareArray;
        m_UpgradeItemArray = new GameObject[currentSoftwareArray.Length];
        for (int i = 0; i < currentSoftwareArray.Length; i++)
        {
            m_UpgradeItemArray[i]=InstantiateUpgradeItem(i);
        }
    }
    public void ClickUpgrade()
    {
        var currentSoftwareArray = GlobalObjectManager.Instance.SoftwareFileController.CurrentSoftwareArray;
        int level=currentSoftwareArray[m_CurrentOpenIndex].Level;
        if(currentSoftwareArray[m_CurrentOpenIndex].Software.UpgradeCost[level]<=GameManager.Instance.Gold)
        {
            currentSoftwareArray[m_CurrentOpenIndex].Level++;
            GameManager.Instance.Gold -= currentSoftwareArray[m_CurrentOpenIndex].Software.UpgradeCost[level];
            m_UpgradeItemArray[m_CurrentOpenIndex] = InstantiateUpgradeItem(m_CurrentOpenIndex);
            UpdateInformation(m_CurrentOpenIndex);
            GlobalObjectManager.Instance.SoundManager.Play(m_SuccessClip);
        }
        else
        {
            GlobalObjectManager.Instance.SoundManager.Play(m_InvalidClip);
        }
        GlobalObjectManager.Instance.SoftwareFileController.SetText();
    }
    public void ClickOpen()
    {
        GlobalObjectManager.Instance.ComputerController.gameObject.SetActive(false);
        GlobalObjectManager.Instance.MachineDisplay.SetActive(false);
    }
    public void ClickClose()
    {
        GlobalObjectManager.Instance.ComputerController.gameObject.SetActive(true);
        GlobalObjectManager.Instance.MachineDisplay.SetActive(true);
        GlobalObjectManager.Instance.ShelteringLayer.SetActive(false);
    }
    private GameObject InstantiateUpgradeItem(int index)
    {
        var currentSoftwareArray = GlobalObjectManager.Instance.SoftwareFileController.CurrentSoftwareArray;
        GameObject go;
        if (currentSoftwareArray[index].Level == 0)
            go = Instantiate<GameObject>(m_UpgradeItemPrefabs[0], transform);
        else
            go = Instantiate<GameObject>(m_UpgradeItemPrefabs[1], transform);

        var pos = m_InstantiatePos[index < currentSoftwareArray.Length /2? 0 : 1].position;
        go.GetComponent<RectTransform>().position = new Vector2(pos.x, pos.y - m_ItemHeight * (index % (currentSoftwareArray.Length/2)));
        go.GetComponent<UpgradeItem>().Init(currentSoftwareArray[index].Software.Name, index, PointerEnterItem);
        return go;
    }
    public void PointerEnterItem(int index)
    {
        if (m_CurrentOpenIndex != index)
        {
            m_CurrentOpenIndex = index;
            UpdateInformation(index);
        }
    }
    private void UpdateInformation(int index)
    {
        var currentSoftwareArray = GlobalObjectManager.Instance.SoftwareFileController.CurrentSoftwareArray;
        int level=currentSoftwareArray[index].Level;
        m_UpgradeInformationName.text = currentSoftwareArray[index].Software.name;
        m_UpgradeInformationLevel.text = level+"";
        if (level < 2)
        {
            m_UpgradeInformationPriceTitle.text = "下一等级";
            m_UpgradeInformationPrice.text = ""+currentSoftwareArray[index].Software.UpgradeCost[level];
            m_ShrinkComponent.Recover(() => m_UpgradeButton.SetActive(true));
            m_UpgradeInformationPriceSign.SetActive(true);
        }
        else
        {
            m_UpgradeInformationPriceTitle.text = "最高等级";
            m_UpgradeInformationPrice.text = "";
            m_UpgradeInformationPriceSign.SetActive(false);
            m_UpgradeButton.SetActive(false);
            m_ShrinkComponent.Shrink();
        }
        switch(level)
        {
            case 0: m_UpgradeInformationExplain.text = currentSoftwareArray[index].Software.Explain[0]; break;
            case 1: m_UpgradeInformationExplain.text = currentSoftwareArray[index].Software.UpgradeExplain; break;
            case 2: m_UpgradeInformationExplain.text = currentSoftwareArray[index].Software.Explain[1]; break;
        }
    }
}
