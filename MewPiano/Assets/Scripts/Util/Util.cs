using DG.Tweening;
using MEC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static EasingFunction;
using static Enums;

public static class Util
{
	public static void RunCoroutine(IEnumerator<float> _coroutine, string _tag = null, CoroutineTag _layer = CoroutineTag.None)
	{
		Timing.KillCoroutines(_tag);

		Timing.RunCoroutine(_coroutine, (int)_layer, _tag);
	}

	public static void KillCoroutine(string _tag = null)
	{
		Timing.KillCoroutines(_tag);
	}

	public static T ConvertStringToEnum<T>(string value) where T : System.Enum
	{
		return (T)System.Enum.Parse(typeof(T), value);
	}

	public static TEnum ConvertIntToEnum<TEnum>(int value)
	{
		if (System.Enum.IsDefined(typeof(TEnum), value))
		{
			return (TEnum)System.Enum.ToObject(typeof(TEnum), value);
		}

		else
		{
			Debug.LogWarning($"Enum value {value} is not defined in {typeof(TEnum)}.");
			return default(TEnum);
		}
	}

	public static int GetEnumLength<T>()
	{
		return System.Enum.GetNames(typeof(T)).Length;
	}

	public static DateTime StringToDateTime(string dateTimeString)
	{
		DateTime dateTime = DateTime.Parse(dateTimeString, null, System.Globalization.DateTimeStyles.RoundtripKind);
		return dateTime;
	}

	public static TMP_Text FindTMPText(GameObject gameObject, string name, string message = null)
	{
		TMP_Text element = gameObject.transform.Search(name).GetComponent<TMP_Text>();

		if (!string.IsNullOrEmpty(message))
		{
			element.text = message;
		}

		return element;
	}

	public static Button FindButton(GameObject gameObject, string name, Action action = null, bool useSound = true, bool useAnimation = false)
	{
		Button element = gameObject.transform.Search(name).GetComponent<Button>();
		if (useSound) element.onClick.AddListener(() => GameManager.Sound.PlaySound(Define.SOUND_OPEN));
		element.onClick.AddListener(() => action?.Invoke());
		if (useAnimation) element.UseAnimation();

		return element;
	}

	public static Image FindImage(GameObject gameObject, string name)
	{
		Image element = gameObject.transform.Search(name).GetComponent<Image>();

		return element;
	}

	public static Slider FindSlider(GameObject gameObject, string name)
	{
		Slider element = gameObject.transform.Search(name).GetComponent<Slider>();

		return element;
	}

	public static ScrollRect FindScrollRect(GameObject gameObject, string name)
	{
		ScrollRect element = gameObject.transform.Search(name).GetComponent<ScrollRect>();

		return element;
	}

	public static TMP_InputField FindInputField(GameObject gameObject, string name)
	{
		TMP_InputField element = gameObject.transform.Search(name).GetComponent<TMP_InputField>();

		return element;
	}

	public static Transform FindTransform(GameObject gameObject, string name)
	{
		return gameObject.transform.Search(name);
	}

	public static ParticleSystem FindParticle(GameObject gameObject, string name)
	{
		return gameObject.transform.Search(name).GetComponent<ParticleSystem>();
	}

	public static GameObject Instantiate(string _path, Vector3 _position, Quaternion _rotation, Transform _parent = null)
	{
		var prefab = UnityEngine.Resources.Load(_path);
		var gameObject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;

		if (_parent != null) gameObject.transform.SetParent(_parent);

		gameObject.transform.localPosition = _position;
		gameObject.transform.localRotation = _rotation;

		return gameObject;
	}

	public static int GetTodayDayIndex()
	{
		DateTime today = DateTime.Now;
		DayOfWeek dayOfWeek = today.DayOfWeek;

		int index = ((int)dayOfWeek + 1) % 7;

		return index - 1;
	}

	public static int GetCurrentWeekNumber()
	{
		DateTime currentDate = System.DateTime.Now;

		System.Globalization.CultureInfo ciCurr = System.Globalization.CultureInfo.CurrentCulture;
		int weekCount = ciCurr.Calendar.GetWeekOfYear(currentDate, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);

		return weekCount;
	}

