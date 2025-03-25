using MEC;
using System;
using System.Collections.Generic;
using static Enums;

public class Splash_Base : UI_Base
{
	protected bool isInitialized = false;

	public float timeout = 1f;
	public bool useAutoTimeOut = true;

	protected Action endAction;

	protected virtual void OnEnable()
	{
		if (isInitialized)
		{
			if (!useAutoTimeOut)
			{
				return;
			}

			Util.RunCoroutine(Co_DisableAfterSeconds(timeout), nameof(Co_DisableAfterSeconds) + this.GetHashCode(), CoroutineTag.UI);
		}

		isInitialized = true;
	}

	private void OnDisable()
	{
		Util.KillCoroutine(nameof(Co_DisableAfterSeconds) + this.GetHashCode());
	}

	private IEnumerator<float> Co_DisableAfterSeconds(float _timeout)
	{
		while (_timeout > 0)
		{
			yield return Timing.WaitForSeconds(1);

			_timeout--;
		}

		GameManager.UI.PopSplash();

		endAction?.Invoke();
	}

	public void SetEndAction(Action _action)
	{
		endAction = _action;
	}
}
