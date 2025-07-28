// using UnityEngine;

// public class MouseCursorManager : MonoBehaviour
// {
//     // Trạng thái hiện tại của con trỏ
//     private bool isCursorVisible = false;

//     void Start()
//     {
//         // Khởi đầu ẩn con trỏ
//         SetCursorState(false);
//     }

//     void Update()
//     {
//         // Nhấn Esc để toggle trạng thái con trỏ
//         if (Input.GetKeyDown(KeyCode.Escape))
//         {
//             isCursorVisible = !isCursorVisible;
//             SetCursorState(isCursorVisible);
//         }
//     }

//     /// <summary>
//     /// Bật hoặc tắt con trỏ chuột.
//     /// </summary>
//     /// <param name="isVisible">True: hiện con trỏ, False: ẩn con trỏ</param>
//     public void SetCursorState(bool isVisible)
//     {
//         Cursor.visible = isVisible;
//         Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
//     }
// }
