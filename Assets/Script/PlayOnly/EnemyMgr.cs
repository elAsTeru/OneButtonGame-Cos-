using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMgr : MonoBehaviour
{
    //敵関連
    [Header("Enemy")]
    [Tooltip("生成するプレハブをアタッチ")][SerializeField] private List<GameObject> enemys;
    [Tooltip("デバッグ用")][SerializeField] private short enemyTypeNum;
    [Tooltip("デバッグ用")][SerializeField] private float timer;

    //アクティブ化関連
    [Header("Activator")]
    [Tooltip("アクティベータをまとめた空間")] private GameObject activateSpace;
    [Tooltip("デバッグ用")][SerializeField] private short activatorNum;

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

        //アクティブ化関連
        activateSpace = this.transform.Find("ActivatePoints").gameObject;  //ジェネレータの空間を検索
        activatorNum = (short)activateSpace.transform.childCount;      //ジェネレータ数を計測

        //オブジェクトプール関連
        objectPool = GameObject.Find("ObjectPool");         //オブジェクトプールを探索
        poolSpace = GameObject.Find("PoolSpace");            //プールオブジェクトを格納する空間を作成
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
    /// <param name="_removeObj">お題にして欲しくない敵を入れる / null:設定なし</param>
    /// <returns>生成したお題(GameObject)</returns>
    public GameObject GenerateTask(GameObject _removeObj)
    {
        //同じにしておくことで条件式の数を減らせる
        GameObject themeObj = _removeObj;
        while (themeObj == _removeObj)
        {
            themeObj = poolSpace.transform.GetChild(Random.Range(0, enemyTypeNum)).gameObject;
        }

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
        //アクティブ化・アクティブ化する場所を設定
        activateSpace.transform.GetChild(Random.Range(0, activatorNum)).GetComponent<ActivatePoint>().Activate(activateEnemyObj);
       
        //アクティブ化した敵オブジェクトを返す
        return activateEnemyObj;
    }

    public GameObject GetPoolSpace()
    {
        return poolSpace;
    }
}