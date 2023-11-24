using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    GameObject[] maps = new GameObject[9];
    GameObject playerPlane;
    // Start is called before the first frame update
    void Start()
    {
        playerPlane = maps[4];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MapCreate()
    {
        int randInt = Random.Range(0, 4);
        randInt = randInt * 2 + 1;
        for(int i = 0; i < randInt; i++)
        {
            //7, 4
            
        }
    }
}
