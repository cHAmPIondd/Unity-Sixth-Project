using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLine : MonoBehaviour
{
    [SerializeField]
    private Text m_LineText;
    public void CreateLink()
    {
        var length = GetComponent<Text>().text.Length;
        var requirements = GlobalObjectManager.Instance.TaskController.Requirements;
        for (int i = 0; i < requirements.Length; i++)
            length -= GlobalObjectManager.Instance.TaskController.TaskNum * 25 * requirements[i].SoftwareList.Length;
        m_LineText.text = "______________________________________________________".Substring(0, length);
    }
    public void DestroyLine()
    {
        m_LineText.text = "";
    }
}  
