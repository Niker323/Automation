using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class FPS : MonoBehaviour {
 
    public static float fps;
 
    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 60;
    }
	
    void OnGUI()
    {
        fps = 1.0f / Time.deltaTime;
        GUILayout.Label("FPS: " + (int)fps);
    }
}