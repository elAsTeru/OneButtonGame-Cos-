using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultMgr : MonoBehaviour
{
    private bool prevKeyState; //�O��̃V�[���̃L�[���
    // Start is called before the first frame update
    void Start()
    {
        //���݂̃L�[�̏�Ԃ�O��̂��̂Ƃ��ċL�^
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
    /// �X�y�[�X�L�[�������ꂽ��^�C�g���ɑJ�ڂ���
    /// </summary>
    private void SpaceUpToTitle()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SceneManager.LoadScene("Title");
        }
    }
}