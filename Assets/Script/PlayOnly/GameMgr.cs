using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    [Header("�Q�[���̐ݒ�")]
    [Tooltip("�ۑ�̒B����(�ǐ�)")][SerializeField] private int taskClearCounter;
    [Tooltip("�c��̐�")][SerializeField] private int lifeNum;

    [Header("�v���C���[�̏��")]
    [Tooltip("C#���蓖�Ċm�F�p(�ǐ�)")][SerializeField]private Player player;
    [Tooltip("true:���܂���� / false:�������(�ǐ�)")][SerializeField] private bool spaceKeyState;
    private bool prevSpaceKeyState; //�O��̃X�y�[�X�L�[�̏��
    [Space(10)]

    [Header("�G�̏��")]
    [Tooltip("C#���蓖�Ċm�F�p(�ǐ�)")][SerializeField] private EnemyMgr enemyMgr;
    [Tooltip("����̓G(�ǐ�)")][SerializeField] private GameObject taskObj;
    [Tooltip("�O��̂���̓G(�ǐ�)")][SerializeField] private GameObject prevTaskObj;
    [Tooltip("���݂̓G(�ǐ�)")][SerializeField] private GameObject activeEnemyObj;
    [Tooltip("�A�N�e�B�u���ҋ@���Ԃ͈̔�(x:�ŏ����� / y:�ő厞��)")][SerializeField] private Vector2 activateWaitTimeRange;
    [Tooltip("����ȊO��������܂ł̎��Ԃ͈̔�(x:�ŏ����� / y:�ő厞��)")][SerializeField] private Vector2 deactivateOtherTimeRange;
    [Tooltip("���肪������܂ł̎��Ԃ͈̔�(x:�ŏ����� / y:�ő厞��)")][SerializeField] private Vector2 deactivateTaskTimeRange;
    [Tooltip("�ݒ肵���͈͂��烉���_���Ɏ��ꂽ�ҋ@���� / -1�Őݒ�Ȃ�(�ǐ�)")][SerializeField]private float waitTime;
    [Tooltip("�G��|���̂ɂ�����������")][SerializeField] private float taskClearTime;
    [Tooltip("�G��|���̂ɂ����������Ԃ̑���")][SerializeField] private float taskClearTimeTotal;
    [Space(10)]

    [Header("UI�̏��")]
    private Image taskImg;   //����\���̉��u���摜
    [Tooltip("����\������")] [SerializeField] private float showTaskTime;
    private bool showTaskFlag;  //true:�^�X�N�\����/false:�^�X�N��\����
    private GameObject kamaePanel;    //���܂��\���̉��u��

    [Header("���L���Ďg�p")]
    [SerializeField] private float timer;

    enum GameState
    {
        ThemeGenerate,
        Play,
        Score,  //(�������Ɖ�)
    }
    [Header("�Q�[���i�s���")]
    [Tooltip("���݂̃��[�h")][SerializeField]private GameState gameState;

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
        waitTime = -1;
        waitTime = -1;

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
        //���萶�����
        if (gameState == GameState.ThemeGenerate)
        {
            //����𐶐�
            GenerateTask();

            //�f���^�^�C�����g�p���锻������O�Ɍv��
            timer += Time.deltaTime;
            //��莞�Ԃ��o�߂�����
            if (timer > showTaskTime)
            {
                //����̕\��������
                taskImg.enabled = false;
                //���܂��̑���w����\��
                kamaePanel.SetActive(true);
                //�g�p�����^�C�}�[�����Z�b�g���Ă���
                timer = 0;
                //�Q�[�����[�h��ύX
                if (spaceKeyState == false)
                {
                    gameState = GameState.Play;
                }
            }
        }
        //�Q�[���v���C���
        else if (gameState == GameState.Play)
        {
            //�X�y�[�X�L�[��������Ă���(���܂����)
            if (SpaceKeyCheck())
            {
                //���܂����\������Ă������\���ɂ���
                if (kamaePanel.activeSelf == true)
                {
                    kamaePanel.SetActive(false);
                }

                //�A�N�e�B�u��Ԃ̓G�����Ȃ����
                if (activeEnemyObj == null)
                {
                    //�ҋ@���Ԃ��쐬
                    GenerateTimeRangeRandom(activateWaitTimeRange);
                    //�ҋ@���Ԃ��o�߂�����
                    if (waitDeltaTime(waitTime))
                    {
                        //�����_���ŃA�N�e�B�u������悤��enemyMgr�Ɏw������
                        activeEnemyObj = enemyMgr.ActivateEnemyRandom();
                    }
                }
                else
                {
                    //�A�N�e�B�u��Ԃ̓G������ƈ�v
                    if (activeEnemyObj == taskObj)
                    {
                        //�������ɃN���A�^�C���Ƃ��Ďg�p���鎞�Ԃ̌v���J�n
                        taskClearTime += Time.deltaTime;
                        //�ҋ@���Ԃ��쐬
                        GenerateTimeRangeRandom(deactivateTaskTimeRange);
                        //�ҋ@���Ԃ��o�߂�����
                        //-------------------------------------------------------
                        //���s(�X�y�[�X�L�[�������ꂽ���)
                        //-------------------------------------------------------
                        if (waitDeltaTime(waitTime))
                        {
                            //��A�N�e�B�u����
                            DeactivateEnemy();
                            //���s����
                            FailedTask();
                        }
                    }
                    //-------------------------------------------------------
                    //���s���(�X�y�[�X�L�[�������ꂽ���)
                    //-------------------------------------------------------
                    else
                    {
                        //�ҋ@���Ԃ��쐬
                        GenerateTimeRangeRandom(deactivateOtherTimeRange);
                        //�ҋ@���Ԃ��o�߂�����
                        if (waitDeltaTime(waitTime))
                        {
                            //��A�N�e�B�u����
                            DeactivateEnemy();
                        }
                    }
                }
            }
            //�X�y�[�X�L�[��������Ă��Ȃ�(�������) && ���܂��w������\��
            else if (!kamaePanel.activeSelf)
            {
                //�A�N�e�B�u��Ԃ̓G�����Ȃ����
                if (activeEnemyObj == null)
                {
                    //-------------------------------------------------------
                    //���s(�X�y�[�X�L�[�������ꂽ���)
                    //-------------------------------------------------------
                    //���s����
                    FailedTask();
                }
                else
                {

                    //�G�̈ʒu���Œ�
                    /*
                    * �G�̈ʒu���Œ菈��������
                    */
                    //-------------------------------------------------------
                    //����(�X�y�[�X�L�[�������ꂽ���)
                    //-------------------------------------------------------

                    //�A�N�e�B�u��Ԃ̓G������ƈ�v
                    if (activeEnemyObj == taskObj)
                    {
                        //��������
                        SucceededTask();
                        //��A�N�e�B�u����
                        DeactivateEnemy();
                    }
                    //-------------------------------------------------------
                    //���s(�X�y�[�X�L�[�������ꂽ���)
                    //-------------------------------------------------------
                    else
                    {
                        //��A�N�e�B�u����
                        DeactivateEnemy();
                        //���s����
                        FailedTask();
                    }
                }
            }
        }
        else if (gameState == GameState.Score)
        {
            gameState = GameState.ThemeGenerate;
        }
    }

    //-------------------------------------------------------
    //Private Method
    //-------------------------------------------------------

    /// <summary>
    /// ���肪������ΐ����w����EnemyMgr�ɏo���A��������������ʂɕ\������
    /// </summary>
    private void GenerateTask()
    {
        if (taskObj == null)
        {
            //EnemyMgr�ɂ��萶���̖��߂��o���Ԃ�l�ɂ�������炤
            taskObj = enemyMgr.GenerateTask(prevTaskObj);
            //�O��̂���Ƃ��ċL�^����
            prevTaskObj = taskObj;
            //�������ʂɕ\������
            taskImg.enabled = true;
        }
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
    /// �ݒ肳�ꂽ�͈͂Ń����_���Ɏ��Ԃ𐶐�����
    /// </summary>
    /// <param name="_timeRange">�����_���Ȏ��Ԃ͈̔�</param>
    private void GenerateTimeRangeRandom(Vector2 _timeRange)
    {
        if (waitTime == -1)
        {
            timer = 0;
            waitTime = Random.Range(_timeRange.x, _timeRange.y);
        }
    }

    /// <summary>
    /// �ݒ肳�ꂽ���Ԃ܂ŏ������X���[���邽�߂Ɏg�p
    /// </summary>
    /// <param name="_waitTime">�҂���</param>
    /// <returns>true:�ݒ莞�Ԍo��</returns>
    private bool waitDeltaTime(float _waitTime)
    {
        timer += Time.deltaTime;
        if(timer >= _waitTime)
        {
            timer = 0;
            waitTime = -1;
            return true;
        }
        return false;
    }

    /// <summary>
    /// �O��̃A�N�e�B�u�I�u�W�F�N�g�Ƃ��ċL�^����A�N�e�B�u�ɕύX����
    /// </summary>
    private void DeactivateEnemy()
    {
        //��A�N�e�B�u�ɂ���
        activeEnemyObj.SetActive(false);
        //�����ݒ肳��Ă��Ȃ���Ԃɂ���
        activeEnemyObj = null;
    }

    /// <summary>
    /// ����𐬌������Ƃ��Ɏ��s���ׂ�����
    /// </summary>
    private void SucceededTask()
    {
        //�N���A�񐔂��J�E���g
        taskClearCounter++;
        //���萬������
        taskClearTime += Time.deltaTime;
        //�����̎��Ԃ��L�^
        taskClearTimeTotal += taskClearTime;
        //���̂���̏���������
        PrepNextTask();
        //�X�R�A�Ɉڍs
        gameState = GameState.Score;
    }

    /// <summary>
    /// ��������s�����Ƃ��Ɏ��s���ׂ�����
    /// </summary>
    private void FailedTask()
    {
        //�c������
        if (lifeNum > 0)
        {
            //�c������炵
            lifeNum--;
            //���̂���̏���������
            PrepNextTask();
            //����擾���[�h�Ɉڍs����
            gameState = GameState.ThemeGenerate;
        }
        else
        {
            //���U���g�ɑJ�ڂ���
            SceneManager.LoadScene("Result");
        }
    }

    /// <summary>
    /// ���̂�����擾���邽�߂Ɏ��s���ׂ������̏���
    /// </summary>
    private void PrepNextTask()
    {
        //���肪��������ĂȂ����
        if(taskObj != null) { taskObj = null; }
        //�^�C�}�[�����Z�b�g����
        timer = 0;
        //����N���A�^�C�������Z�b�g
        taskClearTime = 0;
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
