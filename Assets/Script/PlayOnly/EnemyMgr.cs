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

    //-------------------------------------------------------
    //Public Method For GameMgr
    //-------------------------------------------------------
    /// <summary>
    /// ���萶���̎w�����󂯐�������
    /// </summary>
    /// <returns>������������(GameObject)</returns>
    public GameObject GenerateTask()
    {
        GameObject themeObj;

        themeObj = poolSpace.transform.GetChild(Random.Range(0, enemyTypeNum)).gameObject;

        return themeObj;
    }

    /// <summary>
    /// �A�N�e�B�u���̎w�����󂯃����_���ŃA�N�e�B�u������
    /// </summary>
    /// <returns>�A�N�e�B�u�������G(GameObject)</returns>
    public GameObject ActivateEnemyRandom()
    {
        GameObject activateEnemyObj;
        //�ǂ̓G���A�N�e�B�u�����邩�H
        activateEnemyObj = poolSpace.transform.GetChild((short)Random.Range(0, enemyTypeNum)).gameObject;
        //�A�N�e�B�u������ꏊ��ݒ�
        activateEnemyObj.transform.position = generatorSpace.transform.GetChild(Random.Range(0, generatorNum)).position;
        //�A�N�e�B�u��
        activateEnemyObj.SetActive(true);
        //�A�N�e�B�u�������G�I�u�W�F�N�g��Ԃ�
        return activateEnemyObj;
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