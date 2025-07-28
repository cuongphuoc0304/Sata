using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIRequest : MonoBehaviour
{

    //public SceneManger sceneManager;
    public Canvas UiRequest;
    void Start()
    {
        ShowUIRequest();
    }


    void Update()
    {

    }
    public void ShowUIRequest()
    {
        UiRequest.enabled = true;
        Time.timeScale = 0f;
    }

    public void HideUIRequest()
    {
        UiRequest.enabled = false;
        Time.timeScale = 1f;
    }
    public void LoadScene(string sceneName)
    {
        // GameManage.Instance.AddScore(10);
        // GameManage.Instance.UpdateScoreAndLivesUI();
        SceneManager.LoadScene(sceneName);
        HideUIRequest();
    }
}
