using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MapCreator : MonoBehaviour
{
    public static Transform onPlayerMap;//플레이어가 위치한 땅 이 땅이 currentMapObject[1,1]과 다르다면 플레이어가 이동한 것
    public static bool center;
    Ray checkerRay;
    RaycastHit hit;    //같은 곳에 맵을 성성하지 않도록 땅이 있는지 확인하는 체커
    public static bool[,] mapArray = new bool[3, 3];//땅을 만들때 true를 넣어줌
    public static Transform[,] currentMapObject = new Transform[3, 3]; //플레이어가 어디로 이동했는지 알기 위해 각 땅의 정보를 담을 배열
    public Transform map; //맵

    // Start is called before the first frame update
    void Start()//플레이어가 오브젝트와 겹치는 문제가 있어 시작할때 플레이어의 자리에 땅을 하나 만들어줌
    {
        Transform MapInstance;
        MapInstance = Instantiate(map);
        onPlayerMap = MapInstance;
        mapArray[1, 1] = true;
        center = true;
        for (int z = 0; z < 3; z++) //처음엔 모두 false이기 때문에 플레이어 주변 8칸에 맵이 생성됨
        {
            for(int x = 0; x < 3; x++)
            {
                if (mapArray[z, x] == false)
                {
                    MapCreate(z, x);
                }
            }
        }
        currentMapObject[1, 1] = onPlayerMap; // 플레이어의 위치는 오브젝트 배열상에서 언제나 [1,1]에 위치
    }

    // Update is called once per frame
    void FixedUpdate()//그냥 업데이트를 사용하면 맵이 중복되어 생기는 현상이 있어 fixedupdate사용
    {
        //this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);//맵 생성기가 언제나 각 맵의 중앙에 위치하도록 해줌
        //this.gameObject.transform.position = onPlayerMap.transform.position;
        Debug.Log(center);
    }
    void PlayerMap()
    {
        if(onPlayerMap == currentMapObject[1, 1])//플레이어가 중앙에 있는지 확인
            center = true;
        if(onPlayerMap != currentMapObject[1, 1])//중앙이 아니라면 어느곳으로 이동했는지 확인
        {
            center = false;
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if(onPlayerMap == currentMapObject[i, j])//플레이어의 위치를 확인
                    {
                        if(1 - i == 1)  //만약 i(z)축으로 이동했다면 createdMap배열 윗줄을 모두 false로 만들어 그곳에 맵을 생성함
                        {
                            mapArray[0, 0] = false;
                            mapArray[0, 1] = false;
                            mapArray[0, 2] = false;
                        }
                        else if(1 - i == -1)
                        {
                            mapArray[2, 0] = false;
                            mapArray[2, 1] = false;
                            mapArray[2, 2] = false;
                        }
                        if(1 - j == 1)
                        {
                            mapArray[0, 0] = false;
                            mapArray[1, 0] = false;
                            mapArray[2, 0] = false;
                        }
                        else if(1 - j == -1)
                        {
                            mapArray[0, 2] = false;
                            mapArray[1, 2] = false;
                            mapArray[2, 2] = false;
                        }
                    }
                }
            }
        }
        for(int z = 0; z < 3; z++)
        {
            for(int x = 0; x < 3; x++)
            {
                if(mapArray[z, x] == false)//플레이어가 움직였고 맵을 만들 곳에 맵이없다면 맵을 생성해줌
                    MapCreate(z, x);
            }
        }
    }
    void MapCreate(int z, int x)
    {
        Transform MapInstance;
        MapInstance = Instantiate(map);
        MapInstance.transform.position = onPlayerMap.transform.position;//플레이어가 위치한 땅으로 인스턴스의 위치 초기화
        mapArray[z, x] = false;
        if(Physics.Raycast(MapInstance.transform.position + new Vector3(0.3f * z - 0.3f, 1.0f, 0.3f * x - 0.3f), Vector3.down, out hit, 1.0f))
        {
            mapArray[z, x] = true;
            for(int a = 0; a < 3; a++)
            {
                for(int b = 0; b < 3; b++)
                {
                    Debug.Log(mapArray[a, b]);
                }
            }
        }
        Debug.DrawRay(MapInstance.transform.position + new Vector3(0.3f * z - 0.3f, 1.0f, 0.3f * x - 0.3f), Vector3.down * 1.0f, Color.red);
        //int z = index / 3;  //index를 축으로 변환해줌
        //int x = index % 3;
        switch (z)  //i(z)의 값에 따라 인스턴스의 위치 조절 
        {
            case 0:
                MapInstance.transform.position += new Vector3(0.0f, 0.0f, 0.3f);
                break;
            case 1:
                MapInstance.transform.position += new Vector3(0.0f, 0.0f, 0.0f);
                break;
            case 2:
                MapInstance.transform.position += new Vector3(0.0f, 0.0f, -0.3f);
                break;
        }
        switch (x)  //j(x)의 값에 따라 인스턴스의 위치 조절 
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
        mapArray[z, x] = true;  //만들었으니 배열에 표시해줌
    }

    void OnTriggerEnter(Collider other) 
    {
        PlayerMap();
    }
}