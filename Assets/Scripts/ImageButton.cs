using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ImageButton : MonoBehaviour
{
    [SerializeField] private GameObject imageButtonPrefab;
    [SerializeField] private ImagesData imagesData;
    // [SerializeField] private AddPictureMode addPicture;
    [SerializeField] private SelectImageMode selectImage;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }

        foreach (ImageInfo image in imagesData.images)
        {
            GameObject obj = Instantiate(imageButtonPrefab, transform);
            RawImage rawimage = obj.GetComponent<RawImage>();
            rawimage.texture = image.texture;
            Button button = obj.GetComponent<Button>();
            button.onClick.AddListener(() => OnClick(image));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClick(ImageInfo image)
    {
        // addPicture.imageInfo = image;
        // InteractionConteroller.EnableMode("AddPicture");
        selectImage.ImageSelected(image);
    }
}
