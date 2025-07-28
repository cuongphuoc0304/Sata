
using UnityEngine;
using TMPro;

public class Answer : MonoBehaviour
{
    [Header("UI hiển thị đáp án")]
    public TextMeshProUGUI textAnswer;

    [Header("Thông tin logic")]
    public bool isTrue;
    public int TrueAnswer;

    [Header("Yêu cầu để qua màn")]
    public int requiredSata = 10;        
    public int requiredProducts = 3;     

    [Header("UI Elements")]
    public GameObject UiRight;
    public GameObject UiWrong;
    public GameObject UiQuestion;
    public TextMeshProUGUI wrongMessageText;

    [Header("Các thành phần quản lý")]
    public FirstPersonController firstPersonController;
    public GameObject payment;
    public TextMeshProUGUI coin;

    [Header("Text hiển thị trong UI Right")]
    public TextMeshProUGUI textRightScore;
    public TextMeshProUGUI textRightLives;

    GameManage gameManage;
    ThirdPersonCamera thirdPersonCamera;
    bool isChecked = false;
    private void Start()
    {
        thirdPersonCamera = FindAnyObjectByType<ThirdPersonCamera>();
        gameManage = FindAnyObjectByType<GameManage>();
    }

    public void SetText(string newText)
    {
        if (textAnswer != null)
        {
            textAnswer.text = newText;
        }
    }

    public void SetIsTrue(bool value)
    {
        isTrue = value;
    }

    public void CheckAnswer()
    {
        
        int totalSata = int.Parse(textAnswer.text);
        int totalProduct = GameManage.Instance.cart.Count;

        if (isTrue)
        {
            if (totalSata <= requiredSata && totalProduct == requiredProducts)
            {

                UiRight.SetActive(true);
                UiWrong.SetActive(false);
                gameManage.ClearCart();

                coin.text = requiredSata.ToString(); // Có thể thay đổi logic tính điểm nếu cần
                //GameManage.Instance.AddScore(requiredSata);


            }
            else
            {

                GameManage.Instance.mistakeCount++;
                GameManage.Instance.UpdateScoreAndLivesUI();

                string errorMsg = "";

                if (totalSata == requiredSata && totalProduct != requiredProducts)
                    errorMsg = $"Bạn đã chọn đúng số tiền {totalSata} Sata, nhưng số lượng sản phẩm chưa đúng ({totalProduct}/{requiredProducts}). Hãy thử lại nhé!";
                else if (totalSata > requiredSata)
                    errorMsg = $"Tổng số tiền bạn chọn là {totalSata} Sata, đã vượt quá {requiredSata} Sata cho phép. Cùng kiểm tra lại nào!";
                else if (totalProduct < requiredProducts)
                    errorMsg = $"Bạn mới chọn {totalProduct} sản phẩm, cần đủ {requiredProducts} sản phẩm. Hãy chọn thêm cho đủ nhé!";
                else
                    errorMsg = $"Số tiền {totalSata} Sata và {totalProduct} sản phẩm bạn chọn chưa đúng với yêu cầu đề bài. Cùng thử lại lần nữa nào!";
                gameManage.ClearCart();
                ShowWrong(errorMsg);
            }
        }
        else
        {

            GameManage.Instance.mistakeCount++;
            GameManage.Instance.UpdateScoreAndLivesUI();

            string errorMsg = $"Tổng số tiền không đúng. Hãy tính lại nhé";
            gameManage.ClearCart();
            ShowWrong(errorMsg);
        }

        UiQuestion.SetActive(false);
        thirdPersonCamera.SetCanRotate(true);
        
    }

    public void SelectThisAnswer()
    {
        // Giao diện khi chọn một đáp án
        GameManage.Instance.selectedAnswer = this;  // lưu lại đáp án đã chọn
        Debug.Log($"Đã chọn đáp án: {textAnswer.text}");
    }

    public void ConfirmSelectedAnswer()
    {
        if (GameManage.Instance.selectedAnswer != null)
        {
            GameManage.Instance.selectedAnswer.CheckAnswer();
            GameManage.Instance.selectedAnswer = null;
        }
    }

    private void ShowWrong(string message)
    {
        if (wrongMessageText != null)
            wrongMessageText.text = message;

        UiWrong.SetActive(true);
        UiRight.SetActive(false);
    }
}
