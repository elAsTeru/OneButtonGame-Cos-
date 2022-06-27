using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubResult : MonoBehaviour
{
    [SerializeField]private TMPro.TMP_Text num;
    [SerializeField]private TMPro.TMP_Text time;

    void Start()
    {
        //num = transform.Find("ClearNum").GetComponent<TMPro.TMP_Text>();
        //time = transform.Find("Time").GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrintSubScore(int _clearNum, float _clearTime)
    {
        num.SetText(_clearNum.ToString("D3") + "‘Ì–Ú");
        time.SetText(_clearTime.ToString("N3") + "•b");
    }
}
