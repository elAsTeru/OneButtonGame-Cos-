using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    [Header("ゲームの設定")]
    [Tooltip("課題の達成回数(読専)")][SerializeField] private int taskClearCounter;
    [Tooltip("残基の数")][SerializeField] private int lifeNum;

    [Header("プレイヤーの情報")]
    [Tooltip("C#割り当て確認用(読専)")][SerializeField]private Player player;
    [Tooltip("true:かまえ状態 / false:居合状態(読専)")][SerializeField] private bool spaceKeyState;
    private bool prevSpaceKeyState; //前回のスペースキーの状態
    [Space(10)]

    [Header("敵の情報")]
    [Tooltip("C#割り当て確認用(読専)")][SerializeField] private EnemyMgr enemyMgr;
    [Tooltip("お題の敵(読専)")][SerializeField] private GameObject taskObj;
    [Tooltip("前回のお題の敵(読専)")][SerializeField] private GameObject prevTaskObj;
    [Tooltip("現在の敵(読専)")][SerializeField] private GameObject activeEnemyObj;
    [Tooltip("アクティブ化待機時間の範囲(x:最小時間 / y:最大時間)")][SerializeField] private Vector2 activateWaitTimeRange;
    [Tooltip("お題以外が消えるまでの時間の範囲(x:最小時間 / y:最大時間)")][SerializeField] private Vector2 deactivateOtherTimeRange;
    [Tooltip("お題が消えるまでの時間の範囲(x:最小時間 / y:最大時間)")][SerializeField] private Vector2 deactivateTaskTimeRange;
    [Tooltip("設定した範囲からランダムに取られた待機時間 / -1で設定なし(読専)")][SerializeField]private float waitTime;
    [Tooltip("敵を倒すのにかかった時間")][SerializeField] private float taskClearTime;
    [Tooltip("敵を倒すのにかかった時間の総合")][SerializeField] private float taskClearTimeTotal;
    [Space(10)]

    [Header("UIの情報")]
    private Image taskImg;   //お題表示の仮置き画像
    [Tooltip("お題表示時間")] [SerializeField] private float showTaskTime;
    private bool showTaskFlag;  //true:タスク表示中/false:タスク非表示中
    private GameObject kamaePanel;    //かまえ表示の仮置き

    [Header("共有して使用")]
    [SerializeField] private float timer;

    enum GameState
    {
        ThemeGenerate,
        Play,
        Score,  //(命名あと回し)
    }
    [Header("ゲーム進行情報")]
    [Tooltip("現在のモード")][SerializeField]private GameState gameState;

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
        waitTime = -1;
        waitTime = -1;

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
        //お題生成状態
        if (gameState == GameState.ThemeGenerate)
        {
            //お題を生成
            GenerateTask();

            //デルタタイムを使用する判定を取る前に計測
            timer += Time.deltaTime;
            //一定時間を経過したら
            if (timer > showTaskTime)
            {
                //お題の表示を消す
                taskImg.enabled = false;
                //かまえの操作指示を表示
                kamaePanel.SetActive(true);
                //使用したタイマーをリセットしておく
                timer = 0;
                //ゲームモードを変更
                if (spaceKeyState == false)
                {
                    gameState = GameState.Play;
                }
            }
        }
        //ゲームプレイ状態
        else if (gameState == GameState.Play)
        {
            //スペースキーが押されている(かまえ状態)
            if (SpaceKeyCheck())
            {
                //かまえが表示されていたら非表示にする
                if (kamaePanel.activeSelf == true)
                {
                    kamaePanel.SetActive(false);
                }

                //アクティブ状態の敵がいなければ
                if (activeEnemyObj == null)
                {
                    //待機時間を作成
                    GenerateTimeRangeRandom(activateWaitTimeRange);
                    //待機時間を経過したら
                    if (waitDeltaTime(waitTime))
                    {
                        //ランダムでアクティブ化するようにenemyMgrに指示する
                        activeEnemyObj = enemyMgr.ActivateEnemyRandom();
                    }
                }
                else
                {
                    //アクティブ状態の敵がお題と一致
                    if (activeEnemyObj == taskObj)
                    {
                        //成功時にクリアタイムとして使用する時間の計測開始
                        taskClearTime += Time.deltaTime;
                        //待機時間を作成
                        GenerateTimeRangeRandom(deactivateTaskTimeRange);
                        //待機時間を経過したら
                        //-------------------------------------------------------
                        //失敗(スペースキーが押された状態)
                        //-------------------------------------------------------
                        if (waitDeltaTime(waitTime))
                        {
                            //非アクティブ処理
                            DeactivateEnemy();
                            //失敗処理
                            FailedTask();
                        }
                    }
                    //-------------------------------------------------------
                    //失敗回避(スペースキーが押された状態)
                    //-------------------------------------------------------
                    else
                    {
                        //待機時間を作成
                        GenerateTimeRangeRandom(deactivateOtherTimeRange);
                        //待機時間を経過したら
                        if (waitDeltaTime(waitTime))
                        {
                            //非アクティブ処理
                            DeactivateEnemy();
                        }
                    }
                }
            }
            //スペースキーが押されていない(居合状態) && かまえ指示が非表示
            else if (!kamaePanel.activeSelf)
            {
                //アクティブ状態の敵がいなければ
                if (activeEnemyObj == null)
                {
                    //-------------------------------------------------------
                    //失敗(スペースキーが離された状態)
                    //-------------------------------------------------------
                    //失敗処理
                    FailedTask();
                }
                else
                {

                    //敵の位置を固定
                    /*
                    * 敵の位置を固定処理を書く
                    */
                    //-------------------------------------------------------
                    //成功(スペースキーが離された状態)
                    //-------------------------------------------------------

                    //アクティブ状態の敵がお題と一致
                    if (activeEnemyObj == taskObj)
                    {
                        //成功処理
                        SucceededTask();
                        //非アクティブ処理
                        DeactivateEnemy();
                    }
                    //-------------------------------------------------------
                    //失敗(スペースキーが離された状態)
                    //-------------------------------------------------------
                    else
                    {
                        //非アクティブ処理
                        DeactivateEnemy();
                        //失敗処理
                        FailedTask();
                    }
                }
            }
        }
        else if (gameState == GameState.Score)
        {
            gameState = GameState.ThemeGenerate;
        }
    }

    //-------------------------------------------------------
    //Private Method
    //-------------------------------------------------------

    /// <summary>
    /// お題が無ければ生成指示をEnemyMgrに出し、もらったお題を画面に表示する
    /// </summary>
    private void GenerateTask()
    {
        if (taskObj == null)
        {
            //EnemyMgrにお題生成の命令を出し返り値にお題をもらう
            taskObj = enemyMgr.GenerateTask(prevTaskObj);
            //前回のお題として記録する
            prevTaskObj = taskObj;
            //お題を画面に表示する
            taskImg.enabled = true;
        }
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
    /// 設定された範囲でランダムに時間を生成する
    /// </summary>
    /// <param name="_timeRange">ランダムな時間の範囲</param>
    private void GenerateTimeRangeRandom(Vector2 _timeRange)
    {
        if (waitTime == -1)
        {
            timer = 0;
            waitTime = Random.Range(_timeRange.x, _timeRange.y);
        }
    }

    /// <summary>
    /// 設定された時間まで処理をスルーするために使用
    /// </summary>
    /// <param name="_waitTime">待つ時間</param>
    /// <returns>true:設定時間経過</returns>
    private bool waitDeltaTime(float _waitTime)
    {
        timer += Time.deltaTime;
        if(timer >= _waitTime)
        {
            timer = 0;
            waitTime = -1;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 前回のアクティブオブジェクトとして記録し非アクティブに変更する
    /// </summary>
    private void DeactivateEnemy()
    {
        //非アクティブにする
        activeEnemyObj.SetActive(false);
        //何も設定されていない状態にする
        activeEnemyObj = null;
    }

    /// <summary>
    /// お題を成功したときに実行すべき処理
    /// </summary>
    private void SucceededTask()
    {
        //クリア回数をカウント
        taskClearCounter++;
        //お題成功時間
        taskClearTime += Time.deltaTime;
        //総合の時間を記録
        taskClearTimeTotal += taskClearTime;
        //次のお題の準備をする
        PrepNextTask();
        //スコアに移行
        gameState = GameState.Score;
    }

    /// <summary>
    /// お題を失敗したときに実行すべき処理
    /// </summary>
    private void FailedTask()
    {
        //残基があれば
        if (lifeNum > 0)
        {
            //残基を減らし
            lifeNum--;
            //次のお題の準備をする
            PrepNextTask();
            //お題取得モードに移行する
            gameState = GameState.ThemeGenerate;
        }
        else
        {
            //リザルトに遷移する
            SceneManager.LoadScene("Result");
        }
    }

    /// <summary>
    /// 次のお題を取得するために実行すべき準備の処理
    /// </summary>
    private void PrepNextTask()
    {
        //お題が解除されてなければ
        if(taskObj != null) { taskObj = null; }
        //タイマーをリセットする
        timer = 0;
        //お題クリアタイムをリセット
        taskClearTime = 0;
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
