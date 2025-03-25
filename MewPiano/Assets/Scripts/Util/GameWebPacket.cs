using System;

[Serializable]
public class DefaultRes
{
	public int state;
	public string message;
}

[Serializable]
public class LoginReq
{
	public string id;
	public string password;
}

[Serializable]
public class LoginRes : DefaultRes
{
	public string access;
	public UserData user;
}

[Serializable]
public class SaveRankReq
{
	public int user_id;
	public string nickname;
	public int score;
	public int combo;
	public int song_id;
	public int mode;
	public int difficulty_level;
	public string grade;
}

[Serializable]
public class RankReq
{
	public int song_id;
	public int mode;
	public int difficulty_level;
}

[Serializable]
public class RankRes : DefaultRes
{
	public int id;
	public int user_id;
	public string nickname;
	public int score;
	public int combo;
	public int song_id;
	public int difficulty_level;
	public int mode;
	public string created_at;
	public int ranking;
	public string grade;
}

[Serializable]
public class UserData
{
	public int id;
	public string name;
	public string nickname;
	public string image;
	public string role;
}

public class SaveRankData
{
	public int userId;
	public string nickname;
	public int score;
	public int combo;
	public int songId;
	public int mode;
	public int level;
	public string grade;
}