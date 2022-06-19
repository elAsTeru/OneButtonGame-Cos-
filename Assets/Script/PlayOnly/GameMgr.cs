using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    [Header("�v���C���[�̏��")]
    [Tooltip("�A�^�b�`�m�F�p(�ǐ�)")][SerializeField] private Player player;
    [Tooltip("true:���܂���� / false:�������(�ǐ�)")][SerializeField] private bool spaceKeyState;
    private bool prevSpaceKeyState; //�O��̃X�y�[�X�L�[�̏��
    [Space(10)]

    [Header("�G�̏��")]
    [Tooltip("�A�^�b�`�m�F�p(�ǐ�)")][SerializeField] private EnemyMgr enemyMgr;
    [Tooltip("����̓G(�ǐ�)")][SerializeField] private GameObject themeObj;
    [Tooltip("���݂̓G(�ǐ�)")][SerializeField] private GameObject activeEnemyObj;
    [Tooltip("�A�N�e�B�u���ҋ@���Ԃ͈̔�(x:�ŏ����� / y:�ő厞��)")][SerializeField] private Vector2 activateTimeRange;
    [Tooltip("�A�N�e�B�u���ҋ@����(�ǐ�)")][SerializeField]private float activateWaitTime; //�͈̓����_���ŃA�N�e�B�u���߂��o�����Ԃ��i�[����p(-1�Őݒ�Ȃ�)
    [Tooltip("��A�N�e�B�u�ɂ��鎞�Ԃ͈̔�(x:�ŏ����� / y:�ő厞��)")][SerializeField] private Vector2 deactivateTimeRange;
    [Tooltip("�f�A�N�e�B�u�ɂ��鎞��(�ǐ�)")][SerializeField] private float deactiveWaitTime; //�͈̓����_���Ŕ�A�N�e�B�u�ɂ��鎞�Ԃ��i�[����p(-1�Őݒ�Ȃ�)
    [Space(10)]

    [Header("UI�̏��")]
    [Tooltip("����\�����u���摜")][SerializeField] private Image themeImg;

    [Header("���L���Ďg�p")]
    [SerializeField] private float timer;

    // Start is called before the first frame update
    void Start()
    {
        ////�N���A�񐔂�0�ɂ���
        //cleartargetNum = 0;

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
        themeImg = GameObject.Find("�W�I�\�����u��").GetComponent<Image>();
        themeImg.enabled = false;           //��\���ɂ��Ă���
    }

    // Update is called once per frame
    void Update()
    {
        //���肪�������
        if (themeObj == null) { Init(); }
        //-------------------------------------------------------
        //�X�y�[�X�L�[��������Ă���(�\���̏��)
        //-------------------------------------------------------
        if (spaceKeyState == true)
        {
            //�X�y�[�X�L�[�̏�Ԃ��؂�ւ�����Ƃ��ɂP�񂾂����s
            if(spaceKeyState != prevSpaceKeyState)
            {
                prevSpaceKeyState = spaceKeyState;
                timer = 0;
            }
            //���肪�\������Ă������\���ɂ���
            if (themeImg.enabled == true)
            {
                themeImg.enabled = false;
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
            //�A�N�e�B�u�������G���ڕW�ƕs��v
            //-------------------------------------------------------
            else if (activeEnemyObj != themeObj)
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
        //-------------------------------------------------------
        //�X�y�[�X�L�[�������ꂽ(�����̏��)
        //-------------------------------------------------------
        else if(themeObj != null)
        {
            //�X�y�[�X�L�[�̏�Ԃ��؂�ւ�����Ƃ��ɂP�񂾂����s
            if (spaceKeyState != prevSpaceKeyState)
            {
                prevSpaceKeyState = spaceKeyState;
                timer = 0;
            }
            timer += Time.deltaTime;

            //�^�[�Q�b�g�B���̉��u��
            if(themeObj == activeEnemyObj)
            {
                activeEnemyObj.SetActive(false);
                activeEnemyObj = null;
                themeObj = null;
            }
        }

        //����Ɣ�r���A�Ⴆ�Έ�莞�ԂŔ�A�N�e�B�u��

        //����Ɣ�r���A���v����Ώ������v���C���[�̃X�y�[�X���������̂�҂�

        //�����ꂽ�玞�Ԍv�����I��/Init�̓��e�����s/�N���A�񐔂��J�E���g




        ////�����W�I�\�����u�����\������Ă�����
        //if (printTargetObj.GetComponent<Image>().enabled)
        //{
        //    //�X�y�[�X�������ꂽ��
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        //�W�I�\�����u�����\����
        //        printTargetObj.GetComponent<Image>().enabled = false;
        //        //�v���C���[�̏������J�n
        //        player.enabled = true;      //����̃R���|�[�l���g��X�N���v�g���L������ς�����@
        //    }
        //}
        //else
        //{
        //    if (spaceKeyState == true)
        //    {
        //        //�G�Ǘ��̏������J�n����
        //        enemyGenMgr.SetActive(false);    //�S�̂̃A�N�e�B�u��ς�����@
        //    }
        //    else
        //    {
        //        //�G�Ǘ��̏������J�n����
        //        enemyGenMgr.SetActive(true);    //�S�̂̃A�N�e�B�u��ς�����@
        //    }
        //}

        ////�N���A�񐔂��ݒ�񐔂ɂȂ����烊�U���g�ɑJ��
        //if(cleartargetNum >= targetNum)
        //{
        //    SceneManager.LoadScene("Result");
        //}
    }

    //-------------------------------------------------------
    //Private Method
    //-------------------------------------------------------
    /// <summary>
    /// #���s���e
    /// -���萶��
    /// -����\��
    /// </summary>
    private void Init()
    {
        //EnemyMgr�ɂ��萶���̖��߂��o��(gameobjct�^��string�^�ō��ꂽ�����Ԃ�����)
        themeObj = enemyMgr.GenerateTheme();
        //�����\������(���u���Ŕ�����ʂ�\�����Ă���)
        themeImg.enabled = true;
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
