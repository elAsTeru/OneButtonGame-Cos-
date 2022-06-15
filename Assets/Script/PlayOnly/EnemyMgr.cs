using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMgr : MonoBehaviour
{
    //敵関連
    [Header("Enemy")]
    [Tooltip("生成するプレハブをアタッチ")][SerializeField] private List<GameObject> enemys;
    [Tooltip("デバッグ用")][SerializeField] private short enemyTypeNum;
    [Tooltip("デバッグ用")][SerializeField] private GameObject targetEnemy;     //お題になっている敵が入る
    private bool targetEnemyIsActive;
    [Tooltip("デバッグ用")][SerializeField] private float timer;
    [Tooltip("デバッグ用")][SerializeField] private Transform activateEnemy;      //アクティブ化する敵を格納するよう

    //ジェネレータ関連
    [Header("Generator")]
    [Tooltip("ジェネレータをまとめた空間")] private GameObject generatorSpace;
    [Tooltip("デバッグ用")][SerializeField] private short generatorNum;
    [Tooltip("デバッグ用")][SerializeField] private bool isGenerate;      //生成フラグ

    //オブジェクトプール関連
    [Header("Object Pool")]
    private GameObject objectPool;      //オブジェクトプールを使用するためのオブジェクトをアタッチする
    private GameObject poolSpace;     //オブジェクトプールを格納する空間
    [Tooltip("プールのサイズを指定")][SerializeField] private short poolSize;

    // Start is called before the first frame update
    void Start()
    {
        //敵関連
        enemyTypeNum = (short)enemys.Count;                         //敵の種類を計測

        //ジェネレータ関連
        generatorSpace = this.transform.Find("GeneratePoints").gameObject;  //ジェネレータの空間を検索
        generatorNum = (short)generatorSpace.transform.childCount;      //ジェネレータ数を計測

        //オブジェクトプール関連
        objectPool = GameObject.Find("ObjectPool");         //オブジェクトプールを探索
        poolSpace = new GameObject("PoolSpace");            //プールオブジェクトを格納する空間を作成
        poolSpace.transform.parent = this.transform;        //プール空間を子の位置に移動
        for (int i = 0; i < enemyTypeNum; ++i)                      //プールオブジェクトトを生成
        {
            objectPool.GetComponent<ObjectPool>().InsPool(enemys[i], poolSize, poolSpace);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //目標作成
        CreateTarget();

        //新たにアクティブ化したか？
        if(ActivateEnemy())
        {
            //timerをリセット
            timer = 0;
        }

        //時間を計測
        timer += Time.deltaTime;

        //アクティブなのが目標以外なら
        if (!targetEnemyIsActive)
        {
            //2秒後に非アクティブにして、生成停止を復帰する
            if (timer > 2)
            {
                activateEnemy.gameObject.SetActive(false);
                isGenerate = true;
            }
        }
    }

    /// <summary>
    /// 目標が設定されてなければ、敵として登録されている中からランダムに目標を選び登録する。
    /// </summary>
    private void CreateTarget()
    {
        if (targetEnemy == null)
        {
            targetEnemy = poolSpace.transform.GetChild(Random.Range(0, enemyTypeNum)).gameObject;
            isGenerate = true;
        }
    }

    /// <summary>
    /// ジェネレータが稼働なら敵をアクティブ化する
    /// </summary>
    private bool ActivateEnemy()
    {
        if (isGenerate)
        {
            //どの敵をアクティブにするか？
            activateEnemy = poolSpace.transform.GetChild((short)Random.Range(0, enemyTypeNum));
            //敵のアクティブ化位置を設定
            activateEnemy.position = generatorSpace.transform.GetChild(Random.Range(0, generatorNum)).position;
            //敵をアクティブ状態に変更
            activateEnemy.gameObject.SetActive(true);
            //アクティブになった敵は目標ならフラグを立てる
            if (activateEnemy.gameObject == targetEnemy)
            {
                targetEnemyIsActive = true;
            }
            //生成を一時停止する
            isGenerate = false;

            return true;
        }
        return false;
    }

    /// <summary>
    /// 子にアクティブオブジェクトがあるか調べる
    /// </summary>
    /// <param name="_parent">親のトランスフォーム</param>
    /// <returns>true:ある / false:ない</returns>
    private bool FindActiveObjctInChild(Transform _parent)
    {
        foreach(Transform t in _parent)
        {
            if(t.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }
}