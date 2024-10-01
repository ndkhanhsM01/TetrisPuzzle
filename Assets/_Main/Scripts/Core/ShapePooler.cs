

using System.Collections.Generic;
using UnityEngine;

public class ShapePooler
{
    private Dictionary<Shape, bool> dictShapes;
    private List<Shape> readyShapes;

    private Shape prefab;
    private Transform holder;
    public ShapePooler(int startValue, Shape prefab, Transform holder)
    {
        this.holder = holder;
        this.prefab = prefab;

        dictShapes = new();
        readyShapes = new();

        for(int i=0; i<startValue; i++)
        {
            CloneNewShape();
        }
    }

    public Shape Get()
    {
        MakeSureEnough();

        Shape result = readyShapes[0];
        dictShapes[result] = false;

        return result;
    }
    public void MakeSureEnough()
    {
        if (readyShapes.Count > 0) return;

        CloneNewShape();
    }

    private void CloneNewShape()
    {
        Shape clone = UnityEngine.Object.Instantiate(prefab, holder);

        dictShapes.Add(clone, false);
        readyShapes.Add(clone);
    }
}