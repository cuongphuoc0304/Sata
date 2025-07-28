using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManage : MonoBehaviour
   
{
    public static GameManage Instance;
    public Canvas mainCanvas;
    public TextMeshProUGUI numitem;
    public TextMeshProUGUI numApple;
    public TextMeshProUGUI numWater;
    public TextMeshProUGUI numEgg;
    public TextMeshProUGUI numBread;
    public TextMeshProUGUI numMilk;
    public TextMeshProUGUI textQuestion;
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textLives;

    public GameObject UIError;
    public GameObject UIThua;

    public Answer answer1;
    public Answer answer2;
    public Answer answer3;
    public Answer selectedAnswer;

    public AudioClip pick;
    public List<GameObject> cart = new List<GameObject>();

    private HashSet<string> validTags = new HashSet<string> { "Apple", "Water", "Egg", "Bread", "Milk" };

    public GameObject Uicart;

    public int maxItemsInCart = 3;
    private int maxMistakesAllowed = 5;
    public int mistakeCount = 0;
    public int score = 0;

    public GameObject UIConfirmPanel;
    public TextMeshProUGUI confirmText;
    public TextMeshProUGUI priceText;
    public GameObject currentSelectedItem;
    

    private void Awake()
    {
        UpdateScoreAndLivesUI();

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObj = hit.collider.gameObject;

                if (validTags.Contains(hitObj.tag))
                {
                    if (!Uicart.activeSelf)
                    {
                        if (cart.Count >= maxItemsInCart)
                        {
                            //mistakeCount++;
                            ShowError();
                            //UpdateScoreAndLivesUI();

                            // if (mistakeCount >= maxMistakesAllowed)
                            // {
                            //     ShowGameOver();
                            // }

                            // return;
                        }

                        // hitObj.SetActive(false);
                        // cart.Add(hitObj);
                        // UpdateCartCountUI();

                        // if (UIConfirmPanel != null)
                        // {
                        //     // Đảm bảo game không bị dừng khi hủy
                        //     currentSelectedItem = hitObj;
                        //     string itemName = GetDisplayName(hitObj.tag);
                        //     int itemPrice = GetItemPrice(hitObj.tag);
                        //     confirmText.text = $"{itemName}";
                        //     priceText.text = $"{itemPrice}";
                        //     UIConfirmPanel.SetActive(true);
                        //     Time.timeScale = 0f; // Dừng game khi hiển thị xác nhận
                        // }

                        // Trong đoạn xác nhận
                        if (UIConfirmPanel != null)
                        {
                            currentSelectedItem = hitObj;
                            string itemName = GetDisplayName(hitObj.tag);
                            int itemPrice = GetItemPrice(hitObj.tag);
                            confirmText.text = $"{itemName}";
                            priceText.text = $"{itemPrice}";
                            UIConfirmPanel.SetActive(true);

                            // Đặt vị trí popup gần đối tượng (Canvas Overlay/Camera)
                            Vector3 screenPos = Camera.main.WorldToScreenPoint(hitObj.transform.position);
                            RectTransform panelRect = UIConfirmPanel.GetComponent<RectTransform>();
                            RectTransform canvasRect = mainCanvas.transform as RectTransform;
                            Vector2 localPoint;
                            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                                canvasRect,
                                screenPos,
                                mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCanvas.worldCamera,
                                out localPoint
                            );
                            panelRect.localPosition = localPoint;
                        }

                        Debug.Log($"Đã thêm vào giỏ hàng: {hitObj.name}");
                        ManageSound.Instance.PlaySFX(pick);
                    }
                }
            }
        }
    }
    public void ConfirmAddToCart()
    {

        if (currentSelectedItem != null)
        {
            currentSelectedItem.SetActive(false);
            cart.Add(currentSelectedItem);
            UpdateCartCountUI();
            ManageSound.Instance.PlaySFX(pick);
            Debug.Log($"Đã thêm vào giỏ hàng: {currentSelectedItem.name}");

            currentSelectedItem = null;
            UIConfirmPanel.SetActive(false);
            Time.timeScale = 1f;
        }

         // Đảm bảo game không bị dừng khi xác nhận
    }

    public void CancelAddToCart()
    {
        
        currentSelectedItem = null;
        UIConfirmPanel.SetActive(false);
        Time.timeScale = 1f; 
        }

    private int GetItemPrice(string tag)
    {
        switch (tag)
        {
            case "Apple": return 3;
            case "Bread": return 3;
            case "Milk": return 5;
            case "Egg": return 7;
            case "Water": return 2;
            default: return 0;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreAndLivesUI();
    }
    public void UpdateScoreAndLivesUI()
    {
        if (textScore != null)
            textScore.text = score.ToString();

        if (textLives != null)
            textLives.text = (maxMistakesAllowed - mistakeCount).ToString();
        if (maxMistakesAllowed - mistakeCount <= 0) ShowGameOver();
    }

    private void ShowError()
    {
        if (UIError != null)
        {
            UIError.SetActive(true);
            Time.timeScale = 0f; // Dừng game
            Invoke(nameof(HideError), 15f); // Tự ẩn sau 2 giây
            Time.timeScale = 1f;
        }
    }

    private void HideError()
    {
        if (UIError != null)
            UIError.SetActive(false);
        Time.timeScale = 1f; // Tiếp tục game
    }

    public void ShowGameOver()
    {
        if (UIThua != null)
        {
            UIThua.SetActive(true);
            Time.timeScale = 0f; // Dừng game
            Debug.Log("Bạn đã thua vì vượt quá số lần giới hạn!");
        }
    }

    private void UpdateCartCountUI()
    {
        if (numitem != null)
            numitem.text = cart.Count.ToString();

        int countApple = 0, countWater = 0, countEgg = 0, countBread = 0, countMilk = 0;

        foreach (GameObject item in cart)
        {
            switch (item.tag)
            {
                case "Apple": countApple++; break;
                case "Water": countWater++; break;
                case "Egg": countEgg++; break;
                case "Bread": countBread++; break;
                case "Milk": countMilk++; break;
            }
        }

        if (numApple != null) numApple.text = countApple.ToString();
        if (numWater != null) numWater.text = countWater.ToString();
        if (numEgg != null) numEgg.text = countEgg.ToString();
        if (numBread != null) numBread.text = countBread.ToString();
        if (numMilk != null) numMilk.text = countMilk.ToString();
    }

    public void RemoveRandomItemByTag(string tag)
    {
        List<GameObject> matchingItems = cart.FindAll(obj => obj != null && obj.tag == tag);

        if (matchingItems.Count == 0)
        {
            Debug.LogWarning($"Không có sản phẩm '{tag}' trong giỏ để xóa.");
            return;
        }

        int randomIndex = Random.Range(0, matchingItems.Count);
        GameObject randomItem = matchingItems[randomIndex];
        randomItem.SetActive(true);
        cart.Remove(randomItem);

        Debug.Log($"Đã xóa ngẫu nhiên một '{tag}': {randomItem.name}");
        UpdateCartCountUI();
    }

    public void GenerateQuestionFromCart()
    {
        Dictionary<string, int> prices = new Dictionary<string, int>()
        {
            { "Apple", 3 },
            { "Bread", 3 },
            { "Milk", 5 },
            { "Egg", 7 },
            { "Water", 2 }
        };

        Dictionary<string, int> itemCounts = new Dictionary<string, int>()
        {
            { "Apple", 0 },
            { "Bread", 0 },
            { "Milk", 0 },
            { "Egg", 0 },
            { "Water", 0 }
        };

        foreach (GameObject item in cart)
        {
            if (item != null && itemCounts.ContainsKey(item.tag))
            {
                itemCounts[item.tag]++;
            }
        }

        List<string> parts = new List<string>();
        int total = 0;

        foreach (var kvp in itemCounts)
        {
            int count = kvp.Value;
            if (count > 0)
            {
                string name = kvp.Key;
                int price = prices[name];
                parts.Add($"{count} {GetDisplayName(name)} ({price * count} Sata)");
                total += count * price;
            }
        }

        if (parts.Count == 0)
        {
            textQuestion.text = "Giỏ hàng đang trống.";
            answer1.gameObject.SetActive(false);
            answer2.gameObject.SetActive(false);
            answer3.gameObject.SetActive(false);
        }
        else
        {
            answer1.gameObject.SetActive(true);
            answer2.gameObject.SetActive(true);
            answer3.gameObject.SetActive(true);

            string itemsText = string.Join(" và ", parts);
            textQuestion.text = $"Bạn mua {itemsText}.";
            //textQuestion.text = $"Bạn mua {itemsText}.Nhấn xác nhận để hoàn tất.";
           

            HashSet<int> usedAnswers = new HashSet<int> { total };
            List<int> allAnswers = new List<int> { total };

            int wrong1 = GenerateUniqueWrongAnswer(total, usedAnswers);
            allAnswers.Add(wrong1);
            usedAnswers.Add(wrong1);

            int wrong2 = GenerateUniqueWrongAnswer(total, usedAnswers);
            allAnswers.Add(wrong2);

            ShuffleList(allAnswers);

            //answer1.TrueAnswer = total;
            answer1.SetText(allAnswers[0].ToString());
            answer1.SetIsTrue(allAnswers[0] == total);

            //answer2.TrueAnswer = total;
            answer2.SetText(allAnswers[1].ToString());
            answer2.SetIsTrue(allAnswers[1] == total);

            //answer3.TrueAnswer = total;
            answer3.SetText(allAnswers[2].ToString());
            answer3.SetIsTrue(allAnswers[2] == total);
        }
    }

    private int GenerateUniqueWrongAnswer(int correctAnswer, HashSet<int> used)
    {
        int attempt = 0;
        while (attempt < 10)
        {
            int offset = Random.Range(-30, 31);
            int candidate = correctAnswer + offset;
            if (candidate > 0 && !used.Contains(candidate))
                return candidate;

            attempt++;
        }

        return correctAnswer + 1;
    }

    private void ShuffleList(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    public void ClearCart()
    {
        foreach (GameObject item in cart)
        {
            if (item != null)
            {
                item.SetActive(true);
            }
        }

        cart.Clear();
        UpdateCartCountUI();
        Debug.Log("Đã xóa toàn bộ giỏ hàng.");
    }

    private string GetDisplayName(string tag)
    {
        switch (tag)
        {
            case "Apple": return "Táo";
            case "Bread": return "Bánh";
            case "Milk": return "Sữa";
            case "Egg": return "Trứng";
            case "Water": return "Nước";
            default: return tag;
        }
    }
}
