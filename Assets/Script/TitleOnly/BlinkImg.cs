using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

//インスペクターに表示するが書き換え不可にする
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
    [Tooltip("濃さの最大")][SerializeField] private float maxAlpha = 0.95f;
    [Tooltip("濃さの最小")][SerializeField] private float minAlpha = 0.05f;
    [Tooltip("点滅速度")][Range(0.0f, 50.0f)] public float blinkSpeed = 3.0f;
    [Space(10)]
    [Header("Debug")]
    [Tooltip("円運動の確認用に表示")][SerializeField, ReadOnly] private float time;
    [Tooltip("円運動の確認用に表示")][SerializeField, ReadOnly] private float CosVal;
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
            //このスクリプトをアタッチした画像を取得
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
        //最大値と最小値の幅
        float rangeWidth = maxAlpha - minAlpha;
        time += Time.deltaTime * blinkSpeed;
        //0.5f * (Mathf.Cos(time) + 1)　の計算メモ
        /*頭の0.5  ：()内の計算で0~2になるので0~1の範囲に変更している   */
        /*尻の+1   : -1~1の範囲でカーブするのを0~2に変更している        */
        //上の式に上限と下限を設定
        _color.a = CosVal = (0.5f * (Mathf.Cos(time) + 1)) * rangeWidth + minAlpha;
        return _color;
    }
}