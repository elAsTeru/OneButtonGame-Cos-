using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMgr : MonoBehaviour
{
    AudioSource audioSource;
    [Tooltip("�Q�[���J�n���ɂȂ炷���ʉ�")][SerializeField]AudioClip se_gameStart;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer == 0)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                audioSource.clip = se_gameStart;
                audioSource.Play();
                timer += Time.deltaTime;
            }
        }
        else if (timer <= 4)
        {
            //�t�F�[�h�A�E�g���Ȃ���
            timer += Time.deltaTime;
        }
        else
        {
            //�v���C�V�[���Ɉړ�
            SceneManager.LoadScene("Play");
        }
    }
}
