using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HardwareInstance
{
    public Hardware Hardware;
    public int Level;
    public HardwareInstance(Hardware Hardware, int Level)
    {
        this.Hardware = Hardware;
        this.Level = Level;
    }
}
[CreateAssetMenu(menuName = "MySubMenu/Hardware")]
public class Hardware : ScriptableObject
{
    public string Name;
    public int MaxLevel;
    public int[] Cost;
}
