using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObjectManager : MonoBehaviour {
    public static GlobalObjectManager Instance { get; private set; }
    public GameObject ShelteringLayer2;
    public GameObject ShelteringLayer;
    public ComputerController ComputerController;
    public MachineController MachineController;
    public GameObject MachineDisplay;
    public RightThreeButton SoftwareFileButton;
    public SoftwareFileController SoftwareFileController;
    public DragDrawer HardwareDragDrawer;
    public HardwareController HardwareController;
    public RightThreeButton TaskButton;
    public TaskController TaskController;
    public FeedbackController FeedbackController;
    public DragPartController DragPartController;
    public SoundManager SoundManager;
    public MusicBoxController MusicBoxController;
    public TipController TipController;
    public GoldChangeAnimation GoldChangeAnimation;
    void Awake()
    {
        Instance = this;
    }
	
}
