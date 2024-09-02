using MLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform emptyCellPrefab;
    [SerializeField] private Transform gridHolder;
    [SerializeField] private SpriteRenderer spriteBorder;
    [SerializeField] private Camera mainCamera;

    [Header("Configures")]
    [SerializeField] private int width;
    [SerializeField] private int height;

    public Transform[,] Grid { get; private set; }

    private void Awake()
    {
        if(!mainCamera) mainCamera = Camera.main;

        
    }
    private void Start()
    {
        DrawCameraView();
        DrawEmptyCells();
    }

    [MButton]
    private void DrawEmptyCells()
    {
        ClearOldCells();
        Grid = new Transform[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Transform newCell = Instantiate(emptyCellPrefab, gridHolder);
                newCell.localPosition = new Vector3(x, y);
                newCell.gameObject.name = $"Cell ({x}, {y})";
                Grid[x, y] = newCell;
            }
        }

        spriteBorder.size = new Vector2(width + 0.15f, height + 0.15f);
        spriteBorder.transform.position = gridHolder.position + new Vector3(width / 2 - 0.5f, height / 2 - 0.5f, 0);
    }

    [MButton]
    private void ClearOldCells()
    {
        int childCount = gridHolder.childCount;
        for(int i= childCount - 1; i>=0; i--)
        {
            DestroyImmediate(gridHolder.GetChild(i).gameObject);
        }
    }

    private void DrawCameraView()
    {
        // apply camera view by width/height size
        
    }
}
