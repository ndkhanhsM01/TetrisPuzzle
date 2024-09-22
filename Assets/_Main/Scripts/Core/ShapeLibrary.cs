using UnityEngine;
using System.Collections.Generic;


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

    private Dictionary<int, Shape> shapePooler = new Dictionary<int, Shape>();
    public void InitializeShapePooler(Transform parent)
    {
        shapePooler = new();
        foreach (ShapeConfig config in All)
        {
            Shape shape = Instantiate(config.Prefab, parent);
            shape.SetActive(true);
            shapePooler.Add(shape.ID, shape);
        }
    }
    public Shape GetRandom()
    {
        int target = Random.Range(0, shapePooler.Count);
        int count = 0;
        foreach(Shape shape in shapePooler.Values)
        {
            if (count == target)
            {
                //shape.SetActive(true);
                return shape;
            }
            count++;
        }

        Debug.LogError("Wronggg!!");
        return null;
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