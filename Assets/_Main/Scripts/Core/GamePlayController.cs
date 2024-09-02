using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GamePlayController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Board board;
    [SerializeField] private ShapeSpawner spawner;

    [Header("Configures")]
    [SerializeField] private float dropInterval = 0.3f;

    private Shape activeShape;
    private float dropTimer = 0f;

    private void Start()
    {
        if(!activeShape)
        {
            activeShape = spawner.SpawnShape();
        }
    }

    private void Update()
    {
        if(activeShape && dropTimer <= 0f)
        {
            activeShape.MoveDown();
            dropTimer = dropInterval;
        }

        dropTimer -= Time.deltaTime;
    }
}
