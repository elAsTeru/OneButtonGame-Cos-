using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMgr : MonoBehaviour
{
    //�G�֘A
    [Header("Enemy")]
    [Tooltip("��������v���n�u���A�^�b�`")][SerializeField] private List<GameObject> enemys;
    [Tooltip("�f�o�b�O�p")][SerializeField] private short enemyTypeNum;
    [Tooltip("�f�o�b�O�p")][SerializeField] private GameObject targetEnemy;     //����ɂȂ��Ă���G������
    private bool targetEnemyIsActive;
    [Tooltip("�f�o�b�O�p")][SerializeField] private float timer;
    [Tooltip("�f�o�b�O�p")][SerializeField] private Transform activateEnemy;      //�A�N�e�B�u������G���i�[����悤

    //�W�F�l���[�^�֘A
    [Header("Generator")]
    [Tooltip("�W�F�l���[�^���܂Ƃ߂����")] private GameObject generatorSpace;
    [Tooltip("�f�o�b�O�p")][SerializeField] private short generatorNum;
    [Tooltip("�f�o�b�O�p")][SerializeField] private bool isGenerate;      //�����t���O

    //�I�u�W�F�N�g�v�[���֘A
    [Header("Object Pool")]
    private GameObject objectPool;      //�I�u�W�F�N�g�v�[�����g�p���邽�߂̃I�u�W�F�N�g���A�^�b�`����
    private GameObject poolSpace;     //�I�u�W�F�N�g�v�[�����i�[������
    [Tooltip("�v�[���̃T�C�Y���w��")][SerializeField] private short poolSize;

    // Start is called before the first frame update
    void Start()
    {
        //�G�֘A
        enemyTypeNum = (short)enemys.Count;                         //�G�̎�ނ��v��

        //�W�F�l���[�^�֘A
        generatorSpace = this.transform.Find("GeneratePoints").gameObject;  //�W�F�l���[�^�̋�Ԃ�����
        generatorNum = (short)generatorSpace.transform.childCount;      //�W�F�l���[�^�����v��

        //�I�u�W�F�N�g�v�[���֘A
        objectPool = GameObject.Find("ObjectPool");         //�I�u�W�F�N�g�v�[����T��
        poolSpace = new GameObject("PoolSpace");            //�v�[���I�u�W�F�N�g���i�[�����Ԃ��쐬
        poolSpace.transform.parent = this.transform;        //�v�[����Ԃ��q�̈ʒu�Ɉړ�
        for (int i = 0; i < enemyTypeNum; ++i)                      //�v�[���I�u�W�F�N�g�g�𐶐�
        {
            objectPool.GetComponent<ObjectPool>().InsPool(enemys[i], poolSize, poolSpace);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�ڕW�쐬
        CreateTarget();

        //�V���ɃA�N�e�B�u���������H
        if(ActivateEnemy())
        {
            //timer�����Z�b�g
            timer = 0;
        }

        //���Ԃ��v��
        timer += Time.deltaTime;

        //�A�N�e�B�u�Ȃ̂��ڕW�ȊO�Ȃ�
        if (!targetEnemyIsActive)
        {
            //2�b��ɔ�A�N�e�B�u�ɂ��āA������~�𕜋A����
            if (timer > 2)
            {
                activateEnemy.gameObject.SetActive(false);
                isGenerate = true;
            }
        }
    }

    /// <summary>
    /// �ڕW���ݒ肳��ĂȂ���΁A�G�Ƃ��ēo�^����Ă��钆���烉���_���ɖڕW��I�ѓo�^����B
    /// </summary>
    private void CreateTarget()
    {
        if (targetEnemy == null)
        {
            targetEnemy = poolSpace.transform.GetChild(Random.Range(0, enemyTypeNum)).gameObject;
            isGenerate = true;
        }
    }

    /// <summary>
    /// �W�F�l���[�^���ғ��Ȃ�G���A�N�e�B�u������
    /// </summary>
    private bool ActivateEnemy()
    {
        if (isGenerate)
        {
            //�ǂ̓G���A�N�e�B�u�ɂ��邩�H
            activateEnemy = poolSpace.transform.GetChild((short)Random.Range(0, enemyTypeNum));
            //�G�̃A�N�e�B�u���ʒu��ݒ�
            activateEnemy.position = generatorSpace.transform.GetChild(Random.Range(0, generatorNum)).position;
            //�G���A�N�e�B�u��ԂɕύX
            activateEnemy.gameObject.SetActive(true);
            //�A�N�e�B�u�ɂȂ����G�͖ڕW�Ȃ�t���O�𗧂Ă�
            if (activateEnemy.gameObject == targetEnemy)
            {
                targetEnemyIsActive = true;
            }
            //�������ꎞ��~����
            isGenerate = false;

            return true;
        }
        return false;
    }

    /// <summary>
    /// �q�ɃA�N�e�B�u�I�u�W�F�N�g�����邩���ׂ�
    /// </summary>
    /// <param name="_parent">�e�̃g�����X�t�H�[��</param>
    /// <returns>true:���� / false:�Ȃ�</returns>
    private bool FindActiveObjctInChild(Transform _parent)
    {
        foreach(Transform t in _parent)
        {
            if(t.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }
}