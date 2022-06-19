using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayMgr : MonoBehaviour
{
    [Header("プレイヤーの情報")]
    private Player player;
    [Tooltip("true:かまえ状態 / false:居合状態")][SerializeField]private bool spaceKeyState;

    [Header("PlaySetting")]
    private GameObject enemyGenMgr;        //生成管理用のオブジェクトが入る
    [Tooltip("出されるお題の数")][SerializeField] private short targetNum;
    [Tooltip("クリアしたお題の数(書換無効)")][SerializeField] private short cleartargetNum;
    

    //UI関連
    [SerializeField]private GameObject printTargetObj;

    // Start is called before the first frame update
    void Start()
    {
        //クリア回数を0にする
        cleartargetNum = 0;
        //敵管理を名前で検索しアタッチして非アクティブにする
        enemyGenMgr = GameObject.Find("EnemyMgr");
        enemyGenMgr.SetActive(false);

        printTargetObj = GameObject.Find("標的表示仮置き");
        printTargetObj.GetComponent<Image>().enabled = true;

        //プレイヤーのスクリプトを確保
        player = GameObject.Find("Player").GetComponent<Player>();
        player.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //もし標的表示仮置きが表示されていたら
        if (printTargetObj.GetComponent<Image>().enabled)
        {
            //スペースが押されたら
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //標的表示仮置きを非表示に
                printTargetObj.GetComponent<Image>().enabled = false;
                //プレイヤーの処理を開始
                player.enabled = true;      //特定のコンポーネントやスクリプトが有効かを変える方法
            }
        }
        else
        {
            if (spaceKeyState == true)
            {
                //敵管理の処理を開始する
                enemyGenMgr.SetActive(false);    //全体のアクティブを変える方法
            }
            else
            {
                //敵管理の処理を開始する
                enemyGenMgr.SetActive(true);    //全体のアクティブを変える方法
            }
        }

        //クリア回数が設定回数になったらリザルトに遷移
        if(cleartargetNum >= targetNum)
        {
            SceneManager.LoadScene("Result");
        }
    }


    //Public Method For Player
    public void Kamae() { spaceKeyState = true; }
    public void Iai() { spaceKeyState = false; }

    /// <summary>
    /// プレイヤーがお題をクリアしたのを伝えるために使用
    /// </summary>
    public void TellTargetClear() { ++cleartargetNum; }
}
