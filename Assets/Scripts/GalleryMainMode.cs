using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GalleryMainMode : MonoBehaviour
{
    [SerializeField] private SelectImageMode selectImage;
    [SerializeField] private EditPicture editMode;
    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void OnEnable()
    {
        UIController.ShowUI("Main");
    }

    public void OnSelectObject(InputValue value)
    {
        Vector2 touchPosition = value.Get<Vector2>();
        FindObjectToEdit(touchPosition);
    }
    
    void FindObjectToEdit(Vector2 touchPosition)
    {
        Ray ray = camera.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("PlacedObjects");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            FramedPhoto picture = hit.collider.GetComponentInParent<FramedPhoto>();
            editMode.currentPicture = picture;
            InteractionConteroller.EnableMode("EditPicture");
        }
    }
    
    public void SelectImageToAdd()
    {
        selectImage.isReplacing = false;
        InteractionConteroller.EnableMode("AddPicture");
    }
}
