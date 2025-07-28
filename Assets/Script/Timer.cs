using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTime;
    [SerializeField] private Image iconClock;
    void Start()
    {
        
    }


    void Update()
    {
        if (remainingTime <= 10f)
        {
            
            timerText.color = Color.red;
            iconClock.color = Color.red;
            if (remainingTime <= 0f)
            {
                timerText.text = "00:00";
                GameManage.Instance.ShowGameOver();
                return;
            }
            
        }
        remainingTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
