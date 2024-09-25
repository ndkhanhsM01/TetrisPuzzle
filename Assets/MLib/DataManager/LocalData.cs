using System.Collections;
using System.Collections.Generic;

namespace MLib
{
    [System.Serializable]
    public class LocalData
    {
        public bool isPlayMusic = true;
        public bool isPlaySoundFX = true;

        // Storage
        public int coin;

        // Data unlock
        public int usingBackground = -1;
        public Dictionary<int, bool> itemsBackground = new Dictionary<int, bool>();
        public LocalData()
        {
            isPlayMusic = true;
            isPlaySoundFX = true;
            itemsBackground = new Dictionary<int, bool>();
            usingBackground = -1;
        }

        public void Initialize_ItemsBackground(SOItemShop[] allData)
        {
            Dictionary<int, bool> dictDatas = new Dictionary<int, bool>();
            foreach (SOItemShop data in allData)
            {
                dictDatas.Add(data.ID, false);
            }
            DataManager.Instance.LocalData.itemsBackground = dictDatas;
        }
    }
}
