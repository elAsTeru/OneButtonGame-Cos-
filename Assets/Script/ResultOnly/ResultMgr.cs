using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultMgr : MonoBehaviour
{
    private bool prevKeyState;
    // Start is called before the first frame update
    void Start()
    {
        prevKeyState = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space) && prevKeyState == true)
        {
            SceneManager.LoadScene("Title");
        }
        //ゲーム終了後いきなりタイトルに戻らない為に(改良の余地あり)
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            prevKeyState = true;
        }
    }
}
