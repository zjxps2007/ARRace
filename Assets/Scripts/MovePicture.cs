using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MovePicture : MonoBehaviour
{
    private Camera camera;
    private int layerMask;

    private ARRaycastManager raycaster;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        layerMask = 1 << LayerMask.NameToLayer("PlacedObjects");

        raycaster = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMoveObject(InputValue value)
    {
        if (!enabled)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject(0))
        {
            return;
        }

        Vector2 touchPosition = value.Get<Vector2>();
        MoveObject(touchPosition);
    }

    void MoveObject(Vector2 touchPosition)
    {
        Ray ray = camera.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, Mathf.Infinity, layerMask))
        {
            if (raycaster.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                ARRaycastHit hit = hits[0];
                Vector3 position = hit.pose.position;
                Vector3 normal = -hit.pose.up;
                Quaternion rotation = Quaternion.LookRotation(normal, Vector3.up);
                transform.position = position;
                transform.rotation = rotation;
            }
        }
    }
}
