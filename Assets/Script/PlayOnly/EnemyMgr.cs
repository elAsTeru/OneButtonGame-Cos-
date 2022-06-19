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

    //-------------------------------------------------------
    //Public Method For GameMgr
    //-------------------------------------------------------
    /// <summary>
    /// お題生成の指示を受け生成する
    /// </summary>
    /// <returns>生成したお題(GameObject)</returns>
    public GameObject GenerateTask()
    {
        GameObject themeObj;

        themeObj = poolSpace.transform.GetChild(Random.Range(0, enemyTypeNum)).gameObject;

        return themeObj;
    }

    /// <summary>
    /// アクティブ化の指示を受けランダムでアクティブ化する
    /// </summary>
    /// <returns>アクティブ化した敵(GameObject)</returns>
    public GameObject ActivateEnemyRandom()
    {
        GameObject activateEnemyObj;
        //どの敵をアクティブ化するか？
        activateEnemyObj = poolSpace.transform.GetChild((short)Random.Range(0, enemyTypeNum)).gameObject;
        //アクティブ化する場所を設定
        activateEnemyObj.transform.position = generatorSpace.transform.GetChild(Random.Range(0, generatorNum)).position;
        //アクティブ化
        activateEnemyObj.SetActive(true);
        //アクティブ化した敵オブジェクトを返す
        return activateEnemyObj;
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