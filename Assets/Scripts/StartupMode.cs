using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class StartupMode : MonoBehaviour
{
    [SerializeField] private string nextMode = "Scan";

    private void OnEnable()
    {
        UIController.ShowUI("Startup");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ARSession.state == ARSessionState.Unsupported)
        {
            InteractionConteroller.EnableMode("NonAR");
        }
        else if (ARSession.state >= ARSessionState.Ready)
        {
            InteractionConteroller.EnableMode(nextMode);
        }
    }
}
