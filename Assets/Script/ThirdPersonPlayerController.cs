using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonPlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    [Header("Camera Reference")]
    public Transform cameraPivot; // Pivot của camera để lấy hướng di chuyển

    public Animator ani;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Lấy input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Hướng theo camera
        Vector3 camForward = cameraPivot.forward;
        Vector3 camRight = cameraPivot.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // Tính hướng di chuyển
        Vector3 moveDir = (camForward * v + camRight * h).normalized;

        // Animation
        if (moveDir.magnitude >= 0.1f)
        {
            // if (!ani.GetCurrentAnimatorStateInfo(0).IsName("walk"))
            // {
            //     ani.Play("walk");
            // }
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("run"))
            {
                ani.Play("run");
            }
        }
        else
        {
            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("idle"))
            {
                ani.Play("idle");
            }
        }

        // Di chuyển + xoay hướng
        if (moveDir.magnitude >= 0.1f)
        {
            controller.Move(moveDir * moveSpeed * Time.deltaTime);

            // Xoay hướng di chuyển
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        }

        // Nhảy
        // if (Input.GetButtonDown("Jump") && isGrounded)
        // {
        //     velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        // }

        // Trọng lực
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
