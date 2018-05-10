using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineAnimation : MonoBehaviour {
    [SerializeField]
    private ImageAnimation m_FirstImageAnimation;
    [SerializeField]
    private ImageAnimation m_SecondImageAnimation;
    // Use this for initialization
	void OnEnable () {
        StartCoroutine(Animation());
	}
	IEnumerator Animation()
    {
        m_FirstImageAnimation.gameObject.SetActive(false);
        m_SecondImageAnimation.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        m_FirstImageAnimation.gameObject.SetActive(true);
        m_SecondImageAnimation.gameObject.SetActive(false);
        StartCoroutine(m_FirstImageAnimation.PlayAnimation());
        yield return new WaitForSeconds(2.1f);
        m_FirstImageAnimation.gameObject.SetActive(false);
        m_SecondImageAnimation.gameObject.SetActive(true);
        StartCoroutine(m_SecondImageAnimation.PlayAnimation());
        yield return new WaitForSeconds(4.3f);
        m_FirstImageAnimation.gameObject.SetActive(false);
        m_SecondImageAnimation.gameObject.SetActive(false);
    }
}
