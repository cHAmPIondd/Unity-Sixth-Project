using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Feedback
{
    public string EachSoftwareLevel;
    public string[] FeedbackText;
    [Range(0,1)]
    public float FinishPercentage;
}
[CreateAssetMenu(menuName = "MySubMenu/Requirement")]
public class Requirement : ScriptableObject 
{
    public string[] DescribeList;
    public SoftwareInstance[] SoftwareList;
    public Feedback[] FeedbackList;
}
