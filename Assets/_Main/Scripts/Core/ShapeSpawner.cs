using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Shape[] libraryShapes;

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
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spawnPosition, 0.5f);
    }
    public Shape GetRandomShape()
    {
        int index = Random.Range(0, libraryShapes.Length);
        if (libraryShapes[index])
        {
            return libraryShapes[index];
        }

        return null;
    }

    public Shape SpawnShape()
    {
        Shape shape = null;
        Vector3 targetPosition = new Vector3(spawnPosition.ToIntRound().x, spawnPosition.ToIntRound().y);

        if (!enableCheat)
        {
            shape = Instantiate(GetRandomShape(), targetPosition, Quaternion.identity, shapesHolder);
        }
        else
        {
            shape = Instantiate(shapeCheat, targetPosition, Quaternion.identity, shapesHolder);
        }

        return shape;
    }
}
