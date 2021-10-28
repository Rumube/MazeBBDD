using BBDD;
using System;
using UnityEngine;

public class UserInfo : IUserInfo
{
    Action<string> _createUserCallback;
    public string _nick { get; private set; }
    public string _password { get; private set; }
    public int _current_points { get; private set; }
    public int _global_points { get; private set; }

    public void Init()
    {
        _createUserCallback = (jsonArray) =>
        {

        };
    }
    public void SetInfo(string nick, string password, int current_points, int global_points)
    {
        _nick = nick;
        _password = password;
        _current_points = current_points;
        _global_points = global_points;
    }
    public void GetUserInfo()
    {
        Debug.Log("New user login: "+ _nick + " pass: " + _password + " curr: " +_current_points+ "glo: "+_global_points );
    }

    public string GetUser()
    {
        return _nick;
    }

    public string GetGlobalPoints()
    {
        return _global_points.ToString();
    }
}
