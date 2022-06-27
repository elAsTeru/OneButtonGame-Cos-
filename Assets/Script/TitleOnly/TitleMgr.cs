using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMgr : MonoBehaviour
{
    AudioSource audioSource;
    [Tooltip("ゲーム開始時にならす効果音")][SerializeField]AudioClip se_gameStart;
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
            //フェードアウトしながら
            timer += Time.deltaTime;
        }
        else
        {
            //プレイシーンに移動
            SceneManager.LoadScene("Play");
        }
    }
}
