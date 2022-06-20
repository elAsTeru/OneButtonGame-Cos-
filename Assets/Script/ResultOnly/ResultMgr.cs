using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultMgr : MonoBehaviour
{
    private bool prevKeyState; //前回のシーンのキー状態
    // Start is called before the first frame update
    void Start()
    {
        //現在のキーの状態を前回のものとして記録
        prevKeyState = Input.GetKeyDown(KeyCode.Space);
    }

    // Update is called once per frame
    void Update()
    {
        if (prevKeyState == true)
        {
            prevKeyState = Input.GetKeyDown(KeyCode.Space);
        }
        else
        {
            SpaceUpToTitle();
        }
    }

    //-------------------------------------------------------
    //Private Method
    //-------------------------------------------------------

    /// <summary>
    /// スペースキーが離されたらタイトルに遷移する
    /// </summary>
    private void SpaceUpToTitle()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SceneManager.LoadScene("Title");
        }
    }
}