using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameMgr gameMgr;
    [Header("Player")]
    [Tooltip("�����̃I�u�W�F�N�g")] [SerializeField] private GameObject idol;
    [Tooltip("���܂��̃I�u�W�F�N�g")] [SerializeField] private GameObject kamae;
    [Tooltip("�����̃I�u�W�F�N�g")] [SerializeField] private GameObject iai;

    // Start is called before the first frame update
    void Start()
    {
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();
        idol.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //���܂�
        if (Input.GetKeyDown(KeyCode.Space)) { gameMgr.pressSpace(); }
        //����
        if (Input.GetKeyUp(KeyCode.Space)) { gameMgr.ReleaseSpace(); }
    }


    /// <summary>
    /// �Ǘ�����Ăуv���C���[���\����Ԃɂ���
    /// </summary>
    public void SetIdolMode()
    {
        //���݂�����Ԃ���Ȃ����
        if(!idol.activeSelf)
        {
            //�S�Ă̏�Ԃ��A�N�e�B�u�ɂ��āA
            DeactivateAllMode();
            //������Ԃɂ���
            idol.SetActive(true);
        }
    }
    /// <summary>
    /// �Ǘ�����Ăуv���C���[���\����Ԃɂ���
    /// </summary>
    public void SetKamaeMode()
    {
        //����k���܂���Ԃ���Ȃ����
        if (!kamae.activeSelf)
        {
            //�S�Ă̏�Ԃ��A�N�e�B�u�ɂ��āA
            DeactivateAllMode();
            //���܂���Ԃɂ���
            kamae.SetActive(true);
        }
    }
    /// <summary>
    /// �Ǘ�����Ăуv���C���[��������Ԃɂ���
    /// </summary>
    public void SetIaiMode()
    {
        //���݂�����Ԃ���Ȃ����
        if (!iai.activeSelf)
        {
            //�S�Ă̏�Ԃ��A�N�e�B�u�ɂ��āA
            DeactivateAllMode();
            //������Ԃɂ���
            iai.SetActive(true);
        }
    }
    private void DeactivateAllMode()
    {
        if (idol.activeSelf) { idol.SetActive(false); }
        if (kamae.activeSelf) { kamae.SetActive(false); }
        if (iai.activeSelf) { iai.SetActive(false); }
    }
}
