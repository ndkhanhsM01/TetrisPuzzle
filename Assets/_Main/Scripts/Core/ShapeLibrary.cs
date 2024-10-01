using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static UnityEditorInternal.VersionControl.ListControl;


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

    private Dictionary<int, List<Shape>> shapePooler = new Dictionary<int, List<Shape>>();

    private Transform shapeHolder;
    public void InitializeShapePooler(Transform parent)
    {
        shapeHolder = parent;
        shapePooler = new();
        int count = 2;
        foreach (ShapeConfig config in All)
        {
            int id = config.Prefab.ID;
            if (shapePooler.ContainsKey(id)) continue;

            List<Shape> listShapes = new List<Shape>();
            for (int i = 0; i < count; i++)
            {
                Shape shape = Instantiate(config.Prefab, parent);
                shape.SetActive(false);
                shape.InPool = true;
                listShapes.Add(shape);
            }
            shapePooler.Add(id, listShapes);
        }
    }
    public Shape GetRandom()
    {
        int target = Random.Range(0, shapePooler.Keys.ToArray().Length);

        return GetReadyShape(target);
    }

    public Shape GetReadyShape(int id)
    {
        List<Shape> listShape = shapePooler[id];

        foreach(Shape shape in listShape)
        {
            if (!shape.InPool) continue;

            shape.InPool = false;
            return shape;
        }


        // if not enough
        Shape prefab = null;
        foreach (ShapeConfig config in All)
        {
            if(config.Prefab.ID == id)
            {
                prefab = config.Prefab;
                break;
            }
        }

        if (!prefab) return null;

        Shape clone = Instantiate(prefab, shapeHolder);
        clone.SetActive(false);
        listShape.Add(clone);
        clone.InPool = false;
        return clone;
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