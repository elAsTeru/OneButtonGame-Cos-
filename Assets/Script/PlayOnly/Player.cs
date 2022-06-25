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
        //かまえ
        if (Input.GetKeyDown(KeyCode.Space)) { gameMgr.pressSpace(); }
        //居合
        if (Input.GetKeyUp(KeyCode.Space)) { gameMgr.ReleaseSpace(); }
    }
}
