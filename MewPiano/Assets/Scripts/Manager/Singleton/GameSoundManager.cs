using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundManager : SingletonManager<GameSoundManager>
{
	GameSound gameSound;

	public AudioSource bgm;
	public AudioSource soundEffect;

	float pausedTime = 0;
	string currentFile;

	CoroutineHandle handle_bgm;
	CoroutineHandle handle_replay;

	public float bgmVolume = 1f;
	public float sfxVolume = 1f;

	private Dictionary<string, AudioClip> soundEffectsDict;
	private Dictionary<string, AudioClip> bgmDict;

	public override void OnDestroy()
	{
		base.OnDestroy();

		PlayerPrefs.SetFloat(Define.KEY_BGM, bgmVolume);
		PlayerPrefs.SetFloat(Define.KEY_SOUNDEFFECT, sfxVolume);
	}

	private void Awake()
	{
		gameSound = UnityEngine.Resources.Load<GameSound>(Define.PATH_SOUNDS + "GameSound");

		bgm = gameObject.AddComponent<AudioSource>();
		soundEffect = gameObject.AddComponent<AudioSource>();

		soundEffectsDict = new Dictionary<string, AudioClip>();
		bgmDict = new Dictionary<string, AudioClip>();

		// 사전에 미리 로드
		if (gameSound != null)
		{
			foreach (var clip in gameSound.soundEffects)
			{
				soundEffectsDict[clip.name] = clip;
			}
			foreach (var clip in gameSound.bgm)
			{
				bgmDict[clip.name] = clip;
			}
		}

		if (!Convert.ToBoolean(PlayerPrefs.GetInt(Define.KEY_FIRST)))
		{
			bgmVolume = 1f;
			sfxVolume = 1f;

			PlayerPrefs.SetInt(Define.KEY_FIRST, Convert.ToInt32(true));

			PlayerPrefs.SetFloat(Define.KEY_BGM, 1f);
			PlayerPrefs.SetFloat(Define.KEY_SOUNDEFFECT, 1f);
		}

		bgmVolume = PlayerPrefs.GetFloat(Define.KEY_BGM);
		sfxVolume = PlayerPrefs.GetFloat(Define.KEY_SOUNDEFFECT);

		bgm.volume = bgmVolume;
		soundEffect.volume = sfxVolume;
	}

	public void PlaySound(string _filename, float _volume = 1f)
	{
		if (soundEffectsDict.TryGetValue(_filename, out var clip))
		{
			soundEffect.PlayOneShot(clip, _volume * sfxVolume);
		}
		else
		{
			DebugManager.Log("There is no audio clip.");
		}
	}

	public void PlayBGM(string _filename, float _volume = 1f)
	{
		if (bgmDict.TryGetValue(_filename, out var clip))
		{
			if (currentFile != _filename)
			{
				pausedTime = 0;
				bgm.time = 0;
				currentFile = _filename;
			}
			else
			{
				pausedTime = bgm.time;
			}

			bgm.clip = clip;
			bgm.time = pausedTime;
			bgm.loop = true;
			bgm.Play();

			Timing.KillCoroutines(handle_bgm);
			Timing.KillCoroutines(handle_replay);

			handle_bgm = Timing.RunCoroutine(Co_SetVolume(bgm, _volume * bgmVolume), Define.BGM);
		}
		else
		{
			DebugManager.Log("There is no audio clip.");
		}
	}

	public void StopBGM()
	{
		if (bgm.clip == null) return;

		Timing.KillCoroutines(handle_bgm);

		handle_bgm = Timing.RunCoroutine(Co_SetVolume(bgm, 0f, bgm.Stop), Define.BGM);
	}

	public void SwitchBGM(string _filename, float _volume = 1f)
	{
		if (bgmDict.TryGetValue(_filename, out var clip))
		{
			Timing.KillCoroutines(handle_bgm);

			handle_bgm = Timing.RunCoroutine(Co_SetVolume(bgm, 0f, () => PlayBGM(_filename, _volume * bgmVolume)), Define.BGM);
		}
		else
		{
			DebugManager.Log("There is no audio clip.");
		}
	}

	public void CrossDissolveBGM(string _filename, float _volume = 1f)
	{
		if (handle_bgm.IsRunning) return;

		if (bgmDict.TryGetValue(_filename, out var clip))
		{
			AudioSource target = gameObject.AddComponent<AudioSource>();
			target.clip = clip;
			target.volume = 0f;
			target.Play();

			Timing.KillCoroutines(handle_bgm);

			handle_bgm = Timing.RunCoroutine(Co_CrossDissolveBGM(bgm, target, _volume * bgmVolume), Define.BGM);
		}
		else
		{
			DebugManager.Log("There is no audio clip.");
		}
	}

	private IEnumerator<float> Co_CrossDissolveBGM(AudioSource _current, AudioSource _target, float _volume)
	{
		float elapsedTime = 0f;
		float lerpSpeed = .5f;

		float elapsedTime2 = 0f;

		float total = _current.volume * .25f;

		while (Mathf.Abs(_target.volume - _volume) >= 0.001f)
		{
			_current.volume = Mathf.Lerp(_current.volume, 0f, elapsedTime += lerpSpeed * Time.deltaTime);

			yield return Timing.WaitForOneFrame;

			if (_current.volume <= total)
			{
				_target.volume = Mathf.Lerp(_target.volume, _volume, elapsedTime2 += lerpSpeed * Time.deltaTime);
			}
		}
		_target.volume = _volume;

		_current.Stop();

		Destroy(_current);

		bgm = _target;
	}

	public void SetBGMVolume(float _volume)
	{
		Timing.KillCoroutines(handle_bgm);

		handle_bgm = Timing.RunCoroutine(Co_SetVolume(bgm, _volume), Define.BGM);
	}

	public void SetSoundEffectVolume(float _volume)
	{
		Timing.RunCoroutine(Co_SetVolume(soundEffect, _volume), Define.SOUNDEFFECT);
	}

	IEnumerator<float> Co_SetVolume(AudioSource _audioSource, float _volume, Action _action = null)
	{
		float elapsedTime = 0f;
		float lerpSpeed = .1f;

		while (Mathf.Abs(_audioSource.volume - _volume) >= 0.001f)
		{
			_audioSource.volume = Mathf.Lerp(_audioSource.volume, _volume, elapsedTime += lerpSpeed * Time.deltaTime);

			yield return Timing.WaitForOneFrame;
		}

		_audioSource.volume = _volume;

		_action?.Invoke();
	}

	public void MuteBGM(bool isMute)
	{
		bgm.mute = isMute;
	}
}
