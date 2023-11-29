using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    /*==========================코인 UI 변수 선언==========================*/
    [SerializeField]
    private TextMeshProUGUI coin;
    
    /*==========================이동 속도 변수 선언==========================*/
    private float moveSpeed = 1.0f; // 움직임
    private float rotSpeed = 120.0f; // 회전
    
    /*==========================이동 관련 변수 선언==========================*/
    protected bool isMovingForward = false;  // 전진
    protected bool isMovingBackward = false; // 후진
    protected bool isMovingLeft = false;     // 왼쪽 회전
    protected bool isMovingRight = false;    // 오른쪽 회전

    float timer = 0.0f;
    Animator animator;

    private SceneManager sceneManager;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(isMovingForward || isMovingBackward)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

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
        
        coin.text = $"coin : {PlayerManager.coin}";
        Move();
    }
    
    
    /*===================== 이동 함수 =====================*/
    private void Move()
    {
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

    void OnCollisionEnter(Collision other)
    {
         if (other.transform == PlayerManager.playingPlane.gameObject.transform && timer > 1.5f)
        {
            //게임 오버
            Destroy(this.gameObject);
            SceneManager.LoadScene(0);
        }
    }
    
    /*===================== 이동 제어 버튼 함수 =====================*/
    public void MovingForwardUp()
    {
        isMovingForward = false;
    }
    
    public void MovingForwardDown()
    {
        isMovingForward = true;
    }

    public void MovingBackwardUp()
    {
        isMovingBackward = false;
    }
    
    public void MovingBackwardDown()
    {
        isMovingBackward = true;
    }

    public void MovingLeftUp()
    {
        isMovingLeft = false;
    }
    
    public void MovingLeftDown()
    {
        isMovingLeft = true;
    }

    public void MovingRightUp()
    {
        isMovingRight = false;
    }
    
    public void MovingRightDown()
    {
        isMovingRight = true;
    }
}
