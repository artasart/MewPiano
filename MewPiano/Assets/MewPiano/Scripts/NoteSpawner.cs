using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;  // 노트 프리팹
    public Transform spawnPoint;   // 노트가 생성될 위치
    public float spawnInterval = 1f; // 노트 생성 간격 (초)

    private void Start()
    {
        InvokeRepeating(nameof(SpawnNote), 0f, spawnInterval);
    }

    private void SpawnNote()
    {
        Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);
    }
}
