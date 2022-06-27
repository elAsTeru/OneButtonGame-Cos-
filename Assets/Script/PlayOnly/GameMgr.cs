using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    [Header("アタッチ・値の設定が必要な項目")]
    [Header("ゲームの設定")]
    [Tooltip("残基の数")][SerializeField] private int lifeNum;

    [Header("時間の設定")]
    [Tooltip("お題表示時間")] [SerializeField] private float showTaskTime;

    [Header("UI関連")]
    [Tooltip("お題のキャンバス")]       [SerializeField]   private GameObject obj_taskCanvas;
    [Tooltip("かまえのキャンバス")]     [SerializeField]   private GameObject obj_kamaeCanvas;
    [Tooltip("途中スコアのキャンバス")] [SerializeField]   private GameObject obj_subResult;
    [Tooltip("失敗のキャンバス")]       [SerializeField]   private GameObject obj_failedCanvas;

    [Header("ゲームの設定")]
    [Tooltip("課題の達成回数(読専)")][SerializeField] private int clearCount;
    [SerializeField] public GameObject staticScore;

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
    private bool showTaskFlag;  //true:タスク表示中/false:タスク非表示中

    [Header("SE関連")]
    [Tooltip("ゲーム開始時にならす効果音")][SerializeField] AudioClip se_printTask;
    [Tooltip("ゲーム開始時にならす効果音")] [SerializeField] AudioClip se_split;
    [Tooltip("ゲーム開始時にならす効果音")] [SerializeField] AudioClip se_failed;
    
    AudioSource audioSource;

    [Header("共有して使用")]
    [SerializeField] private float timer;

    enum GameMode
    {
       TUTORIAL,           //最初の一回だけ実行される
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
        //SE
        //-------------------------------------------------------
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mode == GameMode.TUTORIAL)
        {
            if(timer ==0)
            {
                timer += Time.deltaTime;
            }
            else if(timer <= 3)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                mode = GameMode.GENERATE_TASK;
            }

        }
        if (mode == GameMode.GENERATE_TASK)
        {
            if (timer == 0)
            {
                //プレイヤーをアイドル状態に
                player.SetStateIdol();
                //ランダムお題生成の中から除きたい敵を引数に、お題を生成
                obj_task = enemyMgr.GenerateTask(obj_task);
                timer += Time.deltaTime;
                //お題を表示
                obj_taskCanvas.gameObject.SetActive(true);
                audioSource.clip = se_printTask;
                audioSource.Play();
                obj_taskCanvas.GetComponent<TaskCanvas>().printTask(obj_task);
                //時間の計測開始(これでこのif文内に入らなくなる)
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
                obj_taskCanvas.gameObject.SetActive(false);
                //かまえ表示
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
                    //プレイヤーを居合ミス状態に
                    player.SetStateIaiSuka();
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
            if (timer <= 5)
            {
                if(timer == 0)
                {
                    obj_failedCanvas.SetActive(true);
                }
                audioSource.clip = se_failed;
                audioSource.Play();
                obj_activeEnemy.GetComponent<Enemy>().Failed();
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                //残基があれば
                if (lifeNum > 0)
                {
                    obj_failedCanvas.SetActive(false);
                    //1減らし
                    --lifeNum;
                    //Initに移行
                    mode = GameMode.INIT;
                }
                else
                {
                    staticScore.GetComponent<StaticScore>().ScoreCheck(clearCount, clearTimeTotal);
                    //Resultに遷移
                    SceneManager.LoadScene("Result");
                }
            }
        }
        else if (mode == GameMode.GAME_SUCCESS)
        {
            //演出仮置き
            if (timer <= 5)
            {
                if(timer == 0)
                {
                    audioSource.clip = se_split;
                    audioSource.Play();
                    //敵を分裂させる
                    obj_activeEnemy.GetComponent<Enemy>().Split();
                    //倒すのにかかった時間の総合を記録
                    clearTimeTotal += clearTime;
                    //クリア回数をカウント
                    ++clearCount;
                    //途中のリザルトを表示
                    obj_subResult.gameObject.SetActive(true);
                    obj_subResult.GetComponent<SubResult>().PrintSubScore(clearCount, clearTime);
                }
                timer += Time.deltaTime;

                //プレイヤーが居合位置でアイドルになって、敵が割れる
            }
            else
            {
                timer = 0;
                //スコアの表示を非表示
                obj_subResult.gameObject.SetActive(false);
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
