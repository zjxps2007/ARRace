using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ScanMode : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;

    private void OnEnable()
    {
        UIController.ShowUI("Scan");
    }

    // Update is called once per frame
    void Update()
    {
        if (planeManager.trackables.count > 0)
        {
            InteractionConteroller.EnableMode("Main");
        }
    }
}
