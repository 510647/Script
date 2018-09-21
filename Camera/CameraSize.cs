using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour {

    public float size_Height = 1024;
    public float size_Width = 768;

	// Use this for initialization
	void Start () {
        Screen.SetResolution(1024, 768, false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
