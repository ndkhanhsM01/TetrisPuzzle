using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Shape[] libraryShapes;

    [Header("Configures")]
    [SerializeField] private Vector2 spawnPosition;

    private Transform shapesHolder;

    private void Awake()
    {
        shapesHolder = transform;
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
        Shape shape = Instantiate(GetRandomShape(), spawnPosition.ToCeil(), Quaternion.identity, shapesHolder);

        if (!shape) return null;

        return shape;
    }
}
