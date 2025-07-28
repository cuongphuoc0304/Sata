using System.Collections;
using UnityEngine;

public class NFCController : MonoBehaviour
{
    Animator anim;
    public Transform[] points;
    public float moveSpeed = 1.5f;
    public float waitTimeAtTarget = 2f;

    private int currentIndex = 0;
    private Vector3 targetPosition;

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(MoveToPointsInOrder());
    }

    IEnumerator MoveToPointsInOrder()
    {
        while (true)
        {
            // Chọn điểm tiếp theo theo thứ tự
            targetPosition = points[currentIndex].position;
            currentIndex = (currentIndex + 1) % points.Length; // Lặp lại khi hết mảng

            anim.Play("Run");
            while (Vector3.Distance(transform.position, targetPosition) > 1f)
            {
                Vector3 direction = (targetPosition - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;

                // Quay mặt về hướng target
                Vector3 lookDirection = targetPosition - transform.position;
                lookDirection.y = 0;
                if (lookDirection != Vector3.zero)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), 5f * Time.deltaTime);

                yield return null;
            }

            anim.Play("Idle");
            yield return new WaitForSeconds(waitTimeAtTarget);
        }
    }
}