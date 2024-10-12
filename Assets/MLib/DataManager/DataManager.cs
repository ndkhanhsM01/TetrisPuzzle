using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;


namespace MLib
{
    public class DataManager : MSingleton<DataManager>
    {
        [SerializeField] private bool showDebug = true;
        [SerializeField] private string fileName = "SaveData.json";

        [HideInInspector] public LocalData LocalData;

        public static Action<LocalData> OnLoadLocalSuccess;
        public bool IsLoadSuccess { get; private set; }
        public int Coin
        {
            get
            {
                return LocalData == null ? -1 : LocalData.coin;
            }
            set
            {
                if (LocalData == null) return;
                LocalData.coin = value;
                EventsCenter.OnCoinChanged?.Invoke(value);
            }
        }

        public string UserName
        {
            get
            {
                return LocalData == null ? null : LocalData.userName;
            }
            set
            {
                if(LocalData == null) return;
                LocalData.userName = value;
                EventsCenter.OnUserNameChanged?.Invoke(value);
            }
        }

        public int HighScore => LocalData == null ? -1 : LocalData.highScore;

        public bool TrySetNewHighScore(int score)
        {
            if(LocalData.highScore < score)
            {
                LocalData.highScore = score;

                GlobalDataManager.Instance.HttpCaller.Post_UpdateScore(LocalData.userID, LocalData.highScore);
                return true;
            }
            else
            {
                return false;
            }
        }

        public int usingBackground => LocalData == null ? -1 : LocalData.usingBackground;
        public Dictionary<int, bool> ItemsBackground => LocalData == null ? null : LocalData.itemsBackground;

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
            IsLoadSuccess = false;
            // load file
            string path = Application.persistentDataPath + "/" + fileName;
            LocalData = await MHelper.LoadDataFromFile<LocalData>(path, true);
            if( LocalData == null ) LocalData = new LocalData();
            IsLoadSuccess = true;

            if (Application.isPlaying)
            {
                OnLoadLocalSuccess?.Invoke(LocalData);
            }
        }

        public void Save()
        {
            string path = Application.persistentDataPath + "/" + fileName;
            MHelper.SaveDataIntoFile(path, LocalData);
        }
    }
}