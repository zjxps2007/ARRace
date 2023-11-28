using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MapCreator : MonoBehaviour
{
    public static Transform onPlayerMap;//플레이어가 위치한 땅 이 땅이 currentMapObject[1,1]과 다르다면 플레이어가 이동한 것 
    Ray checkerRay;    //같은 곳에 맵을 성성하지 않도록 땅이 있는지 확인하는 체커
    public static bool[,] mapArray = new bool[3, 3];//땅을 만들때 true를 넣어줌
    public static Transform[,] currentMapObject = new Transform[3, 3]; //플레이어가 어디로 이동했는지 알기 위해 각 땅의 정보를 담을 배열
    public Transform map; //맵
    Transform hitTemp;
    Vector3 startPlayerPos;
    int mask = 1 << 6;
    float width = 0.3f;
    float height = 2.75f;
    GameObject playerObj;
    ARPlane playingPlane;

    // Start is called before the first frame update
    void Start()//플레이어가 오브젝트와 겹치는 문제가 있어 시작할때 플레이어의 자리에 땅을 하나 만들어줌
    {
        startPlayerPos = transform.position;
        Transform MapInstance;
        MapInstance = Instantiate(map);
        MapInstance.position = new Vector3(startPlayerPos.x, startPlayerPos.y - height, startPlayerPos.z);
        onPlayerMap = MapInstance;
        StartCoroutine(ElevateMap(MapInstance));
        currentMapObject[1, 1] = onPlayerMap; // 플레이어의 위치는 오브젝트 배열상에서 언제나 [1,1]에 위치
        hitTemp = onPlayerMap.transform;
        MapCreate();
    }

    // Update is called once per frame
    void FixedUpdate()//그냥 업데이트를 사용하면 맵이 중복되어 생기는 현상이 있어 fixedupdate사용
    {
        // if(playingPlane == null)
        // {
        //     playingPlane = GetComponent<PlayerManager>().playingPlane;
        // }
        RaycastHit hit;
        Debug.DrawRay(transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector3.down * 1.0f, Color.red);
        if(Physics.Raycast(transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector3.down, out hit, 1.0f, mask))
        {
            if(hitTemp != hit.transform)
            {
                onPlayerMap = hit.transform;
                MapCheck();
            }
        }
        hitTemp = hit.transform;
        Move();
    }
    void MapCheck()
    {
        RaycastHit hit;
        for(int x = 0; x < 3; x++)
        {
            for(int z = 0; z < 3; z++)
            {
                mapArray[x, z] = false;
                if(Physics.Raycast(onPlayerMap.transform.position + new Vector3(width * x - width, transform.position.y + 5.0f, width * z - width), Vector3.down, out hit, 10.0f, mask))
                {
                    Debug.DrawRay(onPlayerMap.transform.position + new Vector3(width * x - width, transform.position.y + 5.0f, width * z - width), Vector3.down * 10.0f, Color.red);
                    mapArray[x, z] = true;
                }
                else if(Physics.Raycast(onPlayerMap.transform.position + new Vector3(width * x - width, transform.position.y + 5.0f, width * z - width), Vector3.down, out hit, 10.0f))
                {
                    if(hit.transform == null)
                    {
                        mapArray[x, z] = true;
                    }
                }
            }
        }
        MapCreate();
    }
    void MapCreate()
    {
        for(int x = 0; x < 3; x++)
        {
            for(int z = 0; z < 3; z++)
            {
                if(mapArray[x, z])
                    continue;
                else
                {
                    int coinRand = Random.Range(0, 5);
                    int rand = Random.Range(0, 2);
                    Transform MapInstance;
                    MapInstance = Instantiate(map);
                    MapInstance.transform.position = new Vector3(onPlayerMap.transform.position.x, 0.0f, onPlayerMap.transform.position.z);//플레이어가 위치한 땅으로 인스턴스의 위치 초기화
                    // if((z == 1 && x == 0) || (z == 0 && x == 1) || (z == 2 && x == 1) || (z == 1 && x == 2))
                    // {
                    //     MapInstance.transform.position += new Vector3(0.0f, -1.5f, 0.0f);
                    // }
                    switch (z) //z의 값에 따라 인스턴스의 위치 조절
                    {
                        case 0:
                            MapInstance.transform.position += new Vector3(0.0f, 0.0f, -width);
                            break;
                        case 1:
                            MapInstance.transform.position += new Vector3(0.0f, 0.0f, 0.0f);
                            break;
                        case 2:
                            MapInstance.transform.position += new Vector3(0.0f, 0.0f, width);
                            break;
                    }
                    switch (x) //x의 값에 따라 인스턴스의 위치 조절 
                    {
                        case 0:
                            MapInstance.transform.position += new Vector3(-width, 0.0f, 0.0f);
                            break;
                        case 1:
                            MapInstance.transform.position += new Vector3(0.0f, 0.0f, 0.0f);
                            break;
                        case 2:
                            MapInstance.transform.position += new Vector3(width, 0.0f, 0.0f);
                            break;
                    }
                    if(rand == 1)
                    {
                        MapInstance.transform.position += new Vector3(0.0f, startPlayerPos.y - height, 0.0f);
                    }
                    if(rand != 1 && coinRand == 0)
                    {
                        MapInstance.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    currentMapObject[x, z] = MapInstance;
                    MapInstance.transform.position += new Vector3(0.0f, startPlayerPos.y - height, 0.0f);
                    StartCoroutine(ElevateMap(MapInstance));
                    mapArray[x, z] = true;  //만들었으니 배열에 표시해줌
                }
            }
        }
    }

    IEnumerator ElevateMap(Transform mapInstance) //맵이 생성될 때 아래에서 위로 올라오도록 하는 코루틴
    {
        float mapInstanceY = mapInstance.position.y + 1.5f;
        while (mapInstance.position.y < mapInstanceY)
        {
            mapInstance.position += new Vector3(0.0f, 3.0f * Time.deltaTime, 0.0f);
            yield return null;
        }
        mapInstance.position = new Vector3(mapInstance.position.x, mapInstanceY, mapInstance.position.z);
        yield return new WaitForSeconds(10.0f);
        while (mapInstance.position.y > mapInstanceY - 0.5f)
        {
            mapInstance.position -= new Vector3(0.0f, 3.0f * Time.deltaTime, 0.0f);
            yield return null;
        }
        Destroy(mapInstance.gameObject);
        yield return new WaitForSeconds(3.0f);
    }

    void Move()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.forward * 1.0f * Time.deltaTime;
        }
        
        if(Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.back * 1.0f * Time.deltaTime;
        }
        
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * 1.0f * Time.deltaTime;
        }
        
        if(Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * 1.0f * Time.deltaTime;
        }
    }
}