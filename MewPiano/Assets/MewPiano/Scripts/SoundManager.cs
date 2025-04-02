using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource pianoSource;  // 피아노 소리를 위한 오디오 소스
    public List<AudioClip> pianoClips;  // 피아노 음원 리스트

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

        // 오디오 클립을 Dictionary에 등록
        foreach (var clip in pianoClips)
        {
            Debug.Log($" 오디오 등록: {clip.name}");
            pianoSounds[clip.name] = clip; 
        }
    }

    private void Start()
    {
        PlayPianoSound("C4");
        Debug.Log(" 소리 재생됨");
    }

    public void PlayPianoSound(string keyName)
    {
        if (pianoSource == null)
        {
            Debug.LogError(" pianoSource가 설정되지 않았습니다!");
            return;
        }
        if (pianoSounds.ContainsKey(keyName))
        {
            Debug.Log($" {keyName} 소리 재생됨");
            pianoSource.PlayOneShot(pianoSounds[keyName]);
        }
        else
        {
            Debug.LogWarning($"피아노 음원 {keyName}을(를) 찾을 수 없습니다.");
        }
    }
}
