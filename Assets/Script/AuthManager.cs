using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class AuthManager : MonoBehaviour
{
    [Header("Register")]
    public TMP_InputField fullNameInput;
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;
    public TMP_Dropdown languageDropdown;
    public TMP_Dropdown countryDropdown;
    public TMP_Dropdown cityDropdown;
    public TMP_Dropdown dayDropdown;
    public TMP_Dropdown monthDropdown;
    public TMP_Dropdown yearDropdown;
    public TMP_Text registerMessageText;
    public GameObject UI_loginPanel1;
    [Header("Login")]
    public TMP_InputField loginUsernameInput;
    public TMP_InputField loginPasswordInput;
    public TMP_Text loginMessageText;

    [Header("Server")]
    public string registerURL = "http://localhost:3000/api/users/register";
    public string loginURL = "http://localhost:3000/api/users/login";

    [Header("UI Panels")]
    public GameObject UI_loginPanel;
    public GameObject UI_SelectGameMode;

    [Header("UI loading")]
    public GameObject loadingUI;      // UI loading (Panel)
    public Slider loadingSlider;      // Slider hiển thị tiến trình
    public TextMeshProUGUI loadingText;

    private string currentUsername;
    void Start()
    {
        PopulateDropdowns();
        UI_SelectGameMode.SetActive(false);
    }

    void PopulateDropdowns()
    {
        // Populate Language
        languageDropdown.AddOptions(new List<string> { "Tiếng Việt", "English","Tiếng Việt", "English","Tiếng Việt", "English" });

        // Populate Country
        countryDropdown.AddOptions(new List<string> { "Vietnam", "USA", "Japan", "Korea" });

        // Populate City (simplified, normally you'd change based on country)
        cityDropdown.AddOptions(new List<string> { "Đà Nẵng", "Hà Nội", "Hồ Chí Minh", "New York", "Tokyo", "Seoul" });

        // Day (1–31)
        dayDropdown.ClearOptions();
        List<string> days = new List<string>();
        for (int i = 1; i <= 31; i++) days.Add(i.ToString());
        dayDropdown.AddOptions(days);

        // Month (1–12)
        monthDropdown.ClearOptions();
        List<string> months = new List<string>();
        for (int i = 1; i <= 12; i++) months.Add(i.ToString());
        monthDropdown.AddOptions(months);

        // Year (1950–2020)
        yearDropdown.ClearOptions();
        List<string> years = new List<string>();
        for (int i = 1950; i <= 2020; i++) years.Add(i.ToString());
        yearDropdown.AddOptions(years);
    }

    public void OnRegisterClicked()
    {
        string fullname = fullNameInput.text;
        string username = usernameInput.text;
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;

        if (password != confirmPassword)
        {
            registerMessageText.text = "Mật khẩu không khớp.";
            return;
        }

        string language = languageDropdown.options[languageDropdown.value].text;
        string country = countryDropdown.options[countryDropdown.value].text;
        string city = cityDropdown.options[cityDropdown.value].text;

        string birthdate = $"{yearDropdown.options[yearDropdown.value].text}-{monthDropdown.options[monthDropdown.value].text.PadLeft(2, '0')}-{dayDropdown.options[dayDropdown.value].text.PadLeft(2, '0')}";

        //StartCoroutine(RegisterRequest(fullname, username, password, language, country, city, birthdate));
        StartCoroutine(RegisterRequest(
            fullname,
            username,
            password,
            confirmPassword,  // bạn cũng cần gửi confirmPassword
            language,
            country,
            city,
            birthdate
        ));

    }

    IEnumerator RegisterRequest(
    string fullname,
    string username,
    string password,
    string confirmPassword,
    string language,
    string country,
    string city,
    string birthdate
    )
    {
        WWWForm form = new WWWForm();
        form.AddField("full_name", fullname);
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("confirmPassword", confirmPassword); // <- QUAN TRỌNG
        form.AddField("language", language);
        form.AddField("country", country);
        form.AddField("city", city);
        form.AddField("birthdate", birthdate);

        using (UnityWebRequest www = UnityWebRequest.Post(registerURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                registerMessageText.text = "Đăng ký thành công!";
                Debug.Log("Register success: " + www.downloadHandler.text);
                
            }
            else
            {
                string error = www.downloadHandler.text;

                if (error.Contains("đã tồn tại"))
                    registerMessageText.text = "Tên người dùng đã tồn tại.";
                else if (error.Contains("Mật khẩu xác nhận không khớp"))
                    registerMessageText.text = "Mật khẩu xác nhận không khớp.";
                else if (error.Contains("Vui lòng điền đầy đủ thông tin"))
                    registerMessageText.text = "Vui lòng điền đầy đủ thông tin.";
                else
                    registerMessageText.text = "Đăng ký thất bại.";
                
                Debug.Log("Register error: " + error);
            }
        }
    }


    public void OnLoginClicked()
    {
        string username = loginUsernameInput.text;
        string password = loginPasswordInput.text;
        StartCoroutine(LoginRequest(username, password));
    }

    // IEnumerator LoginRequest(string username, string password)
    // {
    //     WWWForm form = new WWWForm();
    //     form.AddField("username", username);
    //     form.AddField("password", password);

    //     using (UnityWebRequest www = UnityWebRequest.Post(loginURL, form))
    //     {
    //         yield return www.SendWebRequest();

    //         if (www.result == UnityWebRequest.Result.Success){
    //             loginMessageText.text = "Đăng nhập thành công!";
    //             Debug.Log("Login success: " + www.downloadHandler.text);
    //             currentUsername = loginUsernameInput.text;
    //             UI_SelectGameMode.SetActive(true);
    //             UI_loginPanel.SetActive(false);

    //         }
    //         else
    //         {
    //             loginMessageText.text = " Thông tin đăng nhập không chính xác.";
    //             Debug.Log("Login error: " + www.downloadHandler.text);

    //             // Load UI based on language
    //             string userLang = languageDropdown.options[languageDropdown.value].text;
    //             ApplyLanguage(userLang);
    //         }
    //     }
    // }

    IEnumerator LoginRequest(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(loginURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success){
                loginMessageText.text = "Đăng nhập thành công!";
                Debug.Log("Login success: " + www.downloadHandler.text);
                currentUsername = loginUsernameInput.text;
                UI_SelectGameMode.SetActive(true);
                UI_loginPanel.SetActive(false);

                // Lưu token vào PlayerPrefs
                var tokenObj = JsonUtility.FromJson<TokenResponse>(www.downloadHandler.text);
                PlayerPrefs.SetString("user_token", tokenObj.token);
                PlayerPrefs.Save();
                Debug.Log("Token hiện tại " + tokenObj.token);

            }
            else
            {
                loginMessageText.text = " Thông tin đăng nhập không chính xác.";
                Debug.Log("Login error: " + www.downloadHandler.text);

                // Load UI based on language
                string userLang = languageDropdown.options[languageDropdown.value].text;
                ApplyLanguage(userLang);
            }
        }
    }

    
    public void OnPlayMode1ButtonClicked()
    {
        StartCoroutine(GetUserDataAndLoadScene(currentUsername));
    }


