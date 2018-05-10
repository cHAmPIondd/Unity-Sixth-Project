using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot{
    public List<SoftwareInstance> SoftwareList { get;private set; }
    public List<HardwareInstance> HardwareList { get;private set; }
    public int Cost { get; set; }
    public Robot()
    {
        SoftwareList = new List<SoftwareInstance>();
        HardwareList = new List<HardwareInstance>();
    }
}
