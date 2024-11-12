using MLib;
using System;
using UnityEngine;

public class FakeUserManager: MSingleton<FakeUserManager>
{
    [SerializeField] private FakeDataUsers fakeDataUsers;

    private UserStorage data;

    public static Action<UserStorage> OnLoadSuccess;

    private const string fileName = "RankData.json";

    public UserStorage Data => data;

    protected override void Awake()
    {
        base.Awake();
        Load();
    }
    private void OnApplicationQuit()
    {
        Save();
    }
    private void OnApplicationFocus(bool focus)
    {
        Save();
    }
    public async void Load()
    {
        // load file
        string path = Application.persistentDataPath + "/" + fileName;
        data = await MHelper.LoadDataFromFile<UserStorage>(path, true);
        if (data == null)
        {
            string namePlayer = GameConstraint.DefaultNameUser;
            int scorePlayer = 0;
            var beginUsers = await fakeDataUsers.SpawnNewFakeList(namePlayer, scorePlayer, 99);
            data = new(beginUsers);
        }

        OnLoadSuccess?.Invoke(data);
    }

    public void Save()
    {
        string path = Application.persistentDataPath + "/" + fileName;
        MHelper.SaveDataIntoFile(path, data);
    }
}