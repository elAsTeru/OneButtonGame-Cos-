using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel : MonoBehaviour
{
    [Header("�ǂݐ�p�̍���")]
    [Tooltip("0:����/1,2:����ȊO�̍��W")][SerializeField] private List<Vector3> posList;
    [Tooltip("����ŕ\������G�̍��W�n")][SerializeField] private List<Transform> tf_enemys;
    [Tooltip("�����̂���Ɣ�r�Ɏg�p")][SerializeField]private Transform tr_objectPool;

    private void Awake()
    {
        //����\���ʒu�ɂ����Ă����̃I�u�W�F�N�g������W���擾
        posList.Add(GameObject.Find("TaskEnemyPos").transform.position);
        posList.Add(GameObject.Find("OtherEnemyPos0").transform.position);
        posList.Add(GameObject.Find("OtherEnemyPos1").transform.position);
        //����\���Ɏg�p����Obj�̍��W�n���擾
        tf_enemys.Add(GameObject.Find("enemy0").transform);
        tf_enemys.Add(GameObject.Find("enemy1").transform);
        tf_enemys.Add(GameObject.Find("enemy2").transform);

        //����̓G����r���邽�߂Ɏg�p����
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
