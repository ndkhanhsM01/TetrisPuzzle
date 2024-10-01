using MLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShapeLibrary;

public class ShapeSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ShapeLibrary shapeLibrary;
    [SerializeField] private CellPooler cellPooler;

    [Header("Configures")]
    [SerializeField] private Vector2 spawnPosition;

    [Header("For Cheat")]
    [SerializeField] private bool enableCheat = false;
    [SerializeField] private Shape shapeCheat;

    private Transform shapesHolder;
    private Queue<Shape> shapesQueue;
    private void Awake()
    {
        shapesHolder = transform;

#if !UNITY_EDITOR
        enableCheat = false;
#endif

        cellPooler.Initialize(shapesHolder);
        shapeLibrary.InitializeShapePooler(shapesHolder);
        PrepareShapes();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spawnPosition, 0.5f);
    }

    public Shape GetNextShape(bool active = true)
    {
        if (shapesQueue == null || shapesQueue.Count <= 0)
        {
            return null;
        }

        Shape result = shapesQueue.Dequeue();
        PreviewNextShape.Instance.ShowNext(shapesQueue.Peek().ID);
        PrepareShapes();

        if(result != null) result.SetActive(active);
        return result;
    }
    public void PrepareShapes()
    {
        int total = 2;
        if(shapesQueue == null) shapesQueue = new Queue<Shape>();

        int spawnNeed = total - shapesQueue.Count;
        while(spawnNeed > 0)
        {
            Shape newShape = SpawnShape();
            newShape.SetActive(false);
            shapesQueue.Enqueue(newShape);
            spawnNeed--;
        }
    }
    public void ReturnShapeToPool(Shape shape)
    {
        shape.SetActive(false);
        shape.InPool = true;
    }
    private Shape SpawnShape()
    {
        Shape shape = null;
        Vector3 targetPosition = new Vector3(spawnPosition.ToIntRound().x, spawnPosition.ToIntRound().y);

        if (!enableCheat)
        {
            shape = shapeLibrary.GetRandom();
            //shape = GetNextShape();
            shape.Initialize();
            shape.Body.position = targetPosition;
        }
        else
        {
            shape = Instantiate(shapeCheat, targetPosition, Quaternion.identity, shapesHolder);
        }

        return shape;
    }

    public List<Cell> GetCellsFromPool(int amount)
    {
        return new List<Cell>();
    }
}
