using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    [Header("�Q�[���̐ݒ�")]
    [Tooltip("�o�肳����")][SerializeField] private int taskNum;
    [Tooltip("�ۑ�̒B����(�ǐ�)")][SerializeField] private int taskClearCounter;

    [Header("�v���C���[�̏��")]
    private Player player;
    [Tooltip("true:���܂���� / false:�������(�ǐ�)")][SerializeField] private bool spaceKeyState;
    private bool prevSpaceKeyState; //�O��̃X�y�[�X�L�[�̏��
    [Space(10)]

    [Header("�G�̏��")]
    private EnemyMgr enemyMgr;
    [Tooltip("����̓G(�ǐ�)")][SerializeField] private GameObject taskObj;
    [Tooltip("���݂̓G(�ǐ�)")][SerializeField] private GameObject activeEnemyObj;
    [Tooltip("�A�N�e�B�u���ҋ@���Ԃ͈̔�(x:�ŏ����� / y:�ő厞��)")][SerializeField] private Vector2 activateTimeRange;
    [Tooltip("�A�N�e�B�u���ҋ@����(�ǐ�)")][SerializeField]private float activateWaitTime; //�͈̓����_���ŃA�N�e�B�u���߂��o�����Ԃ��i�[����p(-1�Őݒ�Ȃ�)
    [Tooltip("��A�N�e�B�u�ɂ��鎞�Ԃ͈̔�(x:�ŏ����� / y:�ő厞��)")][SerializeField] private Vector2 deactivateTimeRange;
    [Tooltip("�f�A�N�e�B�u�ɂ��鎞��(�ǐ�)")][SerializeField] private float deactiveWaitTime; //�͈̓����_���Ŕ�A�N�e�B�u�ɂ��鎞�Ԃ��i�[����p(-1�Őݒ�Ȃ�)
    [Space(10)]

    [Header("UI�̏��")]
    private Image taskImg;   //����\���̉��u���摜
    [Tooltip("����\������")] [SerializeField] private float showTaskTime;
    private bool showTaskFlag;  //true:�^�X�N�\����/false:�^�X�N��\����
    private GameObject kamaePanel;    //���܂��\���̉��u��

    [Header("���L���Ďg�p")]
    [SerializeField] private float timer;


    void Start()
    {
        //-------------------------------------------------------
        //Game Settings
        //-------------------------------------------------------
        taskClearCounter = 0;

        //-------------------------------------------------------
        //Player
        //-------------------------------------------------------
        //�v���C���[�̃X�N���v�g���m��
        player = GameObject.Find("Player").GetComponent<Player>();

        //-------------------------------------------------------
        //Enemy
        //-------------------------------------------------------
        //�G�̃X�N���v�g���m��
        enemyMgr = GameObject.Find("EnemyMgr").GetComponent<EnemyMgr>();
        activateWaitTime = -1;
        deactiveWaitTime = -1;

        //-------------------------------------------------------
        //UI
        //-------------------------------------------------------
        //����\�����u���̉摜���擾
        taskImg = GameObject.Find("�W�I�\�����u��").GetComponent<Image>();
        taskImg.enabled = false;           //��\���ɂ��Ă���
        kamaePanel = GameObject.Find("kamae");
        kamaePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //���肪�������
        if (CheckTask())
        {
            //����𐶐�
            GenerateTask();
        }
        else
        {
            //���肪�\������Ă��邩
            if(CheckShowTask())
            {
                timer += Time.deltaTime;
                //��莞�Ԃ���������
                if (timer > showTaskTime)
                {
                    //����̕\��������
                    taskImg.enabled = false;
                    //���܂��̑���⏕��\������
                    kamaePanel.SetActive(true);
                    //�^�C�}�[���Z�b�g
                    timer = 0;
                }
            }
            else
            {
                //-------------------------------------------------------
                //�X�y�[�X�L�[��������Ă���(�\���̏��)
                //-------------------------------------------------------
                if (SpaceKeyCheck())
                {
                    //���܂����\������Ă������\���ɂ���
                    if (kamaePanel.active == true)
                    {
                        kamaePanel.SetActive(false);
                    }
                    //-------------------------------------------------------
                    //�G�̃A�N�e�B�u��
                    //-------------------------------------------------------
                    //�A�N�e�B�u�ȓG�����Ȃ����
                    if (activeEnemyObj == null)
                    {
                        //���Ԃ��v��
                        timer += Time.deltaTime;
                        //�����_���Ȏ��ԂŃA�N�e�B�u�����߂�enemymgr�ɏo��(�Ԃ�l�ł�����擾)
                        if (activateWaitTime == -1)
                        {
                            activateWaitTime = Random.Range(activateTimeRange.x, activateTimeRange.y);
                            //�^�C�}�[���Z�b�g
                            timer = 0;
                        }
                        //�͈͂Őݒ肳�ꂽ�҂����Ԃ𒴂�����A�N�e�B�u�����߂�EnemyMgr�ɏo��
                        //+�A�N�e�B�u�������I�u�W�F�N�g��Ԃ�l�Ŏ擾����
                        else if (timer >= activateWaitTime)
                        {
                            activeEnemyObj = enemyMgr.ActivateEnemyRandom();
                            //�҂����Ԃ̐ݒ����̏�Ԃɂ���
                            activateWaitTime = -1;
                            //�^�C�}�[�����Z�b�g
                            timer = 0;
                        }
                    }
                    //-------------------------------------------------------
                    //�A�N�e�B�u�������G���ڕW�ƕs��v(��莞�ԂŔ�A�N�e�B�u�ɂ���)
                    //-------------------------------------------------------
                    else if (activeEnemyObj != taskObj)
                    {
                        //���Ԃ��v��
                        timer += Time.deltaTime;
                        //��A�N�e�B�u�ɂ��鎞�Ԃ��ݒ肳��ĂȂ����
                        if (deactiveWaitTime == -1)
                        {
                            deactiveWaitTime = Random.Range(deactivateTimeRange.x, deactivateTimeRange.y);
                            //�^�C�}�[���Z�b�g
                            timer = 0;
                        }
                        //�A�N�e�B�u�����Ă����A�N�e�B�u�ɂ��鎞�Ԃ��o���Ă�����
                        else if (timer >= deactiveWaitTime)
                        {
                            activeEnemyObj.SetActive(false);
                            activeEnemyObj = null;
                        }
                    }
                }
                else
                {
                    timer += Time.deltaTime;

                    //�^�[�Q�b�g�B���̉��u��
                    if (taskObj == activeEnemyObj)
                    {
                        activeEnemyObj.SetActive(false);
                        activeEnemyObj = null;
                        taskObj = null;
                        ++taskClearCounter;
                    }

                    if (taskClearCounter >= taskNum)
                    {
                        SceneManager.LoadScene("Result");
                    }
                }
            }
           
        }
        


        //����Ɣ�r���A���v����Ώ������v���C���[�̃X�y�[�X���������̂�҂�

        //�����ꂽ�玞�Ԍv�����I��/Init�̓��e�����s/�N���A�񐔂��J�E���g


    }

    //-------------------------------------------------------
    //Private Method
    //-------------------------------------------------------

    /// <summary>
    /// �^�X�N��\�����鏈��
    /// </summary>
    private void ShowTask()
    {
        //�����\������(���u���Ŕ�����ʂ�\�����Ă���)
        taskImg.enabled = true;
    }

    /// <summary>
    /// ���肪�o����Ă��邩�𔻒�
    /// </summary>
    /// <returns>true:�o����Ă��� / false:�o����Ă��Ȃ�</returns>
    private bool CheckTask()
    {
        if(taskObj != null)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// ���肪�\������Ă��邩�𔻒�
    /// </summary>
    /// <returns>true:�\������Ă��� / false:�\������Ă��Ȃ�</returns>
    private bool CheckShowTask()
    {
        if (taskImg.enabled)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// ����̐�����EnemyMgr�Ɏw�����A��������������ʂɕ\������
    /// </summary>
    private void GenerateTask()
    {
        //EnemyMgr�ɂ��萶���̖��߂��o���Ԃ�l�ɂ�������炤
        taskObj = enemyMgr.GenerateTask();
        //�������ʂɕ\������
        taskImg.enabled = true;
    }

    /// <summary>
    /// �O��̃L�[�̏�ԂƔ�r���s��v�Ȃ�X�V���ă^�C�}�[�����Z�b�g����
    /// </summary>
    /// <returns>���݂̃L�[�̏��</returns>
    private bool SpaceKeyCheck()
    {
        if(prevSpaceKeyState != spaceKeyState)
        {
            prevSpaceKeyState = spaceKeyState;
            timer = 0;
        }
        return spaceKeyState;
    }

    /// <summary>
    /// ���݃A�N�e�B�u�ȓG�͂���̓G���𔻒�
    /// </summary>
    /// <returns>true:��v / false:�s��v</returns>
    private bool ActiveEnemyIsTask()
    {
        if(activeEnemyObj == taskObj)
        {
            return true;
        }
        return false;
    }

    //-------------------------------------------------------
    //Public Method For Player
    //-------------------------------------------------------
    /// <summary>
    /// �v���C���[���X�y�[�X���������̂�`���邽�߂Ɏg�p
    /// </summary>
    public void pressSpace() { spaceKeyState = true; }
    /// <summary>
    /// �v���C���[���X�y�[�X�𗣂����̂�`���邽�߂Ɏg�p
    /// </summary>
    public void ReleaseSpace() { spaceKeyState = false; }
}