	public static int GetWeekOfMonth(DateTime currentDate)
	{
		DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

		int weekOfMonth = (currentDate.Day - 1 + (int)firstDayOfMonth.DayOfWeek) / 7 + 1;

		return weekOfMonth;
	}

	public static string GetMonthName(DateTime date)
	{
		return date.ToString("MMMM");
	}

	public static string GetDayOfWeek(int index)
	{
		string day;

		switch (index)
		{
			case 0:
				day = "Sun";
				break;
			case 1:
				day = "Mon";
				break;
			case 2:
				day = "Tue";
				break;
			case 3:
				day = "Wed";
				break;
			case 4:
				day = "Thu";
				break;
			case 5:
				day = "Fri";
				break;
			case 6:
				day = "Sat";
				break;
			default:
				day = "Invalid day";
				break;
		}

		return day;
	}

	public static string GetMonthName(int month)
	{
		string monthName;

		switch (month)
		{
			case 1:
				monthName = "January";
				break;
			case 2:
				monthName = "February";
				break;
			case 3:
				monthName = "March";
				break;
			case 4:
				monthName = "April";
				break;
			case 5:
				monthName = "May";
				break;
			case 6:
				monthName = "June";
				break;
			case 7:
				monthName = "July";
				break;
			case 8:
				monthName = "August";
				break;
			case 9:
				monthName = "September";
				break;
			case 10:
				monthName = "October";
				break;
			case 11:
				monthName = "November";
				break;
			case 12:
				monthName = "December";
				break;
			default:
				monthName = "Invalid month";
				break;
		}

		return monthName;
	}

	public static string FormatTime(float seconds)
	{
		TimeSpan time = TimeSpan.FromSeconds(seconds);

		return string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
	}

	public static string GetLanguage()
	{
		SystemLanguage deviceLanguage = Application.systemLanguage;

		if (deviceLanguage != SystemLanguage.English && deviceLanguage != SystemLanguage.Korean)
		{
			return "eng";
		}

		return "kor";
	}

	public static T DeepCopy<T>(T obj)
	{
		if (!typeof(T).IsSerializable)
		{
			throw new ArgumentException("The type must be serializable.", nameof(obj));
		}

		if (ReferenceEquals(obj, null))
		{
			return default(T);
		}

		IFormatter formatter = new BinaryFormatter();
		Stream stream = new MemoryStream();

		using (stream)
		{
			formatter.Serialize(stream, obj);
			stream.Seek(0, SeekOrigin.Begin);
			return (T)formatter.Deserialize(stream);
		}
	}

	public static void FadeCanvasGroup(CanvasGroup current, float target, float lerpspeed = 1f, float delay = 0f, Action _start = null, Action end = null)
	{
		Util.RunCoroutine(Co_FadeCanvasGroup(current, target, lerpspeed, _start, end).Delay(delay), current.GetHashCode().ToString(), CoroutineTag.UI);
	}

	private static IEnumerator<float> Co_FadeCanvasGroup(CanvasGroup current, float target, float lerpspeed = 1f, Action start = null, Action end = null)
	{
		float lerpvalue = 0f;

		start?.Invoke();

		if (target == 0f) current.blocksRaycasts = false;

		while (Mathf.Abs(current.alpha - target) >= 0.001f)
		{
			current.alpha = Mathf.Lerp(current.alpha, target, lerpvalue += lerpspeed * Time.deltaTime);

			yield return Timing.WaitForOneFrame;
		}

		current.alpha = target;

		if (target == 1f) current.blocksRaycasts = true;

		end?.Invoke();
	}

	public static void PingPongCanvasGroup(CanvasGroup current, float target, float lerpSpeed = 1f, float delay = 0f)
	{
		RunCoroutine(Co_PingPongCanvasGroup(current, target, lerpSpeed).Delay(delay), current.GetHashCode().ToString(), CoroutineTag.UI);
	}

