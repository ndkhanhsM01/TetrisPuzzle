using System.Diagnostics;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using Newtonsoft.Json;


namespace MLib
{
    
    public class MDebugWindow : EditorWindow
    {
        GUIStyle titleStyle;

        #region Button style
        #endregion

        #region Time scale variables
        AnimBool animShowTimeScaleFields;
        float maxValue = 1f;
        float timeScale = 1f;
        #endregion

        #region Data variables
        string serializedData;
        bool showExpandData;
        string fileName = "SaveData.json";
        string pathFileData => Application.persistentDataPath + "/" + fileName;
        LocalData localData = new();
        AnimBool animShowDataFields;
        GUIStyle styleDataText = new();
        #endregion

        #region others
        bool showOthers;
        #endregion

        [MenuItem("MLib/Debug Window")]
        public static void ShowExample()
        {
            MDebugWindow wnd = GetWindow<MDebugWindow>();
            wnd.titleContent = new GUIContent("Debug Window");
        }
        private void OnEnable()
        {
            titleStyle = new();
            titleStyle.alignment = TextAnchor.MiddleCenter;

            animShowTimeScaleFields = new AnimBool(false);
            animShowTimeScaleFields.valueChanged.AddListener(Repaint);

            serializedData = JsonConvert.SerializeObject(localData, Formatting.Indented);
            animShowDataFields = new AnimBool(false);
            animShowDataFields.valueChanged.AddListener(Repaint);
            styleDataText = new GUIStyle(EditorStyles.textArea);
            styleDataText.wordWrap = true;
        }
        private void OnGUI()
        {
            SetEditorTimeScale();

            EditorGUILayout.Space(10f);
            SetEditorData();

            EditorGUILayout.Space(10f);
            SetEditorPingResorucesFolder();
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
            {
                pix[i] = col;
            }

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }
        private void SetEditorTimeScale()
        {
            animShowTimeScaleFields.target = EditorGUILayout.ToggleLeft("Enable edit time scale in realtime", animShowTimeScaleFields.target);
            if (EditorGUILayout.BeginFadeGroup(animShowTimeScaleFields.faded))
            {
                if(!Application.isPlaying)
                {
                    EditorGUILayout.HelpBox("Edit time scale only work in \"Playing mode\"", MessageType.Warning);
                }

                maxValue = EditorGUILayout.FloatField("Max", maxValue);
                timeScale = EditorGUILayout.Slider("Value", timeScale, 0f, maxValue);

                if (Application.isPlaying)
                {
                    Time.timeScale = timeScale;
                }
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void SetEditorData()
        {
            animShowDataFields.target = EditorGUILayout.BeginFoldoutHeaderGroup(animShowDataFields.target, "Local Data");
            if (animShowDataFields.target)
            {
                fileName = EditorGUILayout.TextField("File name", fileName);

                showExpandData = EditorGUILayout.Foldout(showExpandData, "Edit JSON Data");

                if (showExpandData)
                {
                    // Display TextArea and update serializedData with its content
                    serializedData = EditorGUILayout.TextArea(serializedData, styleDataText);
                }

                if (GUILayout.Button("Save data into disk"))
                {
                    localData = JsonConvert.DeserializeObject<LocalData>(serializedData);
                    MHelper.SaveDataIntoFile(pathFileData, localData);
                }
                if (GUILayout.Button("Reload data from disk"))
                {
                    localData = MHelper.LoadDataFromFile<LocalData>(pathFileData);

                    serializedData = JsonConvert.SerializeObject(localData, Formatting.Indented);
                }
                if (GUILayout.Button("Clear all data"))
                {
                    localData = new();
                    PlayerPrefs.DeleteAll();
                    MHelper.SaveDataIntoFile(pathFileData, localData);

                    serializedData = JsonConvert.SerializeObject(localData, Formatting.Indented);
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void SetEditorPingResorucesFolder()
        {
            showOthers = EditorGUILayout.Foldout(showOthers, "Others");
            if (showOthers)
            {
                if (GUILayout.Button("OPEN SAVE FILE FOLDER EXPLORER"))
                {
                    OpenFolder(Application.persistentDataPath);
                }
            }
        }

        private void OpenFolder(string path)
        {
            if (System.IO.Directory.Exists(path))
            {
                string windowsPath = path.Replace("/", "\\");
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = windowsPath,
                    FileName = "explorer.exe"
                };
                Process.Start(startInfo);
            }
            else
            {
                UnityEngine.Debug.LogError("Folder path does not exist: " + path);
            }
        }

    }
}
