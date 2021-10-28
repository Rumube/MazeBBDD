using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserInfo 
{
    public void SetInfo(string nick,string password, int current_points, int global_points);
    public void Init();
    public void GetUserInfo();
    public string GetUser();
    public string GetGlobalPoints();


















}
