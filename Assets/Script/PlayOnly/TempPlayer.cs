using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayer : MonoBehaviour
{
    private PlayMgr playMgr;

    // Start is called before the first frame update
    void Start()
    {
        playMgr = GameObject.Find("PlayMgr").GetComponent<PlayMgr>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            playMgr.TellTargetClear();
        }
    }
}
