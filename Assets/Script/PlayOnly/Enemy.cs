using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// ��ʊO�ɏo�����A�N�e�B�u��
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
