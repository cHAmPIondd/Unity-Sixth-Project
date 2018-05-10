using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextColor : MonoBehaviour {
    [SerializeField]
    private Color32 m_KeyWordColor;
	// Use this for initialization
	void Start () {
        var text=GetComponent<Text>();
        text.text = text.text.Replace("ffffffff", m_KeyWordColor.r.ToString("x2") + "" + m_KeyWordColor.g.ToString("x2") + "" + m_KeyWordColor.b.ToString("x2") + "" + m_KeyWordColor.a.ToString("x2"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
