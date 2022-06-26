using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameMgr gameMgr;
    [Header("Player")]
    [Tooltip("立ちのオブジェクト")] [SerializeField] private GameObject idol;
    [Tooltip("かまえのオブジェクト")] [SerializeField] private GameObject kamae;
    [Tooltip("居合のオブジェクト")] [SerializeField] private GameObject iai;
    [Tooltip("前回のスペースキーの状態")][SerializeField] private bool prevSpaceKeyState;

    [Tooltip("現在の状態")][SerializeField] GameObject nowState;
    [Tooltip("前回の状態")][SerializeField] GameObject prevState;

    [Tooltip("初期位置")][SerializeField] Vector3 firstPos;
    [Tooltip("移動後の位置")][SerializeField] Vector3 endPos;
    // Start is called before the first frame update
    void Start()
    {
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();
        nowState = idol;
        prevState = idol;
        //プレイヤーをidol状態で表示
        if (!nowState.activeSelf)
        {
            nowState.SetActive(true);
        }
        prevSpaceKeyState = false;
        //現在位置を初期位置として記録する
        firstPos = this.transform.position;
        //移動後の位置を記録する
        {
            GameObject endPosObj;
            endPosObj = GameObject.Find("EndPos").gameObject;
            endPos = endPosObj.transform.position;
            endPosObj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSaceKeyState();
    }

    /// <summary>
    /// SpaceKeyの状態を前回で現在で比較し、違えば、GameMgrに伝える
    /// </summary>
    private void UpdateSaceKeyState()
    {
        if(Input.GetKey(KeyCode.Space) != prevSpaceKeyState)
        {
            //状態を反転する
            prevSpaceKeyState = !prevSpaceKeyState;
            if(prevSpaceKeyState)
            {
                gameMgr.pressSpace();
            }
            else
            {
                gameMgr.ReleaseSpace();
            }
        }
    }

    /// <summary>
    /// 前回と現在の状態を比較し、違えば変更する
    /// </summary>
    private void UpdateState()
    {
        //現在の状態と前回の状態が違ったら
        if(nowState != prevState)
        {
            //現在の状態を表示に適応し
            nowState.SetActive(true);
            //前回の状態を非表示、
            prevState.SetActive(false);
            //前回の状態に今の状態を記録する
            prevState = nowState;
        }
    }

    public void SetStateIdol()
    {
        this.transform.position = firstPos;
        nowState = idol;
        UpdateState();
    }
    public void SetStateKamae()
    {
        this.transform.position = firstPos;
        nowState = kamae;
        UpdateState();
    }
    public void SetStateIai()
    {
        this.transform.position = endPos;
        nowState = iai;
        UpdateState();
    }
}
