using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    [Header("ゲームの設定")]
    [Tooltip("出題される回数")][SerializeField] private int taskNum;
    [Tooltip("課題の達成回数(読専)")][SerializeField] private int taskClearCounter;

    [Header("プレイヤーの情報")]
    private Player player;
    [Tooltip("true:かまえ状態 / false:居合状態(読専)")][SerializeField] private bool spaceKeyState;
    private bool prevSpaceKeyState; //前回のスペースキーの状態
    [Space(10)]

    [Header("敵の情報")]
    private EnemyMgr enemyMgr;
    [Tooltip("お題の敵(読専)")][SerializeField] private GameObject taskObj;
    [Tooltip("現在の敵(読専)")][SerializeField] private GameObject activeEnemyObj;
    [Tooltip("アクティブ化待機時間の範囲(x:最小時間 / y:最大時間)")][SerializeField] private Vector2 activateTimeRange;
    [Tooltip("アクティブ化待機時間(読専)")][SerializeField]private float activateWaitTime; //範囲ランダムでアクティブ命令を出す時間を格納する用(-1で設定なし)
    [Tooltip("非アクティブにする時間の範囲(x:最小時間 / y:最大時間)")][SerializeField] private Vector2 deactivateTimeRange;
    [Tooltip("デアクティブにする時間(読専)")][SerializeField] private float deactiveWaitTime; //範囲ランダムで非アクティブにする時間を格納する用(-1で設定なし)
    [Space(10)]

    [Header("UIの情報")]
    private Image taskImg;   //お題表示の仮置き画像
    [Tooltip("お題表示時間")] [SerializeField] private float showTaskTime;
    private bool showTaskFlag;  //true:タスク表示中/false:タスク非表示中
    private GameObject kamaePanel;    //かまえ表示の仮置き

    [Header("共有して使用")]
    [SerializeField] private float timer;


    void Start()
    {
        //-------------------------------------------------------
        //Game Settings
        //-------------------------------------------------------
        taskClearCounter = 0;

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
        taskImg = GameObject.Find("標的表示仮置き").GetComponent<Image>();
        taskImg.enabled = false;           //非表示にしておく
        kamaePanel = GameObject.Find("kamae");
        kamaePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //お題が無ければ
        if (CheckTask())
        {
            //お題を生成
            GenerateTask();
        }
        else
        {
            //お題が表示されているか
            if(CheckShowTask())
            {
                timer += Time.deltaTime;
                //一定時間をすぎたら
                if (timer > showTaskTime)
                {
                    //お題の表示を消し
                    taskImg.enabled = false;
                    //かまえの操作補助を表示する
                    kamaePanel.SetActive(true);
                    //タイマーリセット
                    timer = 0;
                }
            }
            else
            {
                //-------------------------------------------------------
                //スペースキーが押されている(構えの状態)
                //-------------------------------------------------------
                if (SpaceKeyCheck())
                {
                    //かまえが表示されていたら非表示にする
                    if (kamaePanel.active == true)
                    {
                        kamaePanel.SetActive(false);
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
                    //アクティブ化した敵が目標と不一致(一定時間で非アクティブにする)
                    //-------------------------------------------------------
                    else if (activeEnemyObj != taskObj)
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
                else
                {
                    timer += Time.deltaTime;

                    //ターゲット達成の仮置き
                    if (taskObj == activeEnemyObj)
                    {
                        activeEnemyObj.SetActive(false);
                        activeEnemyObj = null;
                        taskObj = null;
                        ++taskClearCounter;
                    }

                    if (taskClearCounter >= taskNum)
                    {
                        SceneManager.LoadScene("Result");
                    }
                }
            }
           
        }
        


        //お題と比較し、合致すれば消さずプレイヤーのスペースが離されるのを待つ

        //離されたら時間計測を終了/Initの内容を実行/クリア回数をカウント


    }

    //-------------------------------------------------------
    //Private Method
    //-------------------------------------------------------

    /// <summary>
    /// タスクを表示する処理
    /// </summary>
    private void ShowTask()
    {
        //お題を表示する(仮置きで白い画面を表示しておく)
        taskImg.enabled = true;
    }

    /// <summary>
    /// お題が出されているかを判定
    /// </summary>
    /// <returns>true:出されている / false:出されていない</returns>
    private bool CheckTask()
    {
        if(taskObj != null)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// お題が表示されているかを判定
    /// </summary>
    /// <returns>true:表示されている / false:表示されていない</returns>
    private bool CheckShowTask()
    {
        if (taskImg.enabled)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// お題の生成をEnemyMgrに指示し、もらったお題を画面に表示する
    /// </summary>
    private void GenerateTask()
    {
        //EnemyMgrにお題生成の命令を出し返り値にお題をもらう
        taskObj = enemyMgr.GenerateTask();
        //お題を画面に表示する
        taskImg.enabled = true;
    }

    /// <summary>
    /// 前回のキーの状態と比較し不一致なら更新してタイマーをリセットする
    /// </summary>
    /// <returns>現在のキーの状態</returns>
    private bool SpaceKeyCheck()
    {
        if(prevSpaceKeyState != spaceKeyState)
        {
            prevSpaceKeyState = spaceKeyState;
            timer = 0;
        }
        return spaceKeyState;
    }

    /// <summary>
    /// 現在アクティブな敵はお題の敵かを判定
    /// </summary>
    /// <returns>true:一致 / false:不一致</returns>
    private bool ActiveEnemyIsTask()
    {
        if(activeEnemyObj == taskObj)
        {
            return true;
        }
        return false;
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
