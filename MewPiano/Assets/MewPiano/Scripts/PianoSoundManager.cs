using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoSoundManager : MonoBehaviour
{
    public AudioSource audioSource;  // 오디오 소스를 연결
    public AudioClip[] pianoClips;   // 각 음에 해당하는 오디오 클립

    // 음을 재생하는 메서드
    public void PlaySound(int noteIndex)
    {
        if (noteIndex < 0 || noteIndex >= pianoClips.Length) return;
        audioSource.clip = pianoClips[noteIndex];
        audioSource.Play();
    }
}