	private static IEnumerator<float> Co_PingPongCanvasGroup(CanvasGroup current, float target, float lerpspeed = 1f)
	{
		float lerpvalue = 0f;
		float startAlpha = current.alpha;
		bool pingPongForward = true;

		while (true)
		{
			while ((pingPongForward && Mathf.Abs(current.alpha - target) >= 0.001f) || (!pingPongForward && Mathf.Abs(current.alpha - startAlpha) >= 0.001f))
			{
				float newAlpha = Mathf.Lerp(current.alpha, pingPongForward ? target : startAlpha, lerpvalue += lerpspeed * Time.deltaTime);

				current.alpha = newAlpha;

				yield return Timing.WaitForOneFrame;
			}

			pingPongForward = !pingPongForward;
			lerpvalue = 0f; // 핑퐁 방향이 변경될 때 lerpvalue를 초기화합니다.

			yield return Timing.WaitForOneFrame;
		}
	}

	public static void SetIntToTargetAnimation(TMP_Text text, int start, int target, float _duration = .25f, float delay = 0f, string addon = "") => RunCoroutine(Co_SetIntToTargetAnimation(text, start, target, _duration, addon).Delay(delay), nameof(Co_SetIntToTargetAnimation) + text.GetHashCode(), CoroutineTag.Content);

	private static IEnumerator<float> Co_SetIntToTargetAnimation(TMP_Text txtmp, int start, int target, float _duration = .25f, string addon = "")
	{
		float elapsedTime = 0f;

		while (elapsedTime < _duration)
		{
			float time = Mathf.SmoothStep(0f, 1f, elapsedTime / _duration);
			int value = Mathf.RoundToInt(Mathf.Lerp(start, target, time));

			txtmp.text = value.ToString() + addon;

			elapsedTime += Time.deltaTime;

			yield return Timing.WaitForOneFrame;
		}

		txtmp.text = target.ToString() + addon;
	}

	public static Transform Search(this Transform _target, string name)
	{
		if (_target.name == name) return _target;

		for (int i = 0; i < _target.childCount; ++i)
		{
			var result = Search(_target.GetChild(i), name);

			if (result != null) return result;
		}

		return null;
	}

	public static string RGBToHex(int _red, int _green, int _blue)
	{
		string hex = "#" + _red.ToString("X2") + _green.ToString("X2") + _blue.ToString("X2");

		return hex;
	}

	public static Color HexToRGB(string hex)
	{
		if (hex.StartsWith("#"))
		{
			hex = hex.Substring(1);
		}

		byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

		return new Color(r, g, b, 255) / 255f;
	}

	public static void EaseScale(Transform _transform, Vector3 _scale, float _lerpSpeed = 1f, EasingFunction.Ease _easeMode = EasingFunction.Ease.EaseOutBack, int hasCode = 0) => Timing.RunCoroutine(Co_EaseScale(_transform, _scale, _lerpSpeed, _easeMode), hasCode);

	private static IEnumerator<float> Co_EaseScale(Transform _transform, Vector3 _size, float _lerpSpeed, EasingFunction.Ease _easeMode)
	{
		float lerpvalue = 0f;

		while (lerpvalue <= 1f)
		{
			Function function = GetEasingFunction(_easeMode);

			float x = function(_transform.localScale.x, _size.x, lerpvalue);
			float y = function(_transform.localScale.y, _size.y, lerpvalue);
			float z = function(_transform.localScale.z, _size.z, lerpvalue);

			lerpvalue += _lerpSpeed * Time.deltaTime;

			_transform.localScale = new Vector3(x, y, z);

			yield return Timing.WaitForOneFrame;
		}

		_transform.transform.localScale = _size;
	}

	public static void EaseScale(Transform _transform, float _scale, float _lerpSpeed = 1f, EasingFunction.Ease _easeMode = EasingFunction.Ease.EaseOutBack) => Timing.RunCoroutine(Co_EaseScale(_transform, _scale, _lerpSpeed, _easeMode), (int)CoroutineTag.UI);

	private static IEnumerator<float> Co_EaseScale(Transform _transform, float _size, float _lerpSpeed, EasingFunction.Ease _easeMode)
	{
		float lerpvalue = 0f;

		while (lerpvalue <= 1f)
		{
			Function function = GetEasingFunction(_easeMode);

			float x = function(_transform.localScale.x, _size, lerpvalue);
			float y = function(_transform.localScale.y, _size, lerpvalue);
			float z = function(_transform.localScale.z, _size, lerpvalue);

			lerpvalue += _lerpSpeed * Time.deltaTime;

			_transform.localScale = new Vector3(x, y, z);

			yield return Timing.WaitForOneFrame;
		}

		_transform.transform.localScale = Vector3.one * _size;
	}

