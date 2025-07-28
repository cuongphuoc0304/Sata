using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    public Canvas UiGameOver;
    void Start()
    {
        ShowUIGameOver();
    }


    void Update()
    {

    }
    public void ShowUIGameOver()
    {
        UiGameOver.enabled = true;
        Time.timeScale = 0f;
    }

    public void HideUIGameOver()
    {
        UiGameOver.enabled = false;
        Time.timeScale = 1f;
    }
}
