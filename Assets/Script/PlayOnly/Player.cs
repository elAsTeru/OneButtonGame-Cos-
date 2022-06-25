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

    // Start is called before the first frame update
    void Start()
    {
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();
        idol.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //かまえ
        if (Input.GetKeyDown(KeyCode.Space)) { gameMgr.pressSpace(); }
        //居合
        if (Input.GetKeyUp(KeyCode.Space)) { gameMgr.ReleaseSpace(); }
    }


    /// <summary>
    /// 管理から呼びプレイヤーを構え状態にする
    /// </summary>
    public void SetIdolMode()
    {
        //現在たち状態じゃなければ
        if(!idol.activeSelf)
        {
            //全ての状態を非アクティブにして、
            DeactivateAllMode();
            //たち状態にする
            idol.SetActive(true);
        }
    }
    /// <summary>
    /// 管理から呼びプレイヤーを構え状態にする
    /// </summary>
    public void SetKamaeMode()
    {
        //現在kかまえ状態じゃなければ
        if (!kamae.activeSelf)
        {
            //全ての状態を非アクティブにして、
            DeactivateAllMode();
            //かまえ状態にする
            kamae.SetActive(true);
        }
    }
    /// <summary>
    /// 管理から呼びプレイヤーを居合状態にする
    /// </summary>
    public void SetIaiMode()
    {
        //現在たち状態じゃなければ
        if (!iai.activeSelf)
        {
            //全ての状態を非アクティブにして、
            DeactivateAllMode();
            //たち状態にする
            iai.SetActive(true);
        }
    }
    private void DeactivateAllMode()
    {
        if (idol.activeSelf) { idol.SetActive(false); }
        if (kamae.activeSelf) { kamae.SetActive(false); }
        if (iai.activeSelf) { iai.SetActive(false); }
    }
}
