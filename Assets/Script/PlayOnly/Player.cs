using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    GameMgr gameMgr;
    Animator animator;
    private float timer;
    
    

    // Start is called before the first frame update
    void Start()
    {
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();

        timer = 0.0f;

        transform.position = new Vector3(-5, -3, 0);

        this.animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        //‚©‚Ü‚¦
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            gameMgr.pressSpace();

            this.animator.SetTrigger("ReadyTrigger");
         

        }

      
        //‹‡
        if (Input.GetKeyUp(KeyCode.Space))
        {

            gameMgr.ReleaseSpace();

            this.animator.SetTrigger("SrashTrigger");

            transform.Translate(10, 0, 0);
            if(transform.position.x>11)
            {
                transform.position = new Vector3(-5, -3, 0);
            }
            
        }
        if (timer >= 2.0f)
        {
         transform.position = new Vector3(-5, -3, 0);

         timer = 0.0f;       

        }

    }
}
