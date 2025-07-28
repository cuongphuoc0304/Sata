using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIFPSRaycaster : MonoBehaviour
{
    public Camera fpsCamera; // Gán camera góc nhìn thứ nhất
    public Canvas canvas;    // Canvas đang ở World Space

    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;
    public Button btn;
    public GameObject UiQuestion;
    void Start()
    {
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;
    }

    // void Update()
    // {
    //     if (!UiQuestion.activeSelf) // Chỉ chạy nếu UiQuestion đang bị ẩn
    //     {
    //         // Ray từ tâm màn hình
    //         Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    //         //Vector2 screenCenter = new Vector2(Screen.width , Screen.height );
    //         PointerEventData pointerData = new PointerEventData(eventSystem);
    //         pointerData.position = screenCenter;

    //         List<RaycastResult> results = new List<RaycastResult>();
    //         raycaster.Raycast(pointerData, results);

    //         foreach (RaycastResult result in results)
    //         {
    //             Button hitButton = result.gameObject.GetComponent<Button>();
    //             if (hitButton != null && Input.GetMouseButtonDown(0))
    //             {
    //                 hitButton.onClick.Invoke();
    //                 break;
    //             }
    //         }
    //     }
    // }
    void Update()
{
    if (!UiQuestion.activeSelf)
    {
        // Ray từ vị trí chuột
        Vector2 mousePos = Input.mousePosition;
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = mousePos;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {
            Button hitButton = result.gameObject.GetComponent<Button>();
            if (hitButton != null && Input.GetMouseButtonDown(0))
            {
                hitButton.onClick.Invoke();
                break;
            }
        }
    }
}

}
