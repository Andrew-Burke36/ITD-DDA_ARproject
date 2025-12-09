// Created by Andrew Burke

using UnityEngine;

public class FPSLock : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Disable vSync so Application.targetFrameRate takes effect
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