//     IEnumerator GetUserDataAndLoadScene(string username)
// {
//     string url = "http://localhost:3000/api/users/getUser?username=" + UnityWebRequest.EscapeURL(username);

//     UnityWebRequest request = UnityWebRequest.Get(url);
//     yield return request.SendWebRequest();

//     if (request.result != UnityWebRequest.Result.Success)
//     {
//         Debug.LogError("Lỗi lấy dữ liệu user: " + request.error);
//         yield break;
//     }

//     Debug.Log("Dữ liệu nhận về: " + request.downloadHandler.text);

//     UserData user = JsonUtility.FromJson<UserData>(request.downloadHandler.text);

//     DateTime birthDate = DateTime.Parse(user.birthdate);
//     int age = DateTime.Now.Year - birthDate.Year;
//     if (DateTime.Now < birthDate.AddYears(age)) age--;

//     Debug.Log("Tuổi người chơi: " + age);

//     if (age == 5 )
//     {
//         StartCoroutine(ShowMessageAndLoad("CHÀO MỪNG ĐẾN VỚI THỬ THÁCH CỦA HỌC SINH 5 TUỔI", "map1"));
//     }
//     else if (age == 6 )
//     {
//         StartCoroutine(ShowMessageAndLoad("CHÀO MỪNG ĐẾN VỚI THỬ THÁCH CỦA HỌC SINH 6 TUỔI", "map2"));
//     }
//     else
//     {
//         StartCoroutine(ShowMessageAndLoad("CHÀO MỪNG ĐẾN VỚI THỬ THÁCH CỦA HỌC SINH 5 TUỔI", "map1"));
//     }
// }


IEnumerator GetUserDataAndLoadScene(string username)
{
    string url = "http://localhost:3000/api/users/getUser?username=" + UnityWebRequest.EscapeURL(username);

    UnityWebRequest request = UnityWebRequest.Get(url);

    // 🔐 Lấy token từ PlayerPrefs (nếu bạn đã lưu sau khi login)
    string token = PlayerPrefs.GetString("user_token", "");
    if (!string.IsNullOrEmpty(token))
    {
        request.SetRequestHeader("Authorization", "Bearer " + token);
    }

    yield return request.SendWebRequest();

    if (request.result != UnityWebRequest.Result.Success)
    {
        Debug.LogError("Lỗi lấy dữ liệu user: " + request.error);
        yield break;
    }

    Debug.Log("Dữ liệu nhận về: " + request.downloadHandler.text);

    UserData user = JsonUtility.FromJson<UserData>(request.downloadHandler.text);

    DateTime birthDate = DateTime.Parse(user.birthdate);
    int age = DateTime.Now.Year - birthDate.Year;
    if (DateTime.Now < birthDate.AddYears(age)) age--;

    Debug.Log("Tuổi người chơi: " + age);

    if (age == 5)
    {
        StartCoroutine(ShowMessageAndLoad("CHÀO MỪNG ĐẾN VỚI THỬ THÁCH CỦA HỌC SINH 5 TUỔI", "map1"));
    }
    else if (age == 6)
    {
        StartCoroutine(ShowMessageAndLoad("CHÀO MỪNG ĐẾN VỚI THỬ THÁCH CỦA HỌC SINH 6 TUỔI", "map2"));
    }
    else
    {
        StartCoroutine(ShowMessageAndLoad("CHÀO MỪNG ĐẾN VỚI THỬ THÁCH CỦA HỌC SINH 5 TUỔI", "map1"));
    }
}

private IEnumerator ShowMessageAndLoad(string message, string sceneName)
{
    loadingText.text = message;
    yield return new WaitForSeconds(1f); 
    LoadSceneWithLoading(sceneName);
}

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
    [Serializable]
    public class UsernameWrapper
    {
        public string username;
    }

    [Serializable]
    public class TokenResponse
    {
        public string token;
    }

    void ApplyLanguage(string lang)
    {
        // Load UI text here using lang code: "vi", "en", "ko", "jp"
        // E.g., set titleText.text = TranslationManager.Get("login", lang);
    }
}
