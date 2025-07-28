// using UnityEngine;

// public class ThirdPersonCamera : MonoBehaviour
// {
//     public Transform player;                // Nhân vật
//     public Transform cameraPivot;           // Điểm xoay camera
//     public float distanceFromPivot = 3f;
//     public float verticalOffset = 1.5f;

//     public float dragSensitivity = 0.2f;    // Nhạy khi kéo chuột
//     public Vector2 verticalClamp = new Vector2(-45f, 60f); // Giới hạn xoay dọc

//     public bool canRotate = true;           // Bật tắt xoay

//     private float yaw;                      // Góc xoay ngang
//     private float pitch;                    // Góc xoay dọc
//     private Vector3 lastMousePosition;      // Lưu vị trí chuột frame trước
//     private bool isDragging = false;        // Trạng thái kéo chuột

//     void LateUpdate()
//     {
//         HandleMouseDrag();

//         // Cập nhật vị trí và xoay cho pivot
//         cameraPivot.position = player.position + Vector3.up * verticalOffset;
//         cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0f);

//         // Tính vị trí camera
//         Vector3 offset = cameraPivot.rotation * new Vector3(0, 0, -distanceFromPivot);
//         Vector3 newPosition = cameraPivot.position + offset;

//         // Cập nhật vị trí camera
//         transform.position = newPosition;

//         // Luôn nhìn vào pivot
//         transform.LookAt(cameraPivot.position);
//     }

//     void HandleMouseDrag()
//     {
//         if (!canRotate) return;

//         // Khi giữ chuột trái
//         if (Input.GetMouseButtonDown(0))
//         {
//             isDragging = true;
//             lastMousePosition = Input.mousePosition;
//         }
//         else if (Input.GetMouseButtonUp(0))
//         {
//             isDragging = false;
//         }

//         if (isDragging)
//         {
//             Vector3 delta = Input.mousePosition - lastMousePosition;
//             yaw += delta.x * dragSensitivity;
//             pitch -= delta.y * dragSensitivity;
//             pitch = Mathf.Clamp(pitch, verticalClamp.x, verticalClamp.y);

//             lastMousePosition = Input.mousePosition;
//         }
//     }

//     /// <summary>
//     /// Bật hoặc tắt xoay camera khi kéo chuột.
//     /// </summary>
//     public void SetCanRotate(bool value)
//     {
//         canRotate = value;
//     }
// }


using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;
    public Transform cameraPivot;

    public float normalDistance = 1f;
    public float normalVerticalOffset = 2.5f;

    public float closeDistance = 0f;
    public float closeVerticalOffset = 0.5f;

    public float dragSensitivity = 0.2f;
    public Vector2 verticalClamp = new Vector2(-45f, 60f);
    public bool canRotate = true;

    private float yaw;
    private float pitch;
    private Vector3 lastMousePosition;
    private bool isDragging = false;

    private float distanceFromPivot;
    private float verticalOffset;
    private bool isCloseView = false;

    void Start()
    {
        // Gán giá trị mặc định ban đầu
        distanceFromPivot = normalDistance;
        verticalOffset = normalVerticalOffset;
    }

    void LateUpdate()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
    return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            isCloseView = !isCloseView;

            if (isCloseView)
            {
                distanceFromPivot = closeDistance;
                verticalOffset = closeVerticalOffset;
            }
            else
            {
                distanceFromPivot = normalDistance;
                verticalOffset = normalVerticalOffset;
            }
        }

        HandleMouseDrag();

        // Cập nhật vị trí và xoay cho pivot
        cameraPivot.position = player.position + Vector3.up * verticalOffset;
        cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Tính vị trí camera
        Vector3 offset = cameraPivot.rotation * new Vector3(0, 0, -distanceFromPivot);
        Vector3 newPosition = cameraPivot.position + offset;

        // Cập nhật vị trí camera
        transform.position = newPosition;

        // Luôn nhìn vào pivot
        transform.LookAt(cameraPivot.position);
    }

    void HandleMouseDrag()
    {
        if (!canRotate) return;

        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            yaw += delta.x * dragSensitivity;
            pitch -= delta.y * dragSensitivity;
            pitch = Mathf.Clamp(pitch, verticalClamp.x, verticalClamp.y);

            lastMousePosition = Input.mousePosition;
        }
    }

    public void SetCanRotate(bool value)
    {
        canRotate = value;
    }
}
