public static class Enums
{
	public enum CoroutineLayer
	{
		None,
		Util,
		Game,
		Login,
	}

	public enum CoroutineTag
	{
		None = -1,
		UI,
		Content,
		Web,
	}

	public enum SceneName
	{
		Logo,
		Title,
		Main,
		Game,
	}

	public enum TimeState
	{
		None,
		Play,
		Pause,
	}

	public enum ControlMode
	{
		Game,
		UI,
		GameAndUI,
	}
}