	public static string ConvertGoldToUnit(double amount)
	{
		if (amount >= 1000000000000000)
		{
			return (amount / 1000000000000000f).ToString("N2") + " QT";
		}
		else if (amount >= 1000000000000)
		{
			return (amount / 1000000000000f).ToString("N2") + " T";
		}
		else if (amount >= 1000000000)
		{
			return (amount / 1000000000f).ToString("N2") + " B";
		}
		else if (amount > 1000000)
		{
			return (amount / 1000000f).ToString("N2") + " M";
		}
		return amount.ToString("N0");
	}

	public static void ShakeUI(RectTransform rect)
	{
		rect.DOShakePosition(0.25f, new Vector3(2f, 2f, 0), 40, 90f, false);
	}

	public static void FadeVolume(AudioSource audioSource, float targetVolume, float duration)
	{
		RunCoroutine(Co_FadeVolume(audioSource, targetVolume, duration), nameof(Co_FadeVolume) + audioSource.GetHashCode(), CoroutineTag.Content);
	}

	private static IEnumerator<float> Co_FadeVolume(AudioSource audioSource, float targetVolume, float duration)
	{
		if (audioSource == null) yield break;

		float startVolume = audioSource.volume;
		float elapsedTime = 0f;

		while (elapsedTime < duration)
		{
			if (audioSource == null) yield break;

			elapsedTime += Timing.DeltaTime;

			audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);

			yield return Timing.WaitForOneFrame;
		}

		audioSource.volume = targetVolume;
	}


	public static byte[] ConvertMidToBytes(string midiFileName)
	{
		string inputPath = Path.Combine(Application.streamingAssetsPath, "Midi", midiFileName + ".mid");

		// Debug.Log("MIDI 경로: " + inputPath);

		if (File.Exists(inputPath))
		{
			byte[] fileData = File.ReadAllBytes(inputPath);
			// Debug.Log($"바이트 변환 완료: {inputPath}");
			return fileData;
		}
		else
		{
			// Debug.LogError($"MIDI 파일이 존재하지 않습니다: {inputPath}");
			return null;
		}
	}
}



#region Extensions

public static class TMPTextExtensions
{
	public static void UsePingPong(this TMP_Text txtmp)
	{
		txtmp.gameObject.AddComponent<TextAnimation>();
		txtmp.gameObject.AddComponent<CanvasGroup>();
	}
	public static void StartPingPong(this TMP_Text txtmp, float pingpongSpeed = 1f)
	{
		txtmp.GetComponent<TextAnimation>().StartPingPong(pingpongSpeed);
	}

	public static void StopPingPong(this TMP_Text txtmp)
	{
		txtmp.GetComponent<TextAnimation>().StopPingPong();
	}

	public static void UseShadow(this TMP_Text txtmp)
	{
		var text = txtmp.transform.parent.Search("txtmp_Shadow").GetComponent<TMP_Text>();

		text.text = txtmp.text;
	}
}

public static class ButtonExtensions
{
	public static void UseAnimation(this Button button, float targetScale = .95f)
	{
		button.gameObject.AddComponent<ButtonAnimation>();
		button.gameObject.GetComponent<ButtonAnimation>().targetScale = targetScale;
	}

	public static void UseTextHover(this Button button, Color highlightTextColor)
	{
		var buttonText = button.transform.GetComponentInChildren<TMP_Text>();

		button.GetComponent<ButtonAnimation>().useTextHover = true;
		button.GetComponent<ButtonAnimation>().defaultTextColor = buttonText.color;
		button.GetComponent<ButtonAnimation>().highlightTextColor = highlightTextColor;
		button.GetComponent<ButtonAnimation>().buttonText = buttonText;
	}

	public static void UseClickAction(this Button button, Action _pointerDown = null, Action _pointerUp = null)
	{
		button.gameObject.AddComponent<ButtonClickAction>();

		button.gameObject.GetComponent<ButtonClickAction>().action_PointerDown = _pointerDown;
		button.gameObject.GetComponent<ButtonClickAction>().action_PointerUp = _pointerUp;
	}
}

#endregion