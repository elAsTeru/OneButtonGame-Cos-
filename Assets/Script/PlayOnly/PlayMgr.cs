using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayMgr : MonoBehaviour
{
    [Header("PlaySetting")]
    private GameObject enemyGenMgr;        //生成管理用のオブジェクトが入る
    [Tooltip("出されるお題の数")][SerializeField] private short targetNum;
    [Tooltip("クリアしたお題の数(書換無効)")][SerializeField] private short cleartargetNum;
    

    //UI関連
    [SerializeField]private GameObject printTargetObj;

    //プレイヤー関連(仮置き)
    [SerializeField]private TempPlayer tempPlayer;

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
        tempPlayer = GameObject.Find("Player(temp)").GetComponent<TempPlayer>();
        tempPlayer.enabled = false;
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
            }
        }
        else
        {
            //敵管理の処理を開始する
            enemyGenMgr.SetActive(true);    //全体のアクティブを変える方法
            //プレイヤーの処理を開始
            tempPlayer.enabled = true;      //特定のコンポーネントやスクリプトが有効かを変える方法
        }

        //クリア回数が設定回数になったらリザルトに遷移
        if(cleartargetNum >= targetNum)
        {
            SceneManager.LoadScene("Result");
        }
    }

    /// <summary>
    /// プレイヤーがお題をクリアしたのを伝えるために使用
    /// </summary>
    public void TellTargetClear() { ++cleartargetNum; }
}
