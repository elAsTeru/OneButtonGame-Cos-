using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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
}
