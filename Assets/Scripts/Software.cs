using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoftwareInstance
{
    public Software Software;
    public int Level;
    public SoftwareInstance(Software Software, int Level)
    {
        this.Software = Software;
        this.Level = Level;
    }
    public void Upgrade()
    {
        if (Level < Software.MaxLevel)
            Level += 1;
    }
}
[CreateAssetMenu(menuName = "MySubMenu/Software")]
public class Software : ScriptableObject
{
    public string Name;
    public int MaxLevel;
    public int[] UpgradeCost;
    public string[] Explain;
    public string UpgradeExplain;
    public Hardware[] HardwareList;
}
