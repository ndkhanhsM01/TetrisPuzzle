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
    private void Awake()
    {
        shapesHolder = transform;

#if !UNITY_EDITOR
        enableCheat = false;
#endif

        cellPooler.Initialize(shapesHolder);
        shapeLibrary.InitializeShapePooler(shapesHolder);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spawnPosition, 0.5f);
    }


    public Shape SpawnShape()
    {
        Shape shape = null;
        Vector3 targetPosition = new Vector3(spawnPosition.ToIntRound().x, spawnPosition.ToIntRound().y);

        if (!enableCheat)
        {
            shape = shapeLibrary.GetRandom();
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
