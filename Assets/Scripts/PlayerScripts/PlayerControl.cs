using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    
    /*==========================이동 속도 변수 선언==========================*/
    private float moveSpeed = 1.0f; // 움직임
    private float rotSpeed = 120.0f; // 회전
    
    /*==========================이동 관련 변수 선언==========================*/
    protected bool isMovingForward = false;  // 전진
    protected bool isMovingBackward = false; // 후진
    protected bool isMovingLeft = false;     // 왼쪽 회전
    protected bool isMovingRight = false;    // 오른쪽 회전

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

        if (isMovingForward)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        
        if (isMovingBackward)
        {
            transform.position -= transform.forward * moveSpeed * Time.deltaTime;
        }
        
        if (isMovingLeft )
        {
            transform.rotation *= Quaternion.Euler(0, Time.deltaTime * -rotSpeed, 0f);
        }
        
        if (isMovingRight)
        {
            transform.rotation *= Quaternion.Euler(0, Time.deltaTime * rotSpeed, 0f);
        }
    }
}
