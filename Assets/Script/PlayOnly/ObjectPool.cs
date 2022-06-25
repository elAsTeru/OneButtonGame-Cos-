using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    /// <summary>
    /// オブジェクトプールを作成する関数
    /// </summary>
    /// <param name="_poolingObj">プーリングするオブジェクト</param>
    /// <param name="_size">プールのサイズ</param>
    /// <param name="_insSpace">生成する場所</param>
    public void InsPool(GameObject _poolingObj, int _size, GameObject _insSpace)
    {
        //プールのサイズ分対象のオブジェクトを指定場所に生成する
        for (int i = 0; i < _size; i++)
        {
            Instantiate(_poolingObj, this.transform.position, _poolingObj.transform.rotation, _insSpace.transform);
        }
        //アクティブなオブジェクトを探索して無効化する
        //foreach...要素の数だけループが行われる
        foreach (Transform t in _insSpace.transform)
        {
            if (t.gameObject.activeSelf)
            {
                t.gameObject.SetActive(false);
            }
        }
    }

    GameObject insdObj;
    /// <summary>
    /// プール中からオブジェクトをアクティブにする
    /// </summary>
    /// <param name="_poolingSpace">プールしている場所</param>
    /// <param name="_setPos">アクティブにするときの位置</param>
    public GameObject SetActive(GameObject _poolingSpace, Vector3 _setPos)
    {    
        //非アクティブオブジェクトをプールの中から探索
        foreach (Transform t in _poolingSpace.transform)
        {
            if (!t.gameObject.activeSelf)
            {
                //オブジェクトに位置と回転をセット
                t.SetPositionAndRotation(_setPos, t.transform.rotation);
                //アクティブ化
                t.gameObject.SetActive(true);
                //return で1回しか処理されないようになる
                return t.gameObject;
            }
        }
        foreach(Transform t in _poolingSpace.transform)
        {
            //非アクティブオブジェクトがなかったら新規に生成
            insdObj = Instantiate(t.gameObject, _setPos, t.transform.rotation, _poolingSpace.transform);
        }
        return insdObj;
    }
    /// <summary>
    /// プール内または指定オブジェクトを非アクティブにする関数
    /// </summary>
    /// <param name="_poolingSpace">プールしている場所：指定して非アクティブ化の場合は　null</param>
    /// <param name="_setObj">指定して非アクティブ化：プール内どれでもよければ null</param>
    /// <detail> 引数はどちらかをnullどちらかを指定して使う必要がある </detail>
    public void SetDeactive(GameObject _poolingSpace, GameObject _setObj)
    {
        //実行させないで終了させる場合
        if(_poolingSpace && _setObj || !_poolingSpace && !_setObj)
        {
            Debug.Log("非アクティブ化に失敗しました。引数が両方ともnullかセットされています。片方のみにしてください");
            Application.Quit();
        }
        //指定されていたら指定されているオブジェクトを非アクティブ化
        if(_setObj)
        {
            _setObj.SetActive(false);
            return;
        }
        //アクティブなオブジェクトをプールの中から探索
        foreach (Transform t in _poolingSpace.transform)
        {
            if (t.gameObject.activeSelf)
            {
                //非アクティブ化
                t.gameObject.SetActive(false);
                //非アクティブにしたら終了
                return;
            }
        }
    }

    /// <summary>
    /// プール内のオブジェクトの数を数えて返す関数
    /// </summary>
    /// <param name="_poolingSpace"></param>
    /// <returns></returns>
    public int CountPoolingObj(GameObject _poolingSpace)
    {
        int count = 0;
        //プール内からアクティブなオブジェクトを探索する
        foreach(Transform t in _poolingSpace.transform)
        {
            if(t.gameObject.activeSelf)
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// 子にアクティブオブジェクトがあるか調べる
    /// </summary>
    /// <param name="_parent">親のトランスフォーム</param>
    /// <returns>アクティブオブジェクトを返す</returns>
    public GameObject FindActiveObjctInChild(Transform _parent)
    {
        foreach (Transform t in _parent)
        {
            if (t.gameObject.activeSelf)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}
