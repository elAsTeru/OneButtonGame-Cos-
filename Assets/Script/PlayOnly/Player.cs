using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayMgr playMgr;
    // Start is called before the first frame update
    void Start()
    {
        playMgr = GameObject.Find("PlayMgr").GetComponent<PlayMgr>();
    }

    // Update is called once per frame
    void Update()
    {
        //Ç©Ç‹Ç¶
        if (Input.GetKeyDown(KeyCode.Space)) { playMgr.Kamae(); }
        //ãèçá
        if (Input.GetKeyUp(KeyCode.Space)) { playMgr.Iai(); }
    }
}
