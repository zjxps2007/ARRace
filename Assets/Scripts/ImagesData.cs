using UnityEngine;

[System.Serializable]
public struct ImageInfo
{
    public Texture texture;
    public int width;
    public int height;
}

public class ImagesData : MonoBehaviour
{

    public ImageInfo[] images;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Vector2 AspectRation(float width, float height)
    {
        Vector2 scale = Vector2.one;

        if (width == 0 || height == 0)
        {
            return scale;
        }

        if (width > height)
        {
            scale.x = 1f;
            scale.y = height / width;
        }
        else
        {
            scale.x = width / height;
            scale.y = 1f;
        }

        return scale;
    }
}
