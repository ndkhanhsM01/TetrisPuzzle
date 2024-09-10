using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "ShapeLibrary", menuName = "Tetris Setup/Shape Library")]
public class ShapeLibrary: ScriptableObject
{
    [System.Serializable]
    public struct ShapeConfig
    {
        public Shape Prefab;
        public Color Color;
    }

    [SerializeField] private ShapeConfig[] library;

    public ShapeConfig[] All => library;

    public Shape GetRandom()
    {
        return library.GetRandom().Prefab;
    }
    public Shape SpawnRandom()
    {
        Shape shape = GetRandom();
        Shape cloner = Instantiate(shape);
        return cloner;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        ValidateIDShapes();
    }
    private void ValidateIDShapes()
    {
        for (int i = 0; i < library.Length; i++)
        {
            library[i].Prefab.ID = i;
            EditorUtility.SetDirty(library[i].Prefab);
        }
    }
#endif
}