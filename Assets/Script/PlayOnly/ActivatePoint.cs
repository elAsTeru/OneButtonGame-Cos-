using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivatePoint : MonoBehaviour
{
    [Tooltip("アクティブ化時に付与する力")][SerializeField]private Vector2 force;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// エネミーをアクティブ化し飛び出す力を加える
    /// </summary>
    /// <param name="_activateObj"></param>
    public void Activate(GameObject _activateObj)
    {
        _activateObj.transform.position = this.transform.position;
        
        _activateObj.SetActive(true);

        _activateObj.GetComponent<SpriteRenderer>().color = InitAlpha(_activateObj.GetComponent<SpriteRenderer>().color);

        rb = _activateObj.GetComponent<Rigidbody2D>();
        
        rb.AddForce(force);
    }

    private Color InitAlpha(Color _color)
    {
        _color.a = 1.0f;
        return _color;
    }
}
