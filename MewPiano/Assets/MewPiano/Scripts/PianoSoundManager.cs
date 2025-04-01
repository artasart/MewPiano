using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoSoundManager : MonoBehaviour
{
    public AudioSource audioSource;  // ����� �ҽ��� ����
    public AudioClip[] pianoClips;   // �� ���� �ش��ϴ� ����� Ŭ��

    // ���� ����ϴ� �޼���
    public void PlaySound(int noteIndex)
    {
        if (noteIndex < 0 || noteIndex >= pianoClips.Length) return;
        audioSource.clip = pianoClips[noteIndex];
        audioSource.Play();
    }
}
