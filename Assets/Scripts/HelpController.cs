using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpController : MonoBehaviour
{
    public void ClickOpenOrClose()
    {
        gameObject.SetActive(!isActiveAndEnabled);
    }
}
