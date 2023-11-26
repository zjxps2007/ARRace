using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deBugText;
    public float rotSpeed = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        deBugText.text = "Start";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Moved)
            {
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

                RaycastHit hitInfo;
                if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << 8))
                {
                    Vector3 deltaPos = touch.deltaPosition;
                    transform.Rotate(transform.up, deltaPos.x * -1.0f * rotSpeed);
                }
            }
        }
    }

    public void Up()
    {
        deBugText.text = "UP";
    }

    public void Down()
    {
        deBugText.text = "Down";
    }

    public void Left()
    {
        deBugText.text = "Left";
    }

    public void Right()
    {
        deBugText.text = "Right";
    }
}
