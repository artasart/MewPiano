using UnityEngine;

[CreateAssetMenu(fileName = "GameSound", menuName = "Sound/GameSound", order = 1)]
public class GameSound : ScriptableObject
{
    public AudioClip[] bgm;
    public AudioClip[] soundEffects;
}