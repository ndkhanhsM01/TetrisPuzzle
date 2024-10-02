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

        public void Load()
        {
            // load file
            string path = Application.persistentDataPath + "/" + fileName;
            LocalData = MHelper.LoadDataFromFile<LocalData>(path, true);
            if( LocalData == null ) LocalData = new LocalData();
        }

        public void Save()
        {
            string path = Application.persistentDataPath + "/" + fileName;
            MHelper.SaveDataIntoFile(path, LocalData);
        }
    }
}