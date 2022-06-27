using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

//�C���X�y�N�^�[�ɕ\�����邪���������s�ɂ���
//https://kazupon.org/unity-no-edit-param-view-inspector/
public class ReadOnlyAttribute : PropertyAttribute
{
}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.PropertyField(_position, _property, _label);
        EditorGUI.EndDisabledGroup();
    }
}
#endif

public class BlinkImg : MonoBehaviour
{
    [Header("Blink Setting")]
    [Tooltip("�Z���̍ő�")][SerializeField] private float maxAlpha = 0.95f;
    [Tooltip("�Z���̍ŏ�")][SerializeField] private float minAlpha = 0.05f;
    [Tooltip("�_�ő��x")][Range(0.0f, 50.0f)] public float blinkSpeed = 3.0f;
    [Space(10)]
    [Header("Debug")]
    [Tooltip("�~�^���̊m�F�p�ɕ\��")][SerializeField, ReadOnly] private float time;
    [Tooltip("�~�^���̊m�F�p�ɕ\��")][SerializeField, ReadOnly] private float CosVal;
    enum Type
    {
        IMAGE,
        TMP
    }
    [SerializeField]Type type;
    
    private Image img;
    private TMPro.TextMeshProUGUI tmp;
    
    // Start is called before the first frame update
    void Start()
    {
        if (type == Type.IMAGE)
        {
            //���̃X�N���v�g���A�^�b�`�����摜���擾
            img = this.GetComponent<Image>();
        }
        else if(type == Type.TMP)
        {
            tmp = this.GetComponent<TMPro.TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (type == Type.IMAGE)
        {
            img.color = GetAlphaColor(img.color);
        }
        else if (type == Type.TMP)
        {
            tmp.color = GetAlphaColor(tmp.color);
        }
    }

    Color GetAlphaColor(Color _color)
    {
        //�ő�l�ƍŏ��l�̕�
        float rangeWidth = maxAlpha - minAlpha;
        time += Time.deltaTime * blinkSpeed;
        //0.5f * (Mathf.Cos(time) + 1)�@�̌v�Z����
        /*����0.5  �F()���̌v�Z��0~2�ɂȂ�̂�0~1�͈̔͂ɕύX���Ă���   */
        /*�K��+1   : -1~1�͈̔͂ŃJ�[�u����̂�0~2�ɕύX���Ă���        */
        //��̎��ɏ���Ɖ�����ݒ�
        _color.a = CosVal = (0.5f * (Mathf.Cos(time) + 1)) * rangeWidth + minAlpha;
        return _color;
    }
}