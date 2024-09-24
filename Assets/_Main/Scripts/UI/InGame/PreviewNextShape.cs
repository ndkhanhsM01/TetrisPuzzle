using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLib;
using static ShapeLibrary;
using System.Drawing;
public class PreviewNextShape : MSingleton<PreviewNextShape> 
{
    [SerializeField] private ShapeLibrary shapeLibrary;
    [SerializeField] private Transform shapeHolder;
    [SerializeField] private Camera cam;

    private Dictionary<int, Shape> dictShapes = new();
    private void OnEnable()
    {
        EventsCenter.OnSceneLoaded += PrepareShapes;
    }
    private void OnDisable()
    {
        EventsCenter.OnSceneLoaded -= PrepareShapes;
    }
    public void ShowNext(int idShape)
    {
        HideAllShape();

        dictShapes[idShape].SetActive(true);
    }

    private void HideAllShape()
    {
        foreach(var shape in dictShapes.Values)
        {
            shape.SetActive(false);
        }
    }

    private void PrepareShapes()
    {
        dictShapes = new();
        foreach (ShapeConfig config in shapeLibrary.All)
        {
            var clone = SpawnNewShape(config.Prefab);
            clone.SetActive(false);
            dictShapes.TryAdd(config.Prefab.ID, clone);
        }
    }
    private Shape SpawnNewShape(Shape originalShape)
    {
        Shape cloner = Instantiate(originalShape);
        cloner.gameObject.name = $"Preview_{originalShape.name}";
        cloner.Body.parent = shapeHolder;
        cloner.Body.localPosition = Vector3.zero;
        cloner.Initialize();

        return cloner;
    }
}
