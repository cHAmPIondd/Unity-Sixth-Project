using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour {
    [SerializeField]
    private float m_Time=1;
	// Use this for initialization
	void Start () {
        Destroy(gameObject,m_Time);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
