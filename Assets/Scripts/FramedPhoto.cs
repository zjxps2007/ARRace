using UnityEngine;

public class FramedPhoto : MonoBehaviour
{
    [SerializeField] private Transform scalarobject;
    [SerializeField] private GameObject imageobject;
    [SerializeField] private GameObject highlightObject;
    [SerializeField] private Collider boundingCollider;

    private int layer;
    private bool isEditing;
    private ImageInfo imageInfo;
    private MovePicture movePicture;
    private ResizePicture resizePicture;

    private void Awake()
    {
        layer = LayerMask.NameToLayer("PlacedObjects");
        Highlight(false);
        movePicture = GetComponent<MovePicture>();
        resizePicture = GetComponent<ResizePicture>();
        movePicture.enabled = false;
        resizePicture.enabled = false;
    }
    
    private void OnTriggerStay(Collider other)
    {
        const float spacing = 0.1f;

        if (isEditing && other.gameObject.layer == layer)
        {
            Bounds bounds = boundingCollider.bounds;
            if (other.bounds.Intersects(bounds))
            {
                Vector3 centerDistance = bounds.center = other.bounds.center;
                Vector2 distOnPlane = Vector3.ProjectOnPlane(centerDistance, transform.forward);
                Vector3 direction = distOnPlane.normalized;
                float distanceToMoveThisFrame = bounds.size.x * spacing;
                transform.Translate(direction * distanceToMoveThisFrame);
            }
        }
    }

    public void SetImage(ImageInfo image)
    {
        imageInfo = image;

        Renderer renderer = imageobject.GetComponent<Renderer>();
        Material material = renderer.material;
        material.SetTexture("_BaseMap", imageInfo.texture);
        AdjustScale();
    }

    public void AdjustScale()
    {
        Vector2 scale = ImagesData.AspectRation(imageInfo.width, imageInfo.height);
        scalarobject.localScale = new Vector3(scale.x, scale.y, 1f);
    }

    public void Highlight(bool show)
    {
        if (highlightObject)
        {
            highlightObject.SetActive(show);
        }
    }

    public void BeginEdited(bool editing)
    {
        Highlight(editing);
        isEditing = editing;
        movePicture.enabled = editing;
        resizePicture.enabled = editing;
    }
}
