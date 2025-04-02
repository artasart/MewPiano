using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource pianoSource;  // �ǾƳ� �Ҹ��� ���� ����� �ҽ�
    public List<AudioClip> pianoClips;  // �ǾƳ� ���� ����Ʈ

    private Dictionary<string, AudioClip> pianoSounds = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // ����� Ŭ���� Dictionary�� ���
        foreach (var clip in pianoClips)
        {
            Debug.Log($" ����� ���: {clip.name}");
            pianoSounds[clip.name] = clip; 
        }
    }

    private void Start()
    {
        PlayPianoSound("C4");
        Debug.Log(" �Ҹ� �����");
    }

    public void PlayPianoSound(string keyName)
    {
        if (pianoSource == null)
        {
            Debug.LogError(" pianoSource�� �������� �ʾҽ��ϴ�!");
            return;
        }
        if (pianoSounds.ContainsKey(keyName))
        {
            Debug.Log($" {keyName} �Ҹ� �����");
            pianoSource.PlayOneShot(pianoSounds[keyName]);
        }
        else
        {
            Debug.LogWarning($"�ǾƳ� ���� {keyName}��(��) ã�� �� �����ϴ�.");
        }
    }
}
