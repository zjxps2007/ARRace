using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MapCreator : MonoBehaviour
{
    public static Transform onPlayerMap;//플레이어가 위치한 땅 이 땅이 currentMapObject[1,1]과 다르다면 플레이어가 이동한 것
    public static bool center;
    Ray checkerRay;    //같은 곳에 맵을 성성하지 않도록 땅이 있는지 확인하는 체커
    public static bool[,] mapArray = new bool[3, 3];//땅을 만들때 true를 넣어줌
    public static Transform[,] currentMapObject = new Transform[3, 3]; //플레이어가 어디로 이동했는지 알기 위해 각 땅의 정보를 담을 배열
    public Transform map; //맵
    Transform hitTemp;
    int mask;

    // Start is called before the first frame update
    void Start()//플레이어가 오브젝트와 겹치는 문제가 있어 시작할때 플레이어의 자리에 땅을 하나 만들어줌
    {
        Transform MapInstance;
        MapInstance = Instantiate(map);
        onPlayerMap = MapInstance;
        mapArray[1, 1] = true;
        center = true;
        mask = 1 << 6;
        currentMapObject[1, 1] = onPlayerMap; // 플레이어의 위치는 오브젝트 배열상에서 언제나 [1,1]에 위치
        hitTemp = onPlayerMap.transform;
        MapCreate();
    }

    // Update is called once per frame
    void FixedUpdate()//그냥 업데이트를 사용하면 맵이 중복되어 생기는 현상이 있어 fixedupdate사용
    {
        //this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);//맵 생성기가 언제나 각 맵의 중앙에 위치하도록 해줌
        //this.gameObject.transform.position = onPlayerMap.transform.position;
        //PlayerMoved();
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
    }
    void MapCheck()
    {
        RaycastHit hit;
        for(int x = 0; x < 3; x++)
        {
            for(int z = 0; z < 3; z++)
            {
                mapArray[x, z] = false;
                if(Physics.Raycast(onPlayerMap.transform.position + new Vector3(0.3f * x - 0.3f, 1.0f, 0.3f * z - 0.3f), Vector3.down, out hit, 2.0f, mask))
                {
                    Debug.DrawRay(onPlayerMap.transform.position + new Vector3(0.3f * x - 0.3f, 1.0f, 0.3f * z - 0.3f), Vector3.down * 2.0f, Color.red);
                    mapArray[x, z] = true;
                    for(int a = 0; a < 3; a++)
                    {
                        for(int b = 0; b < 3; b++)
                        {
                            Debug.Log(mapArray[a, b]);
                        }
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
                //10, 01, 21, 12
                if(mapArray[x, z])
                    continue;
                else
                {
                    int rand = Random.Range(0, 2);
                    Transform MapInstance;
                    MapInstance = Instantiate(map);
                    MapInstance.transform.position = onPlayerMap.transform.position;//플레이어가 위치한 땅으로 인스턴스의 위치 초기화
                    // if((z == 1 && x == 0) || (z == 0 && x == 1) || (z == 2 && x == 1) || (z == 1 && x == 2))
                    // {
                    //     MapInstance.transform.position += new Vector3(0.0f, -1.5f, 0.0f);
                    // }
                    switch (z) //z의 값에 따라 인스턴스의 위치 조절
                    {
                        case 0:
                            MapInstance.transform.position += new Vector3(0.0f, 0.0f, -0.3f);
                            break;
                        case 1:
                            MapInstance.transform.position += new Vector3(0.0f, 0.0f, 0.0f);
                            break;
                        case 2:
                            MapInstance.transform.position += new Vector3(0.0f, 0.0f, 0.3f);
                            break;
                    }
                    switch (x) //x의 값에 따라 인스턴스의 위치 조절 
                    {
                        case 0:
                            MapInstance.transform.position += new Vector3(-0.3f, 0.0f, 0.0f);
                            break;
                        case 1:
                            MapInstance.transform.position += new Vector3(0.0f, 0.0f, 0.0f);
                            break;
                        case 2:
                            MapInstance.transform.position += new Vector3(0.3f, 0.0f, 0.0f);
                            break;
                    }
                    if(rand == 1)
                    {
                        MapInstance.transform.position += new Vector3(0.0f, -1.5f, 0.0f);
                    }
                    currentMapObject[x, z] = MapInstance;
                    mapArray[x, z] = true;  //만들었으니 배열에 표시해줌
                }
            }
        }
    }

    // void OnCollisionEnter(Collision other) 
    // {
    //     onPlayerMap = other.transform;
    //     MapCheck();
    // }
}