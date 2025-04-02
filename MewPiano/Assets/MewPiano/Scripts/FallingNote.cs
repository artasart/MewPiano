using UnityEngine;

public class FallingNote : MonoBehaviour
{
    public float fallSpeed = 5f;  // ��Ʈ �������� �ӵ� (���� ����)

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime; // �Ʒ��� �̵�

        // ���� ���� �Ʒ��� �������� ���� (Miss ó�� ����)
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }
}
