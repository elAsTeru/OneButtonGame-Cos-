using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticScore : MonoBehaviour
{
    const int rankinguNum = 5;
    private List<int> ranking_score;
    int nowGameEnemyScore, nowGameFastTime;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < rankinguNum; i++)
        {
            ranking_score.Add(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScoreCheck(int enemyNum, int time)
    {

    }
}
