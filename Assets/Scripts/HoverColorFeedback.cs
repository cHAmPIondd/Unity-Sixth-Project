using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class HoverColorFeedback : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler{
    private Image m_Image;
    private Text m_Text;
    private Color m_ImageColor;
    private Color m_TextColor;
    void Start()
    {
        m_Image = GetComponent<Image>();
        m_Text = GetComponentInChildren<Text>();
        m_ImageColor = m_Image.color;
        m_TextColor = m_Text.color;
    }
    void OnDisable()
    {
        if (m_Image != null)
        {
            m_Image.color = m_ImageColor;
            m_Text.color = m_TextColor;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        m_Image.color = m_TextColor;
        m_Text.color = m_ImageColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_Image.color = m_ImageColor;
        m_Text.color = m_TextColor;
    }
}
