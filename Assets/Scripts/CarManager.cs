using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CarManager : MonoBehaviour
{
    public GameObject GameObjectToInstantiate;// 터치하여 생성할 오브젝트

    private ARRaycastManager arRaycastManager; // RaycastManager 참조
    private ARPlaneManager arPlaneManager; // ARPlaneManager 참조
    private GameObject spawnedObject; // 생성한 게임 오브젝트 저장할 변수 선언
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();


    // Start is called before the first frame update
    void Start()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!TryGetTouchPostion(out Vector2 touchPositon))
        {
            return; // 사용자의 터치가 발생하지 않은 경우에는 업데이트 함수 실행을 더 이상 진행하지 않음 
        }

        // Raycast를 실행하며, 그 결과값을 hits 변수에 담아준다. 
        if (arRaycastManager.Raycast(touchPositon, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose; // ray에 맞은 결과의 첫번째 정보를 변수로 선언

            if (spawnedObject == null)
            {
                // 생성된 게임 오브젝트가 없으면 변수로 할당한 오브젝트를 생성하고 spawnObject에 담는다
                spawnedObject = Instantiate(GameObjectToInstantiate, hitPose.position, hitPose.rotation);
            }
            else
            {
                // 생성된 오브젝트가 있다면, hitPose 위치 정보에 맞게 위치 좌표와 회전값을 대입하여 이동시킨다.
                spawnedObject.transform.position = hitPose.position;
                spawnedObject.transform.rotation = hitPose.rotation;

                foreach (var plane in arPlaneManager.trackables)
                {
                    // 오브젝트가 생성되었기 때문에 Plane 인스턴스 생성을 멈추게 한다.
                    plane.gameObject.SetActive(false);
                }
            }
        }
    }

    private bool TryGetTouchPostion(out Vector2 touchPositon)
    {
        if (Input.touchCount > 0)
        {
            touchPositon = Input.GetTouch(0).position;
            return true;
        }

        touchPositon = default;
        return false;
    }
}