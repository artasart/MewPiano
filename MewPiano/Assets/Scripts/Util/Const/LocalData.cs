using EnhancedScrollerDemos.NestedScrollers;
using System;
using System.Collections.Generic;
using UnityEngine.Device;

public static class LocalData
{
	public static UserData_Test userData;

	public static void SaveUserData()
	{
		DebugManager.Log("User Data saved.", DebugColor.Data);
	}

	public static void LoadUserData()
	{
		userData = JsonManager<UserData_Test>.LoadData(Define.JSON_USERDATA);

		if(userData == null)
		{
			DebugManager.Log("Warnning..! There is no userdata.", DebugColor.Data);

			// SaveUserData();
		}
	}
}

public class UserData_Test
{

}

public static class SceneData
{

}