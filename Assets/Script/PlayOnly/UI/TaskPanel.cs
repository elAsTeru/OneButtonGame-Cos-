using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel : MonoBehaviour
{
    [SerializeField] private GameObject taskPos;
    [SerializeField] private GameObject otherPos1;
    [SerializeField] private GameObject otherPos2;

    [SerializeField] private GameObject obj_enemy1;
    [SerializeField] private GameObject obj_enemy2;
    [SerializeField] private GameObject obj_enemy3;

    [SerializeField]private GameObject objectPool;

    void Start()
    {
        objectPool = GameObject.Find("PoolSpace");
    }

    public void SetImagePos(GameObject _taskEnemy)
    {
        
        if(_taskEnemy == objectPool.transform.GetChild(0).gameObject)
        {
            obj_enemy1.transform.position = taskPos.transform.position;
            obj_enemy2.transform.position = otherPos1.transform.position;
            obj_enemy3.transform.position = otherPos2.transform.position;
        }
        else if (_taskEnemy == objectPool.transform.GetChild(1).gameObject)
        {
            obj_enemy2.transform.position = taskPos.transform.position;
            obj_enemy1.transform.position = otherPos1.transform.position;
            obj_enemy3.transform.position = otherPos2.transform.position;
        }
        else if (_taskEnemy == objectPool.transform.GetChild(2).gameObject)
        {
            obj_enemy3.transform.position = taskPos.transform.position;
            obj_enemy1.transform.position = otherPos1.transform.position;
            obj_enemy2.transform.position = otherPos2.transform.position;
        }
    }
}
