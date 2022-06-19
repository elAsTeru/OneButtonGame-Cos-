using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    [Header("プレイヤーの情報")]
    [Tooltip("アタッチ確認用(読専)")][SerializeField] private Player player;
    [Tooltip("true:かまえ状態 / false:居合状態(読専)")][SerializeField] private bool spaceKeyState;
    private bool prevSpaceKeyState; //前回のスペースキーの状態
    [Space(10)]

    [Header("敵の情報")]
    [Tooltip("アタッチ確認用(読専)")][SerializeField] private EnemyMgr enemyMgr;
    [Tooltip("お題の敵(読専)")][SerializeField] private GameObject themeObj;
    [Tooltip("現在の敵(読専)")][SerializeField] private GameObject activeEnemyObj;
    [Tooltip("アクティブ化待機時間の範囲(x:最小時間 / y:最大時間)")][SerializeField] private Vector2 activateTimeRange;
    [Tooltip("アクティブ化待機時間(読専)")][SerializeField]private float activateWaitTime; //範囲ランダムでアクティブ命令を出す時間を格納する用(-1で設定なし)
    [Tooltip("非アクティブにする時間の範囲(x:最小時間 / y:最大時間)")][SerializeField] private Vector2 deactivateTimeRange;
    [Tooltip("デアクティブにする時間(読専)")][SerializeField] private float deactiveWaitTime; //範囲ランダムで非アクティブにする時間を格納する用(-1で設定なし)
    [Space(10)]

    [Header("UIの情報")]
    [Tooltip("お題表示仮置き画像")][SerializeField] private Image themeImg;

    [Header("共有して使用")]
    [SerializeField] private float timer;

    // Start is called before the first frame update
    void Start()
    {
        ////クリア回数を0にする
        //cleartargetNum = 0;

        //-------------------------------------------------------
        //Player
        //-------------------------------------------------------
        //プレイヤーのスクリプトを確保
        player = GameObject.Find("Player").GetComponent<Player>();

        //-------------------------------------------------------
        //Enemy
        //-------------------------------------------------------
        //敵のスクリプトを確保
        enemyMgr = GameObject.Find("EnemyMgr").GetComponent<EnemyMgr>();
        activateWaitTime = -1;
        deactiveWaitTime = -1;

        //-------------------------------------------------------
        //UI
        //-------------------------------------------------------
        //お題表示仮置きの画像を取得
        themeImg = GameObject.Find("標的表示仮置き").GetComponent<Image>();
        themeImg.enabled = false;           //非表示にしておく
    }

    // Update is called once per frame
    void Update()
    {
        //お題が無ければ
        if (themeObj == null) { Init(); }
        //-------------------------------------------------------
        //スペースキーが押されている(構えの状態)
        //-------------------------------------------------------
        if (spaceKeyState == true)
        {
            //スペースキーの状態が切り替わったときに１回だけ実行
            if(spaceKeyState != prevSpaceKeyState)
            {
                prevSpaceKeyState = spaceKeyState;
                timer = 0;
            }
            //お題が表示されていたら非表示にする
            if (themeImg.enabled == true)
            {
                themeImg.enabled = false;
            }
            //-------------------------------------------------------
            //敵のアクティブ化
            //-------------------------------------------------------
            //アクティブな敵がいなければ
            if (activeEnemyObj == null)
            {
                //時間を計測
                timer += Time.deltaTime;
                //ランダムな時間でアクティブ化命令をenemymgrに出す(返り値でそれを取得)
                if (activateWaitTime == -1)
                {
                    activateWaitTime = Random.Range(activateTimeRange.x, activateTimeRange.y);
                    //タイマーリセット
                    timer = 0;
                }
                //範囲で設定された待ち時間を超えたらアクティブ化命令をEnemyMgrに出す
                //+アクティブ化したオブジェクトを返り値で取得する
                else if (timer >= activateWaitTime)
                {
                    activeEnemyObj = enemyMgr.ActivateEnemyRandom();
                    //待ち時間の設定を空の状態にする
                    activateWaitTime = -1;
                    //タイマーをリセット
                    timer = 0;
                }
            }
            //-------------------------------------------------------
            //アクティブ化した敵が目標と不一致
            //-------------------------------------------------------
            else if (activeEnemyObj != themeObj)
            {
                //時間を計測
                timer += Time.deltaTime;
                //非アクティブにする時間が設定されてなければ
                if (deactiveWaitTime == -1)
                {
                    deactiveWaitTime = Random.Range(deactivateTimeRange.x, deactivateTimeRange.y);
                    //タイマーリセット
                    timer = 0;
                }
                //アクティブ化してから非アクティブにする時間が経っていたら
                else if (timer >= deactiveWaitTime)
                {
                    activeEnemyObj.SetActive(false);
                    activeEnemyObj = null;
                }
            }
        }
        //-------------------------------------------------------
        //スペースキーが離された(居合の状態)
        //-------------------------------------------------------
        else if(themeObj != null)
        {
            //スペースキーの状態が切り替わったときに１回だけ実行
            if (spaceKeyState != prevSpaceKeyState)
            {
                prevSpaceKeyState = spaceKeyState;
                timer = 0;
            }
            timer += Time.deltaTime;

            //ターゲット達成の仮置き
            if(themeObj == activeEnemyObj)
            {
                activeEnemyObj.SetActive(false);
                activeEnemyObj = null;
                themeObj = null;
            }
        }

        //お題と比較し、違えば一定時間で非アクティブに

        //お題と比較し、合致すれば消さずプレイヤーのスペースが離されるのを待つ

        //離されたら時間計測を終了/Initの内容を実行/クリア回数をカウント




        ////もし標的表示仮置きが表示されていたら
        //if (printTargetObj.GetComponent<Image>().enabled)
        //{
        //    //スペースが押されたら
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        //標的表示仮置きを非表示に
        //        printTargetObj.GetComponent<Image>().enabled = false;
        //        //プレイヤーの処理を開始
        //        player.enabled = true;      //特定のコンポーネントやスクリプトが有効かを変える方法
        //    }
        //}
        //else
        //{
        //    if (spaceKeyState == true)
        //    {
        //        //敵管理の処理を開始する
        //        enemyGenMgr.SetActive(false);    //全体のアクティブを変える方法
        //    }
        //    else
        //    {
        //        //敵管理の処理を開始する
        //        enemyGenMgr.SetActive(true);    //全体のアクティブを変える方法
        //    }
        //}

        ////クリア回数が設定回数になったらリザルトに遷移
        //if(cleartargetNum >= targetNum)
        //{
        //    SceneManager.LoadScene("Result");
        //}
    }

    //-------------------------------------------------------
    //Private Method
    //-------------------------------------------------------
    /// <summary>
    /// #実行内容
    /// -お題生成
    /// -お題表示
    /// </summary>
    private void Init()
    {
        //EnemyMgrにお題生成の命令を出す(gameobjct型かstring型で作られたお題を返すもの)
        themeObj = enemyMgr.GenerateTheme();
        //お題を表示する(仮置きで白い画面を表示しておく)
        themeImg.enabled = true;
    }

    //-------------------------------------------------------
    //Public Method For Player
    //-------------------------------------------------------
    /// <summary>
    /// プレイヤーがスペースを押したのを伝えるために使用
    /// </summary>
    public void pressSpace() { spaceKeyState = true; }
    /// <summary>
    /// プレイヤーがスペースを離したのを伝えるために使用
    /// </summary>
    public void ReleaseSpace() { spaceKeyState = false; }
}
