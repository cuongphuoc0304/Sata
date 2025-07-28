using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    public GameObject loadingUI;      // UI loading (Panel)
    public Slider loadingSlider;      // Slider hiển thị tiến trình

    /// <summary>
    /// Gọi hàm này để hiện UI loading và chuyển scene
    /// </summary>
    public void LoadSceneWithLoading(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingUI.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;

            if (operation.progress >= 0.9f)
            {
                loadingSlider.value = 1f;
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}