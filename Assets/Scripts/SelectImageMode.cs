using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectImageMode : MonoBehaviour
{
    [SerializeField] private AddPictureMode addPicture;
    [SerializeField] private EditPicture editPicture;
    
    public bool isReplacing = false;
    
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
        UIController.ShowUI("SelectImage");
    }

    public void ImageSelected(ImageInfo image)
    {
        if (isReplacing)
        {
            editPicture.currentPicture.SetImage(image);
            InteractionConteroller.EnableMode("EditPicture");
        }
        else
        {
            addPicture.imageInfo = image;
            InteractionConteroller.EnableMode("AddPicture");
        }
    }
}
