using System;
using System.Collections;
using System.Collections.Generic;

namespace MLib
{
    [System.Serializable]
    public class LocalData
    {
        public static Action<LocalData> OnInitData;

        // Information
        public string userName;
        public int userID;

        // Setting
        public bool isPlayMusic = true;
        public bool isPlaySoundFX = true;
        public float touchSensitivity = 0f;

        // Storage
        public int highScore;
        public int coin;

        // Data unlock
        public int usingBackground = -1;
        public Dictionary<int, bool> itemsBackground = new Dictionary<int, bool>();
        public LocalData()
        {
            userName = GameConstraint.DefaultNameUser;
            userID = -1;

            isPlayMusic = true;
            isPlaySoundFX = true;
            touchSensitivity = 0f;

            itemsBackground = new Dictionary<int, bool>();
            usingBackground = -1;
        }

        public void Initialize_ItemsBackground(SOItemShop[] allData)
        {
            Dictionary<int, bool> dictDatas = new Dictionary<int, bool>();
            foreach (SOItemShop data in allData)
            {
                if (data.IsDefault)
                {
                    dictDatas.Add(data.ID, true);
                    usingBackground = data.ID;
                }
                else
                {
                    dictDatas.Add(data.ID, false);
                }
            }
            itemsBackground = dictDatas;
        }
    }
}
