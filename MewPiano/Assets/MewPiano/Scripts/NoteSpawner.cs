using UnityEngine;
using UnityEngine.UI;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;  // 계이름(PNG) 프리팹
    public Transform pianoTextGroup; // group_Piano_text 패널
    public Sprite[] noteSprites;  // 계이름 PNG 배열

    void Start()
    {
        SpawnNotes();
    }

    void SpawnNotes()
    {
        string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        for (int i = 0; i < noteNames.Length; i++)
        {
            GameObject note = Instantiate(notePrefab, pianoTextGroup);
            note.GetComponent<Image>().sprite = noteSprites[i];
        }
    }
}
