using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceMainMode : MonoBehaviour
{
    [SerializeField] private ARFaceManager faceManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        UIController.ShowUI("Main");
    }

    public void ChangePosePrefab(GameObject prefab)
    {
        foreach (ARFace face in faceManager.trackables)
        {
            ChangeableFace changeable = face.GetComponent<ChangeableFace>();
            if (changeable != null)
            {
                changeable.SetPosePrefab(prefab);
            }
        }
    }

    public void ResetFace()
    {
        foreach (ARFace face in faceManager.trackables)
        {
            ChangeableFace changeable = face.GetComponent<ChangeableFace>();
            if (changeable != null)
            {
                changeable.SetPosePrefab(null);
                changeable.ResetAccessories();
                changeable.SetMeshMaterial(null);
            }
        }
    }

    public void AddAccessory(GameObject prefab)
    {
        foreach (ARFace face in faceManager.trackables)
        {
            ChangeableFace changeable = face.GetComponent<ChangeableFace>();
            if (changeable != null)
            {
                changeable.AddAccessory(prefab);
            }
        }
    }

    public void ChangeMaterial(Material mat)
    {
        foreach (ARFace face in faceManager.trackables)
        {
            ChangeableFace changeable = face.GetComponent<ChangeableFace>();
            if (changeable != null)
            {
                changeable.SetMeshMaterial(mat);
            }
        }
    }
}
