using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerController : MonoBehaviour {
    [SerializeField]
    private Text m_OutText;
    [SerializeField]
    private InputField m_InputField;
    [SerializeField]
    private Text m_SoftwareText;
    [SerializeField]
    private GameObject m_DoneGameObject;
    [SerializeField]
    private GameObject m_IngGameObject;
    [SerializeField]
    private GameObject[] m_LightGameObject;
    [SerializeField]
    private float m_LightTime;
    [SerializeField]
    private AudioClip m_MachineStartCilp;
    [SerializeField]
    private AudioClip m_MachineStopCilp;
    [SerializeField]
    private AudioClip m_LostMoneyCilp;

    /// <summary>
    /// 提交指令
    /// </summary>
    public void SubmitInstruction(InputField inputField)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteInstruction(inputField.text);
            inputField.text = "";
            inputField.ActivateInputField();
        }
    }
    /// <summary>
    /// 执行指令
    /// </summary>
    private void ExecuteInstruction(string instruction)
    {
        OutputString(instruction);
        if (GameManager.Instance.IsDone)
        {
            OutputString("已终止输入");
            return;
        }
        //检索特殊指令
        if (instruction.Equals("Done", System.StringComparison.CurrentCultureIgnoreCase))
        {
            OutputString("全速制作中，请等待......"); 
            Done();
            return;
        }
        if (instruction.Equals("Clear", System.StringComparison.CurrentCultureIgnoreCase))
        {
            ClearOutput();
            return;
        }
        //检索格式是否正确
        var instructionArray = instruction.Split(' ');
        if (instructionArray.Length > 2)
        {//不仅一空格
            OutputString("错误格式，正确格式为 \"software level\"，例如Navigation 1");
            return;
        }
        int level;
        if (instructionArray.Length == 1 || instructionArray[1]=="")
        {
            level = 1;
        }
        else
        {
            try
            {
                level = int.Parse(instructionArray[1]);
            }
            catch (System.Exception)
            {//空格后非数字
                OutputString("错误格式，\"" + instructionArray[1] + "\" 不是一个数字");
                return;
            }
        }

        //检索软件指令
        var list = GlobalObjectManager.Instance.SoftwareFileController.CurrentSoftwareArray;
        for (int i = 0; i < list.Length; i++)
        {
            if (instructionArray[0].Equals(list[i].Software.name, System.StringComparison.CurrentCultureIgnoreCase))
            {
                if (list[i].Level < level)
                {
                    OutputString("您的软件等级过低，请先升级");
                }
                else
                {
                    var robot = GameManager.Instance.CurrentRobot;
                    //判断是否缺乏所要求的硬件
                    var hardwares=list[i].Software.HardwareList;//需要的硬件
                    for (int j = 0; j < hardwares.Length; j++)
                    {
                        for(int k=0;k<robot.HardwareList.Count;k++)
                        {
                            if(robot.HardwareList[k].Hardware.name.Equals(hardwares[j].name,System.StringComparison.CurrentCultureIgnoreCase))
                            {//有同名
                                if (robot.HardwareList[k].Level < level)
                                {
                                    OutputString("\""+hardwares[j].Name+"\"硬件等级不足");
                                    return;
                                }
                                goto BreakAndSkip;
                            }
                        }
                        OutputString("缺少\"" + hardwares[j].Name+"\"硬件");
                        return;
                    BreakAndSkip: ;
                    }
                    //判断是否已输入该软件
                    for (int j = 0; j < robot.SoftwareList.Count;j++ )
                    {
                        if(robot.SoftwareList[j].Software.name.Equals(list[i].Software.name, System.StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (robot.SoftwareList[j].Level == level)
                            {
                                OutputString("这个软件已经输入过了");
                            }
                            else
                            {
                                if (level == 0)
                                {
                                    robot.SoftwareList.RemoveAt(j);
                                    OutputString("成功移除软件");
                                }
                                else
                                {
                                    robot.SoftwareList[j].Level = level;
                                    OutputString("软件等级已成功更改");
                                }
                                UpdateSoftwareText();
                            }
                            return;
                        }
                    }
                    robot.SoftwareList.Add(new SoftwareInstance(list[i].Software,level));
                    OutputString("成功输入\""+list[i].Software.Name+"\""+level+"级");
                    UpdateSoftwareText();
                    GlobalObjectManager.Instance.TipController.Next(5);
                }
                return;
            }
        }
        if (instructionArray.Length == 1)
            OutputString("错误格式，正确格式为 \"software level\"，\n例如Navigation 1");
        else
            OutputString("无法找到 \"" + instructionArray[0] + "\" 这个软件");
    }
    public void Done()
    {
        GlobalObjectManager.Instance.TipController.Next(6);
        StartCoroutine(DoneProcess());
    }
    private IEnumerator DoneProcess()
    {
        GameManager.Instance.CloseBackgroundPages();
        GameManager.Instance.IsDone = true;
        GameManager.Instance.Gold -= GameManager.Instance.CurrentRobot.Cost;
        GlobalObjectManager.Instance.SoundManager.Play(m_LostMoneyCilp);
        yield return new WaitForSeconds(1f);
        m_DoneGameObject.SetActive(false);
        m_IngGameObject.SetActive(true);
        GlobalObjectManager.Instance.SoundManager.Play(m_MachineStartCilp);
        for (int i = 0; i < m_MachineStartCilp.length / m_LightTime; i++)
        {
            m_LightGameObject[i % 3].SetActive(true);
            m_LightGameObject[(i + 1) % 3].SetActive(false);
            m_LightGameObject[(i + 2) % 3].SetActive(false);
            yield return new WaitForSeconds(m_LightTime);
        }
        m_LightGameObject[0].SetActive(false);
        m_LightGameObject[1].SetActive(false);
        m_LightGameObject[2].SetActive(false);
        m_DoneGameObject.SetActive(true);
        m_IngGameObject.SetActive(false);
        GlobalObjectManager.Instance.SoundManager.Play(m_MachineStopCilp);
        yield return new WaitForSeconds(1f);
        StartCoroutine(GlobalObjectManager.Instance.FeedbackController.SubmitTask());
        GlobalObjectManager.Instance.ShelteringLayer.SetActive(true);
    }
    /// <summary>
    /// 往m_OutText输出文本
    /// </summary>
    private void OutputString(string text)
    {
        m_OutText.text += (text + "\n");
    }
    public void ClearOutput()
    {
        m_OutText.text = "";
    }
    public void UpdateSoftwareText()
    {
        m_SoftwareText.text = "";
        var list = GameManager.Instance.CurrentRobot.SoftwareList;
        for (int i = 0; i < list.Count; i++)
        {
            m_SoftwareText.text += list[i].Software.name + " " + list[i].Level+ "\n";
        }
    }
}
