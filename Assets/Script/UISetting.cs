using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : MonoBehaviour
{
    public GameObject UiSetting;
    void Start()
    {
    }


    void Update()
    {

    }
    public void ShowUiSetting()
    {
        UiSetting.SetActive(true);
        Time.timeScale = 0f; // Dừng thời gian khi hiển thị UI
    }
    public void HideUiSetting()
    {
        UiSetting.SetActive(false);
        Time.timeScale = 1f; // Tiếp tục thời gian khi ẩn UI
    }
}
