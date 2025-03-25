using UnityEngine;

public static class Define
{
	[Header("Core")]
	public const string GAMEMANAGER = "GameManager";
	public const string GAMEWORLD = "GameWorld";
	public const string GAMEUI = "GameUI";

	[Header("ResourcePath")]
	public const string PATH_IMAGES = "Images/";
	public const string PATH_ALBUMART = PATH_IMAGES + "AlbumArt/";
	public const string PATH_PREFABS = "Prefabs/";
	public const string PATH_SOUNDS = "Sounds/";
	public const string PATH_ICONS = PATH_SPRITES + "Icons/";
	public const string PATH_SPRITES = PATH_IMAGES + "Sprites/";
	public const string PATH_MANAGER = PATH_PREFABS + "Manager/";
	public const string PATH_UI = PATH_PREFABS + "UI/";
	public const string PATH_GAMEOBJECT = PATH_PREFABS + "GameObject/";

	[Header("BackEnd")]
	public const string USER_DATA = "USER_DATA";
	public const string UUID_RANK = "019481a4-043a-7e0b-91c5-0bd0275374bf";

	public const string CHART_ITEM = "159515";
	public const string CHART_PC = "160822";
	public const string CHART_CUSTOMER = "160436";

	public const string GOOGLETOKEN = "토큰값 입력 필요";
	public const string APPLEUSERID = "AppleUserId";

	[Header("Sound")]
	public const string BGM = "BGM";
	public const string SOUNDEFFECT = "SoundEffect";
	public const string SOUND_CLOSE = "Click_2";
	public const string SOUND_OPEN = "Click_1";
	public const string SOUND_DENIED = "Denied";
	public const string SOUND_TOAST = "Toast";


	[Header("Color")]
	public const string COLOR_DISABLE = "#9EA4AA"; // Disable
	public const string COLOR_ENABLE = "#454C53";        // enable
	public const string COLOR_BACK = "#26282B";  // background
	public const string COLOR_SELECT = "#5F6DFF";        //		select
	public const string COLOR_FAVORITE = "#FEC800";   // yellow

	[Header("PlayerPerfsKey")]
	public const string KEY_FIRST = "First";
	public const string KEY_BGM = "BGMVolume";
	public const string KEY_SOUNDEFFECT = "EffectVolume";

	public const string KEY_MYSONGLIST = "MySongList";
	public const string KEY_MYFAVORITESONGLIST = "MyFavoriteSongList";

	[Header("ObjectPool")]
	public const string SPAWN = "Spawn";
	public const string POOL = "Pool";

	[Header("Keys")]
	public const string GO_CANVAS = "go_Canvas";
	public const string GO_CANVASMASTER = "go_Canvas_Master";
	public const string GROUP_MODAL = "group_Modal";

	[Header("Data")]
	public const string JSON_USERDATA = "UserData";

	[Header("Color")]
	public static Color COLOR_DEFAULT = Util.HexToRGB("#DCDCDC");
	public static Color COLOR_WARNING = Util.HexToRGB("#D42323");
	public static Color COLOR_HIGHLIGHT = Color.yellow;

	public const string MOVEMENT = "Movement";
	public const string JUMP = "Jump";

	public const int JUMPRUN = 1;
	public const int JUMPSTAND = 2;

	public const string LAYER_INVISIBLE = "Invisible";
	public const string LAYER_DEFAULT = "Default";
}