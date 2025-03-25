#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class ScreenCaptureEditor
{
	[MenuItem("Tools/Screenshot/Take Screenshot of Game View %&s")]
	static void TakeScreenshot()
	{
		string path = Application.persistentDataPath + "/" + System.DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".png";
		ScreenCapture.CaptureScreenshot(path);
		DebugManager.Log("Save Screenshot Capture : " + path);
	}

	[MenuItem("Tools/Screenshot/Open Screenshot Folder %&o")]
	static void OpenScreenshotFolder()
	{
		string path = Application.persistentDataPath;
		Application.OpenURL("file://" + path);
	}
}
#endif