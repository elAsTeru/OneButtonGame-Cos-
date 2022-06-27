using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskCanvas : MonoBehaviour
{
    [Tooltip("��ʂ��Â����邽�߂̃p�l��(�ǐ�)")][SerializeField] private Image img_blackPanel;
    [Tooltip("����p�l���̃X�N���v�g")][SerializeField] private TaskPanel taskPanel;
    [Tooltip("�����\�����邽�߂̃p�l��")][SerializeField] private Image img_taskPanel;
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
        //�w�i���Â�����(1��̂�)
        img_blackPanel.gameObject.SetActive(true);
        //img_blackPanel.color = SetAlphaColor(150, img_blackPanel.color);
        
        //�����\��(1��̂�)
        taskPanel.SetImagePos(_taskEnemy);
        //�����ł���̕\�����Ԃ�\��(�P��̂�)
        //����̕\�����Ԃ��J�E���g�_�E��(����)
    }


    private Color SetAlphaColor(float _alphaValue, Color _color)
    {
        _color.a = _alphaValue / 255;

        return _color;
    }
}
