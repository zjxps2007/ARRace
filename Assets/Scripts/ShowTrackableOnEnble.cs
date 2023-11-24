using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ShowTrackableOnEnble : MonoBehaviour
{
    [SerializeField] private XROrigin sessionOrigin;

    private ARPlaneManager planeManager;

    private ARPointCloudManager cloudManager;

    private bool isStarted;

    private void Awake()
    {
        planeManager = sessionOrigin.GetComponent<ARPlaneManager>();
        cloudManager = sessionOrigin.GetComponent<ARPointCloudManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        ShowTrackables(true);
    }

    private void OnDisable()
    {
        if (isStarted)
        {
            ShowTrackables(false);
        }
    }

    void ShowTrackables(bool show)
    {
        if (cloudManager)
        {
            cloudManager.SetTrackablesActive(show);
            cloudManager.enabled = show;
        }

        if (planeManager)
        {
            planeManager.SetTrackablesActive(show);
            planeManager.enabled = show;
        }
    }
}
