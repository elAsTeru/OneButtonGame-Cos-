using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    [Header("ゲームの設定")]
    [Tooltip("課題の達成回数(読専)")][SerializeField] private int clearCount;
    [Tooltip("残基の数")][SerializeField] private int lifeNum;

    [Header("プレイヤーの情報")]
    [Tooltip("C#割り当て確認用(読専)")][SerializeField]private Player player;
    [Tooltip("true:かまえ状態 / false:居合状態(読専)")][SerializeField] private bool spaceKeyState;
    [Space(10)]

    [Header("敵の情報")]
    [Tooltip("C#割り当て確認用(読専)")][SerializeField] private EnemyMgr enemyMgr;
    [Tooltip("お題の敵(読専)")][SerializeField] private GameObject obj_task;
    [Tooltip("現在の敵(読専)")][SerializeField] private GameObject obj_activeEnemy;
    [Tooltip("アクティブ化待機時間の範囲(x:最小時間 / y:最大時間)")][SerializeField] private Vector2 activateWaitTimeRange;
    [Tooltip("設定した範囲からランダムに取られた待機時間 / -1で設定なし(読専)")][SerializeField]private float waitTime;
    [Tooltip("敵を倒すのにかかった時間")][SerializeField] private float clearTime;
    [Tooltip("敵を倒すのにかかった時間の総合")][SerializeField] private float clearTimeTotal;
    [Space(10)]

    [Header("UIの情報")]
    [Tooltip("お題のキャンバス格納")][SerializeField]private GameObject obj_taskCanvas;
    [Tooltip("かまえのキャンバス格納")][SerializeField] private GameObject obj_kamaeCanvas;
    [Tooltip("お題表示時間")] [SerializeField] private float showTaskTime;
    private bool showTaskFlag;  //true:タスク表示中/false:タスク非表示中

    [Header("共有して使用")]
    [SerializeField] private float timer;

    enum GameMode
    {
       GENERATE_TASK,      //お題を生成する
       ACTIVATE_ENEMY,     //敵をアクティブ化する
       GAME_MAIN,          //
       GAME_FAILED,        //失敗
       GAME_SUCCESS,       //成功
       INIT,               //初期化(2回目以降のプレイに必要)
    }
    [Header("ゲーム進行情報")]
    [Tooltip("現在のモード")][SerializeField]private GameMode mode;

    void Start()
    {
        //-------------------------------------------------------
        //Game Settings
        //-------------------------------------------------------

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
        waitTime = -1;

        //-------------------------------------------------------
        //UI
        //-------------------------------------------------------
        //お題キャンバスの取得と非表示
        obj_taskCanvas = GameObject.Find("TaskCanvas").gameObject;
        obj_taskCanvas.SetActive(false);
        //かまえキャンバスをの取得と非表示
        obj_kamaeCanvas = GameObject.Find("KamaeCanvas").gameObject;
        obj_kamaeCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(mode == GameMode.GENERATE_TASK)
        {
            if (timer == 0)
            {
                //プレイヤーをアイドル状態に
                player.SetStateIdol();
                //ランダムお題生成の中から除きたい敵を引数に、お題を生成
                obj_task = enemyMgr.GenerateTask(obj_task);
                //お題を表示
                obj_taskCanvas.SetActive(true);
                //時間の計測開始(これでこのif文内に入らなくなる)
                timer += Time.deltaTime;
            }
            else if (timer <= showTaskTime)
            {
                //時間の計測を続ける
                timer += Time.deltaTime;
            }
            else
            {
                //使い終わったのでリセット
                timer = 0;
                //お題の表示を終わりかまえの操作指示を表示
                obj_taskCanvas.SetActive(false);
                obj_kamaeCanvas.SetActive(true);
                //Activateに移行
                mode = GameMode.ACTIVATE_ENEMY;
            }
        }

        if(mode == GameMode.ACTIVATE_ENEMY && spaceKeyState)
        {
            if (timer == 0)
            {
                //かまえの操作指示を非表示
                obj_kamaeCanvas.SetActive(false);
                //プレイヤーを構え状態に
                player.SetStateKamae();
                //待機時間を設定された範囲内で作成
                waitTime = GenerateTimeRangeRandom(activateWaitTimeRange);
                //時間の計測開始
                timer += Time.deltaTime;
            }
            else if (timer <= waitTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                //使い終わったのでタイマーをリセット
                timer = 0;
                //敵をランダムにアクティブ化
                obj_activeEnemy = enemyMgr.ActivateEnemyRandom();
                //GAME_MAINに移行
                mode = GameMode.GAME_MAIN;
            }
        }

        if (mode == GameMode.GAME_MAIN)
        {
            if (obj_activeEnemy == obj_task)
            {
                //倒すのにかかった時間の記録開始
                clearTime += Time.deltaTime;
                if (spaceKeyState)
                {
                    if (!obj_activeEnemy.activeSelf)
                    {
                        //Failedに移行
                        mode = GameMode.GAME_FAILED;
                    }
                }
                else
                {
                    //プレイヤーを居合状態に
                    player.SetStateIai();
                    //敵の位置を固定
                    obj_activeEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    //Successに移行
                    mode = GameMode.GAME_SUCCESS;
                }
            }
            else
            {
                if (spaceKeyState)
                {
                    if (!obj_activeEnemy.activeSelf)
                    {
                        //Activateに移行
                        mode = GameMode.ACTIVATE_ENEMY;
                    }
                }
                else
                {
                    //プレイヤーを居合状態に
                    player.SetStateIai();
                    //敵の位置を固定
                    obj_activeEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    //Failedに移行
                    mode = GameMode.GAME_FAILED;
                }
            }
        }

        if (mode == GameMode.GAME_FAILED)
        {
            //演出の仮置き
            if (timer <= 3)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                //残基があれば
                if (lifeNum > 0)
                {
                    //1減らし
                    --lifeNum;
                    //Initに移行
                    mode = GameMode.INIT;
                }
                else
                {
                    //Resultに遷移
                    SceneManager.LoadScene("Result");
                }
            }
        }
        else if (mode == GameMode.GAME_SUCCESS)
        {
            //演出仮置き
            if (timer <= 3)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                
                //倒すのにかかった時間の総合を記録
                clearTimeTotal += clearTime;
                //クリア回数をカウント
                ++clearCount;
                //Initに移行
                mode = GameMode.INIT;
            }
        }

        if (mode == GameMode.INIT)
        {
            //演出仮置き時の間必要
            obj_activeEnemy.SetActive(false);

            //倒すのにかかった時間を初期化
            clearTime = 0;
            //Generateに移行
            mode = GameMode.GENERATE_TASK;
        }

        ////お題生成状態
        //if (gameState == GameState.ThemeGenerate)
        //{
        //    //お題を生成
        //    GenerateTask();
        //    player.SetStateIdol();
        //    //デルタタイムを使用する判定を取る前に計測
        //    timer += Time.deltaTime;
        //    //一定時間を経過したら
        //    if (timer > showTaskTime)
        //    {
        //        //お題の表示を消す
        //        taskImg.enabled = false;
        //        //かまえの操作指示を表示
        //        kamaePanel.SetActive(true);
        //        //使用したタイマーをリセットしておく
        //        timer = 0;
        //        //ゲームモードを変更
        //        if (spaceKeyState == false)
        //        {                  
        //            gameState = GameState.Play;
        //        }
        //    }
        //}
        ////ゲームプレイ状態
        //else if (gameState == GameState.Play)
        //{
        //    //スペースキーが押されている(かまえ状態)
        //    if (spaceKeyState == true)
        //    {
        //        //かまえが表示されていたら非表示にする
        //        if (kamaePanel.activeSelf == true)
        //        {
        //            player.SetStateKamae();
        //            kamaePanel.SetActive(false);
        //        }

        //        //アクティブ状態の敵がいなければ
        //        if (activeEnemyObj == null)
        //        {
        //            //待機時間を作成
        //            GenerateTimeRangeRandom(activateWaitTimeRange);
        //            //待機時間を経過したら
        //            if (waitDeltaTime(waitTime))
        //            {
        //                //ランダムでアクティブ化するようにenemyMgrに指示する
        //                activeEnemyObj = enemyMgr.ActivateEnemyRandom();
        //            }
        //        }
        //        else
        //        {
        //            //アクティブ状態の敵がお題と一致
        //            if (activeEnemyObj == taskObj)
        //            {
        //                //成功時にクリアタイムとして使用する時間の計測開始
        //                taskClearTime += Time.deltaTime;
        //                //待機時間を作成
        //                GenerateTimeRangeRandom(deactivateTaskTimeRange);
        //                //待機時間を経過したら
        //                //-------------------------------------------------------
        //                //失敗(スペースキーが押された状態)
        //                //-------------------------------------------------------
        //                if (waitDeltaTime(waitTime))
        //                {
        //                    //失敗処理
        //                    FailedTask();
        //                }
        //            }
        //            //-------------------------------------------------------
        //            //失敗回避(スペースキーが押された状態)
        //            //-------------------------------------------------------
        //            /*処理なし*/
        //        }
        //    }
        //    //スペースキーが押されていない(居合状態) && かまえ指示が非表示
        //    else if (!kamaePanel.activeSelf)
        //    {
        //        player.SetStateIai();
        //        activeEnemyObj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        //        //アクティブ状態の敵がいなければ
        //        if (activeEnemyObj == null)
        //        {
        //            //-------------------------------------------------------
        //            //失敗(スペースキーが離された状態)
        //            //-------------------------------------------------------
        //            //失敗処理
        //            FailedTask();
        //        }
        //        else
        //        {

        //            //敵の位置を固定
        //            /*
        //            * 敵の位置を固定処理を書く
        //            */
        //            //-------------------------------------------------------
        //            //成功(スペースキーが離された状態)
        //            //-------------------------------------------------------

        //            //アクティブ状態の敵がお題と一致
        //            if (activeEnemyObj == taskObj)
        //            {
        //                //成功処理
        //                SucceededTask();
        //                //非アクティブ処理
        //                DeactivateEnemy();
        //            }
        //            //-------------------------------------------------------
        //            //失敗(スペースキーが離された状態)
        //            //-------------------------------------------------------
        //            else
        //            {
        //                //非アクティブ処理
        //                DeactivateEnemy();
        //                //失敗処理
        //                FailedTask();
        //            }
        //        }
        //    }
        //}
        //else if (gameState == GameState.Score)
        //{
        //    //試験用で5秒待機するようにしてる
        //    if (waitDeltaTime(5))
        //    {
        //        gameState = GameState.ThemeGenerate;
        //    }
        //}
    }

    //-------------------------------------------------------
    //Private Method
    //-------------------------------------------------------

    /// <summary>
    /// 設定された範囲でランダムに時間を作成する
    /// </summary>
    /// <param name="_timeRange">ランダムな時間の範囲</param>
    /// <returns>作成された時間</returns>
    private float GenerateTimeRangeRandom(Vector2 _timeRange)
    {
        return Random.Range(_timeRange.x, _timeRange.y);
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
