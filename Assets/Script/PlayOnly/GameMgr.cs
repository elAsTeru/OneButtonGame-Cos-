using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    [Header("�Q�[���̐ݒ�")]
    [Tooltip("�ۑ�̒B����(�ǐ�)")][SerializeField] private int clearCount;
    [Tooltip("�c��̐�")][SerializeField] private int lifeNum;

    [Header("�v���C���[�̏��")]
    [Tooltip("C#���蓖�Ċm�F�p(�ǐ�)")][SerializeField]private Player player;
    [Tooltip("true:���܂���� / false:�������(�ǐ�)")][SerializeField] private bool spaceKeyState;
    [Space(10)]

    [Header("�G�̏��")]
    [Tooltip("C#���蓖�Ċm�F�p(�ǐ�)")][SerializeField] private EnemyMgr enemyMgr;
    [Tooltip("����̓G(�ǐ�)")][SerializeField] private GameObject obj_task;
    [Tooltip("���݂̓G(�ǐ�)")][SerializeField] private GameObject obj_activeEnemy;
    [Tooltip("�A�N�e�B�u���ҋ@���Ԃ͈̔�(x:�ŏ����� / y:�ő厞��)")][SerializeField] private Vector2 activateWaitTimeRange;
    [Tooltip("�ݒ肵���͈͂��烉���_���Ɏ��ꂽ�ҋ@���� / -1�Őݒ�Ȃ�(�ǐ�)")][SerializeField]private float waitTime;
    [Tooltip("�G��|���̂ɂ�����������")][SerializeField] private float clearTime;
    [Tooltip("�G��|���̂ɂ����������Ԃ̑���")][SerializeField] private float clearTimeTotal;
    [Space(10)]

    [Header("UI�̏��")]
    [Tooltip("����̃L�����o�X�i�[")][SerializeField]private GameObject obj_taskCanvas;
    [Tooltip("���܂��̃L�����o�X�i�[")][SerializeField] private GameObject obj_kamaeCanvas;
    [Tooltip("����\������")] [SerializeField] private float showTaskTime;
    private bool showTaskFlag;  //true:�^�X�N�\����/false:�^�X�N��\����

    [Header("���L���Ďg�p")]
    [SerializeField] private float timer;

    enum GameMode
    {
       GENERATE_TASK,      //����𐶐�����
       ACTIVATE_ENEMY,     //�G���A�N�e�B�u������
       GAME_MAIN,          //
       GAME_FAILED,        //���s
       GAME_SUCCESS,       //����
       INIT,               //������(2��ڈȍ~�̃v���C�ɕK�v)
    }
    [Header("�Q�[���i�s���")]
    [Tooltip("���݂̃��[�h")][SerializeField]private GameMode mode;

    void Start()
    {
        //-------------------------------------------------------
        //Game Settings
        //-------------------------------------------------------

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

        //-------------------------------------------------------
        //UI
        //-------------------------------------------------------
        //����L�����o�X�̎擾�Ɣ�\��
        obj_taskCanvas = GameObject.Find("TaskCanvas").gameObject;
        obj_taskCanvas.SetActive(false);
        //���܂��L�����o�X���̎擾�Ɣ�\��
        obj_kamaeCanvas = GameObject.Find("KamaeCanvas").gameObject;
        obj_kamaeCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(mode == GameMode.GENERATE_TASK)
        {
            if (timer == 0)
            {
                //�v���C���[���A�C�h����Ԃ�
                player.SetStateIdol();
                //�����_�����萶���̒����珜�������G�������ɁA����𐶐�
                obj_task = enemyMgr.GenerateTask(obj_task);
                //�����\��
                obj_taskCanvas.SetActive(true);
                //���Ԃ̌v���J�n(����ł���if�����ɓ���Ȃ��Ȃ�)
                timer += Time.deltaTime;
            }
            else if (timer <= showTaskTime)
            {
                //���Ԃ̌v���𑱂���
                timer += Time.deltaTime;
            }
            else
            {
                //�g���I������̂Ń��Z�b�g
                timer = 0;
                //����̕\�����I��肩�܂��̑���w����\��
                obj_taskCanvas.SetActive(false);
                obj_kamaeCanvas.SetActive(true);
                //Activate�Ɉڍs
                mode = GameMode.ACTIVATE_ENEMY;
            }
        }

        if(mode == GameMode.ACTIVATE_ENEMY && spaceKeyState)
        {
            if (timer == 0)
            {
                //���܂��̑���w�����\��
                obj_kamaeCanvas.SetActive(false);
                //�v���C���[���\����Ԃ�
                player.SetStateKamae();
                //�ҋ@���Ԃ�ݒ肳�ꂽ�͈͓��ō쐬
                waitTime = GenerateTimeRangeRandom(activateWaitTimeRange);
                //���Ԃ̌v���J�n
                timer += Time.deltaTime;
            }
            else if (timer <= waitTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                //�g���I������̂Ń^�C�}�[�����Z�b�g
                timer = 0;
                //�G�������_���ɃA�N�e�B�u��
                obj_activeEnemy = enemyMgr.ActivateEnemyRandom();
                //GAME_MAIN�Ɉڍs
                mode = GameMode.GAME_MAIN;
            }
        }

        if (mode == GameMode.GAME_MAIN)
        {
            if (obj_activeEnemy == obj_task)
            {
                //�|���̂ɂ����������Ԃ̋L�^�J�n
                clearTime += Time.deltaTime;
                if (spaceKeyState)
                {
                    if (!obj_activeEnemy.activeSelf)
                    {
                        //Failed�Ɉڍs
                        mode = GameMode.GAME_FAILED;
                    }
                }
                else
                {
                    //�v���C���[��������Ԃ�
                    player.SetStateIai();
                    //�G�̈ʒu���Œ�
                    obj_activeEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    //Success�Ɉڍs
                    mode = GameMode.GAME_SUCCESS;
                }
            }
            else
            {
                if (spaceKeyState)
                {
                    if (!obj_activeEnemy.activeSelf)
                    {
                        //Activate�Ɉڍs
                        mode = GameMode.ACTIVATE_ENEMY;
                    }
                }
                else
                {
                    //�v���C���[��������Ԃ�
                    player.SetStateIai();
                    //�G�̈ʒu���Œ�
                    obj_activeEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    //Failed�Ɉڍs
                    mode = GameMode.GAME_FAILED;
                }
            }
        }

        if (mode == GameMode.GAME_FAILED)
        {
            //���o�̉��u��
            if (timer <= 3)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                //�c������
                if (lifeNum > 0)
                {
                    //1���炵
                    --lifeNum;
                    //Init�Ɉڍs
                    mode = GameMode.INIT;
                }
                else
                {
                    //Result�ɑJ��
                    SceneManager.LoadScene("Result");
                }
            }
        }
        else if (mode == GameMode.GAME_SUCCESS)
        {
            //���o���u��
            if (timer <= 3)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                
                //�|���̂ɂ����������Ԃ̑������L�^
                clearTimeTotal += clearTime;
                //�N���A�񐔂��J�E���g
                ++clearCount;
                //Init�Ɉڍs
                mode = GameMode.INIT;
            }
        }

        if (mode == GameMode.INIT)
        {
            //���o���u�����̊ԕK�v
            obj_activeEnemy.SetActive(false);

            //�|���̂ɂ����������Ԃ�������
            clearTime = 0;
            //Generate�Ɉڍs
            mode = GameMode.GENERATE_TASK;
        }

        ////���萶�����
        //if (gameState == GameState.ThemeGenerate)
        //{
        //    //����𐶐�
        //    GenerateTask();
        //    player.SetStateIdol();
        //    //�f���^�^�C�����g�p���锻������O�Ɍv��
        //    timer += Time.deltaTime;
        //    //��莞�Ԃ��o�߂�����
        //    if (timer > showTaskTime)
        //    {
        //        //����̕\��������
        //        taskImg.enabled = false;
        //        //���܂��̑���w����\��
        //        kamaePanel.SetActive(true);
        //        //�g�p�����^�C�}�[�����Z�b�g���Ă���
        //        timer = 0;
        //        //�Q�[�����[�h��ύX
        //        if (spaceKeyState == false)
        //        {                  
        //            gameState = GameState.Play;
        //        }
        //    }
        //}
        ////�Q�[���v���C���
        //else if (gameState == GameState.Play)
        //{
        //    //�X�y�[�X�L�[��������Ă���(���܂����)
        //    if (spaceKeyState == true)
        //    {
        //        //���܂����\������Ă������\���ɂ���
        //        if (kamaePanel.activeSelf == true)
        //        {
        //            player.SetStateKamae();
        //            kamaePanel.SetActive(false);
        //        }

        //        //�A�N�e�B�u��Ԃ̓G�����Ȃ����
        //        if (activeEnemyObj == null)
        //        {
        //            //�ҋ@���Ԃ��쐬
        //            GenerateTimeRangeRandom(activateWaitTimeRange);
        //            //�ҋ@���Ԃ��o�߂�����
        //            if (waitDeltaTime(waitTime))
        //            {
        //                //�����_���ŃA�N�e�B�u������悤��enemyMgr�Ɏw������
        //                activeEnemyObj = enemyMgr.ActivateEnemyRandom();
        //            }
        //        }
        //        else
        //        {
        //            //�A�N�e�B�u��Ԃ̓G������ƈ�v
        //            if (activeEnemyObj == taskObj)
        //            {
        //                //�������ɃN���A�^�C���Ƃ��Ďg�p���鎞�Ԃ̌v���J�n
        //                taskClearTime += Time.deltaTime;
        //                //�ҋ@���Ԃ��쐬
        //                GenerateTimeRangeRandom(deactivateTaskTimeRange);
        //                //�ҋ@���Ԃ��o�߂�����
        //                //-------------------------------------------------------
        //                //���s(�X�y�[�X�L�[�������ꂽ���)
        //                //-------------------------------------------------------
        //                if (waitDeltaTime(waitTime))
        //                {
        //                    //���s����
        //                    FailedTask();
        //                }
        //            }
        //            //-------------------------------------------------------
        //            //���s���(�X�y�[�X�L�[�������ꂽ���)
        //            //-------------------------------------------------------
        //            /*�����Ȃ�*/
        //        }
        //    }
        //    //�X�y�[�X�L�[��������Ă��Ȃ�(�������) && ���܂��w������\��
        //    else if (!kamaePanel.activeSelf)
        //    {
        //        player.SetStateIai();
        //        activeEnemyObj.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        //        //�A�N�e�B�u��Ԃ̓G�����Ȃ����
        //        if (activeEnemyObj == null)
        //        {
        //            //-------------------------------------------------------
        //            //���s(�X�y�[�X�L�[�������ꂽ���)
        //            //-------------------------------------------------------
        //            //���s����
        //            FailedTask();
        //        }
        //        else
        //        {

        //            //�G�̈ʒu���Œ�
        //            /*
        //            * �G�̈ʒu���Œ菈��������
        //            */
        //            //-------------------------------------------------------
        //            //����(�X�y�[�X�L�[�������ꂽ���)
        //            //-------------------------------------------------------

        //            //�A�N�e�B�u��Ԃ̓G������ƈ�v
        //            if (activeEnemyObj == taskObj)
        //            {
        //                //��������
        //                SucceededTask();
        //                //��A�N�e�B�u����
        //                DeactivateEnemy();
        //            }
        //            //-------------------------------------------------------
        //            //���s(�X�y�[�X�L�[�������ꂽ���)
        //            //-------------------------------------------------------
        //            else
        //            {
        //                //��A�N�e�B�u����
        //                DeactivateEnemy();
        //                //���s����
        //                FailedTask();
        //            }
        //        }
        //    }
        //}
        //else if (gameState == GameState.Score)
        //{
        //    //�����p��5�b�ҋ@����悤�ɂ��Ă�
        //    if (waitDeltaTime(5))
        //    {
        //        gameState = GameState.ThemeGenerate;
        //    }
        //}
    }

    //-------------------------------------------------------
    //Private Method
    //-------------------------------------------------------

    /// <summary>
    /// �ݒ肳�ꂽ�͈͂Ń����_���Ɏ��Ԃ��쐬����
    /// </summary>
    /// <param name="_timeRange">�����_���Ȏ��Ԃ͈̔�</param>
    /// <returns>�쐬���ꂽ����</returns>
    private float GenerateTimeRangeRandom(Vector2 _timeRange)
    {
        return Random.Range(_timeRange.x, _timeRange.y);
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
