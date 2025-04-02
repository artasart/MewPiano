using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;  // ��Ʈ ������
    public Transform spawnPoint;   // ��Ʈ�� ������ ��ġ
    public float spawnInterval = 1f; // ��Ʈ ���� ���� (��)

    private void Start()
    {
        InvokeRepeating(nameof(SpawnNote), 0f, spawnInterval);
    }

    private void SpawnNote()
    {
        Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);
    }
}
