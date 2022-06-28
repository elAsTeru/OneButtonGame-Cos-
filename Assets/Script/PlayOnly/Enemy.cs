using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("画像")]
    [Tooltip("切れたとき①")] [SerializeField] private GameObject obj_split1;
    [Tooltip("切れたとき②")] [SerializeField] private GameObject obj_split2;
    [Tooltip("切れた画像があるパス")] [SerializeField] private string path1;
    [Tooltip("切れた画像があるパス")] [SerializeField] private string path2;
    //[Tooltip("切れたときの背景")] [SerializeField] private GameObject obj_back;
    private GameObject spriteSpace;

    private Rigidbody2D rb_split1;
    private Rigidbody2D rb_split2;

    private void Awake()
    {
        spriteSpace = GameObject.Find("SpliteSpace");

        path1 = "Assets/Prefab/" + this.GetComponent<SpriteRenderer>().sprite.name + "split1.prefab";
        obj_split1 = AssetDatabase.LoadAssetAtPath<GameObject>(path1);
        obj_split1 = Instantiate(obj_split1);
        obj_split1.transform.parent = spriteSpace.transform;
        obj_split1.SetActive(false);
        rb_split1 = obj_split1.GetComponent<Rigidbody2D>();

        path2 = "Assets/Prefab/" + this.GetComponent<SpriteRenderer>().sprite.name + "split2.prefab";
        obj_split2 = AssetDatabase.LoadAssetAtPath<GameObject>(path2);
        obj_split2 = Instantiate(obj_split2);
        obj_split2.transform.parent = spriteSpace.transform;
        obj_split2.SetActive(false);
        rb_split2 = obj_split2.GetComponent<Rigidbody2D>();

    }
    private void Start()
    {
        //切られたときに飛ぶオブジェクトを取得

        //切れたときに飛ぶオブジェクトから物理を取得
    }

    /// <summary>
    /// 画面外に出たら非アクティブ化
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag=="Map")
        {
            this.gameObject.SetActive(false);
        }
    }

    public void Split()
    {
        //力の量を作成
        Vector2 vec2 = new Vector2(-600, 600);
        //生成する位置を設定
        obj_split1.transform.position = this.transform.position;
        obj_split2.transform.position = this.transform.position;
        //アクティブに
        obj_split1.SetActive(true);
        obj_split2.SetActive(true);
        //力をリセットしてから
        rb_split1.velocity = Vector2.zero;
        rb_split2.velocity = Vector2.zero;
        //自分を非アクティブにする
        this.gameObject.SetActive(false);
        //力を加える
        rb_split1.AddForce(vec2);
        rb_split2.AddForce(-vec2);
    }
    public void Failed()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = HalfAlpha(this.gameObject.GetComponent<SpriteRenderer>().color);
    }

    private Color HalfAlpha(Color _color)
    {
        _color.a = _color.a / 2;
        return _color;
    }
}
