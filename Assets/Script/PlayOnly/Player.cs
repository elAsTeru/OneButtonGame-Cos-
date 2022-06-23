using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    GameMgr gameMgr;

    public Sprite imageTh2;
    public Sprite imageTh3;
    public Image samurai;

    // Start is called before the first frame update
    void Start()
    {
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();

        samurai= GameObject.Find("Image").GetComponent<Image>();

        samurai.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        //Ç©Ç‹Ç¶
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            gameMgr.pressSpace();

            samurai.enabled = true;
            samurai.sprite = imageTh2;

        }


        //ãèçá
        if (Input.GetKeyUp(KeyCode.Space))
        {

            gameMgr.ReleaseSpace();

            samurai.enabled = true;
            samurai.sprite = imageTh3;
        }
    }
}
