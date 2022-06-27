using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel : MonoBehaviour
{
    [Header("読み専用の項目")]
    [Tooltip("0:お題/1,2:それ以外の座標")][SerializeField] private List<Vector3> posList;
    [Tooltip("お題で表示する敵の座標系")][SerializeField] private List<Transform> tf_enemys;
    [Tooltip("引数のお題と比較に使用")][SerializeField]private Transform tr_objectPool;

    private void Awake()
    {
        //お題表示位置においてある空のオブジェクトから座標を取得
        posList.Add(GameObject.Find("TaskEnemyPos").transform.position);
        posList.Add(GameObject.Find("OtherEnemyPos0").transform.position);
        posList.Add(GameObject.Find("OtherEnemyPos1").transform.position);
        //お題表示に使用するObjの座標系を取得
        tf_enemys.Add(GameObject.Find("enemy0").transform);
        tf_enemys.Add(GameObject.Find("enemy1").transform);
        tf_enemys.Add(GameObject.Find("enemy2").transform);

        //お題の敵か比較するために使用する
        tr_objectPool = GameObject.Find("PoolSpace").transform;
    }

    public void SetImagePos(GameObject _taskEnemy)
    {
        for(int i = 0; i < tf_enemys.Count; i++)
        {
            if(_taskEnemy == tr_objectPool.GetChild(i).gameObject)
            {
                tf_enemys[i].position = posList[0];
                if (i == 0)
                {
                    tf_enemys[1].position = posList[1];
                    tf_enemys[2].position = posList[2];
                }
                else if (i == 1)
                {
                    tf_enemys[0].position = posList[1];
                    tf_enemys[2].position = posList[2];
                }
                else if(i == 2)
                {
                    tf_enemys[0].position = posList[1];
                    tf_enemys[1].position = posList[2];
                }
                break;
            }
        }
    }
}
