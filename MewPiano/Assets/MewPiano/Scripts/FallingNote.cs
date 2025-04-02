using UnityEngine;

public class FallingNote : MonoBehaviour
{
    public float fallSpeed = 5f;  // 노트 떨어지는 속도 (조절 가능)

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime; // 아래로 이동

        // 일정 범위 아래로 내려가면 삭제 (Miss 처리 예정)
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }
}
