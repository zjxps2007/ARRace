using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deBugText;

    private float moveSpeed = 1.0f;
    public float rotSpeed = 0.1f;
    
    private bool moveForward;
    private bool moveBack;
    private bool moveLeft;
    private bool moveRight;
    private int count = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
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

        if (moveForward)
        {
            transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
        }
        if (moveBack)
        {
            transform.position -= Vector3.forward * moveSpeed * Time.deltaTime;
        }
        if (moveRight)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        if (moveLeft)
        {
            transform.position -= Vector3.right * moveSpeed * Time.deltaTime;
        }
    }

    public void Up()
    {
        count++;
        deBugText.text = "UP, " + count;
        moveForward = true;
    }
    
    public void UpDown()
    {
        deBugText.text = "UP";
        moveForward = false;
    }

    public void Down()
    {
        deBugText.text = "Down";
        moveBack = true;
    }

    public void DownDown()
    {
        deBugText.text = "Down";
        moveBack = false;
    }

    public void Left()
    {
        deBugText.text = "Left";
        moveLeft = true;
    }
    public void LeftDown()
    {
        deBugText.text = "Left";
        moveLeft = false;
    }

    public void Right()
    {
        deBugText.text = "Right";
        moveRight = true;
    }
    public void RightDown()
    {
        deBugText.text = "Right";
        moveRight = false;
    }
}
