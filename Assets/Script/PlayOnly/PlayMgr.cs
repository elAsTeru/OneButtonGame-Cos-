using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayMgr : MonoBehaviour
{
    [Header("�v���C���[�̏��")]
    private Player player;
    [Tooltip("true:���܂���� / false:�������")][SerializeField]private bool spaceKeyState;

    [Header("PlaySetting")]
    private GameObject enemyGenMgr;        //�����Ǘ��p�̃I�u�W�F�N�g������
    [Tooltip("�o����邨��̐�")][SerializeField] private short targetNum;
    [Tooltip("�N���A��������̐�(��������)")][SerializeField] private short cleartargetNum;
    

    //UI�֘A
    [SerializeField]private GameObject printTargetObj;

    // Start is called before the first frame update
    void Start()
    {
        //�N���A�񐔂�0�ɂ���
        cleartargetNum = 0;
        //�G�Ǘ��𖼑O�Ō������A�^�b�`���Ĕ�A�N�e�B�u�ɂ���
        enemyGenMgr = GameObject.Find("EnemyMgr");
        enemyGenMgr.SetActive(false);

        printTargetObj = GameObject.Find("�W�I�\�����u��");
        printTargetObj.GetComponent<Image>().enabled = true;

        //�v���C���[�̃X�N���v�g���m��
        player = GameObject.Find("Player").GetComponent<Player>();
        player.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //�����W�I�\�����u�����\������Ă�����
        if (printTargetObj.GetComponent<Image>().enabled)
        {
            //�X�y�[�X�������ꂽ��
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //�W�I�\�����u�����\����
                printTargetObj.GetComponent<Image>().enabled = false;
                //�v���C���[�̏������J�n
                player.enabled = true;      //����̃R���|�[�l���g��X�N���v�g���L������ς�����@
            }
        }
        else
        {
            if (spaceKeyState == true)
            {
                //�G�Ǘ��̏������J�n����
                enemyGenMgr.SetActive(false);    //�S�̂̃A�N�e�B�u��ς�����@
            }
            else
            {
                //�G�Ǘ��̏������J�n����
                enemyGenMgr.SetActive(true);    //�S�̂̃A�N�e�B�u��ς�����@
            }
        }

        //�N���A�񐔂��ݒ�񐔂ɂȂ����烊�U���g�ɑJ��
        if(cleartargetNum >= targetNum)
        {
            SceneManager.LoadScene("Result");
        }
    }


    //Public Method For Player
    public void Kamae() { spaceKeyState = true; }
    public void Iai() { spaceKeyState = false; }

    /// <summary>
    /// �v���C���[��������N���A�����̂�`���邽�߂Ɏg�p
    /// </summary>
    public void TellTargetClear() { ++cleartargetNum; }
}
