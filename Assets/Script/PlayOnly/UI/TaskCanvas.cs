using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskCanvas : MonoBehaviour
{
    [Tooltip("画面を暗くするためのパネル(読専)")][SerializeField] private Image img_blackPanel;
    [Tooltip("お題パネルのスクリプト")][SerializeField] private TaskPanel taskPanel;
    [Tooltip("お題を表示するためのパネル")][SerializeField] private Image img_taskPanel;
    // Start is called before the first frame update
    void Awake()
    {
        img_blackPanel = transform.Find("BlackPanel").GetComponent<Image>();
        img_blackPanel.color = SetAlphaColor(150, img_blackPanel.color);
        img_blackPanel.gameObject.SetActive(false);
        taskPanel = transform.Find("TaskPanel").GetComponent<TaskPanel>();
        img_taskPanel = taskPanel.gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void printTask(GameObject _taskEnemy)
    {
        //背景を暗くする(1回のみ)
        img_blackPanel.gameObject.SetActive(true);
        //img_blackPanel.color = SetAlphaColor(150, img_blackPanel.color);
        
        //お題を表示(1回のみ)
        taskPanel.SetImagePos(_taskEnemy);
        //整数でお題の表示時間を表示(１回のみ)
        //お題の表示時間をカウントダウン(数回)
    }


    private Color SetAlphaColor(float _alphaValue, Color _color)
    {
        _color.a = _alphaValue / 255;

        return _color;
    }
}
