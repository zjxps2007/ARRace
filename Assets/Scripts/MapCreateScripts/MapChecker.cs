using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChecker : MonoBehaviour
{
    public int index;
    Transform checkedMap;//체커에 닿은 땅을 넣어줄 변수

    void Start()
    {
        index = retindex();
    }
    void FixedUpdate()//그냥 업데이트를 사용하면 맵이 중복되어 생기는 현상이 있어 fixedupdate사용
    {
        if(checkedMap == null)//만약 확인된 땅이 계속 null일경우 그곳에는 맵이 없는것으로 판단해 exist배열에 false저장
        {
            MapCreator.mapArray[index] = false;
        }
        MapCreator.currentMapObject[index / 3, index % 3] = checkedMap; //트리거로 가져온 땅을 오브젝트 배열에 넣어줌
        if(!MapCreator.center)
        {
            checkedMap = null;//플레이어가 중앙에 없을 경우(이동했을 경우) null로 초기화
        }
    }
    int retindex()//각 체커의 인덱스 리턴
    {
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                if(this.gameObject.name == "MapChecker["+ i +", "+ j +"]")
                    return i * 3 + j;
            }
        }
        return -1;
    }
    void OnTriggerStay(Collider other) //땅에 닿을경우 각 체커에 해당하는 인덱스의 exist배열 값을 true로 변환
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            checkedMap = other.transform; //체커에 닿은 땅 오브젝트을 가져옴
            MapCreator.mapArray[index] = true;
        }
    }
}
