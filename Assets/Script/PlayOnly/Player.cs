using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    GameMgr gameMgr;

    // Start is called before the first frame update
    void Start()
    {
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();
    }

    // Update is called once per frame
    void Update()
    {
        //���܂�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            gameMgr.pressSpace();


        }


        //����
        if (Input.GetKeyUp(KeyCode.Space))
        {

            gameMgr.ReleaseSpace(); 

        }
    }
}
