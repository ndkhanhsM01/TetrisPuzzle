using System;
using System.Collections.Generic;

[Serializable]
public class UserStorage
{
    public List<UserInfo> AllUsers = new();

    public UserStorage(List<UserInfo> listSequenceUser)
    {
        AllUsers = new();
        AllUsers.AddRange(listSequenceUser);
    }
}