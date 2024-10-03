using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace MLib
{
    public static class MHelper
    {
        public static T LoadDataFromFile<T>(string path, bool createFileDefault = false)
        {
            T result = default;

            try
            {
                if (File.Exists(path))
                {
                    string content = File.ReadAllText(path);
                    result = JsonConvert.DeserializeObject<T>(content);
                    Debug.LogWarning($"Read data <{typeof(T)}> from file: {path}");
                }
                else
                {
                    Debug.LogWarning($"Not exist file: {path}");

                    if (createFileDefault)
                    {
                        SaveDataIntoFile<T>(path, result);
                    }
                }
            }
            catch
            {
                Debug.LogWarning("Load file failure!");
            }

            return result;
        }

        public static void SaveDataIntoFile<T>(string path, T data)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

                File.WriteAllText(path, jsonData);
                Debug.LogWarning($"Game data <{typeof(T)}> saved to: " + path);
            }
            catch
            {
                Debug.LogWarning("Save file failure!");
            }

        }

        public static void DeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    Debug.LogWarning("File cleared: " + path);
                }
                else
                    Debug.LogWarning("Save file failure!");
            }
            catch
            {
                Debug.LogWarning("Save file failure!");
            }
        }
    }

}