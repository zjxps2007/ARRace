using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceScanMode : MonoBehaviour
{
    [SerializeField] private ARFaceManager faceManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (faceManager.trackables.count > 0)
        {
            InteractionConteroller.EnableMode("Main");
        }
    }

    private void OnEnable()
    {
        UIController.ShowUI("Scan");
    }
}
