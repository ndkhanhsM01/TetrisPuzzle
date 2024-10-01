using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLib;
using static ShapeLibrary;
using System.Drawing;
public class PreviewNextShape : MSingleton<PreviewNextShape> 
{
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform shapeHolder;
    [SerializeField] private Camera cam;

    [SerializeField] private Shape[] allShapePreview;
    private Dictionary<int, Shape> dictShapes = new();

    protected override void Awake()
    {
        base.Awake();
        PrepareShapes();
    }
    public void ShowNext(int idShape)
    {
        HideAllShape();

        if(!dictShapes.ContainsKey(idShape))
        {
            Debug.LogError("Shape's ID not found");
            return;
        }

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
        foreach (Shape shape in allShapePreview)
        {
            dictShapes.TryAdd(shape.ID, shape);
            shape.SetActive(false);
        }
    }
}
