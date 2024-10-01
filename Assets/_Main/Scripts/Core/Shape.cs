using MLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public int ID;
    [SerializeField] private bool canRotate = true;
    [SerializeField] private CellPooler cellPooler;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Vector2[] cellsPosition;

    [Header("Debug")]
    [SerializeField] private float sizeBorder = 1f;
    public Transform Body { get; private set; }

    public List<Cell> Cells { get; private set; }

    private bool activeTrail = false;

    public bool InPool { get; set; }
    private void Awake()
    {
        Body = transform;
    }

    private void OnDrawGizmosSelected()
    {
        if(cellsPosition == null || cellsPosition.Length == 0) return;
        if(!Body) Body = transform;
        Gizmos.color = defaultColor;
        foreach(Vector2 position in cellsPosition)
        {
            Vector3 targetPos = Body.position + (Vector3) position;
            targetPos.z = Body.position.z;
            Gizmos.DrawWireCube(targetPos, Vector3.one * sizeBorder);
        }
    }

    private void Move(Vector3 direction)
    {
        Body.position += direction;
    }

    public void Initialize()
    {
        Cells = new List<Cell>();
        for(int i=0; i<cellsPosition.Length; i++)
        {
            Vector3 position = cellsPosition[i];
            position.z = Body.position.z;
            Cells.Add(cellPooler.GetOne(Body, position));
        }
        DisableTrail();
        SetColor(defaultColor);
    }

    [MButton]
    private void InitPreview()
    {
        Cells = new List<Cell>();
        Transform center = transform;
        for (int i = 0; i < cellsPosition.Length; i++)
        {
            Cell cellClone = Instantiate(cellPrefab, center);
            Cells.Add(cellClone);
            Vector3 position = cellsPosition[i];
            position.z = center.position.z;
            cellClone.transform.localPosition = position;
        }
        DisableTrail();
        SetColor(defaultColor);
    }
    public void SetColor(Color color)
    {
        if(Cells == null || Cells.Count == 0) return;
        
        foreach(Cell cell in Cells)
        {
            cell.Graphic.color = color;
        }
    }

    public void MoveLeft()
    {
        Move(Vector3.left);
    }

    public void MoveRight()
    {
        Move(Vector3.right);
    }

    public void MoveDown()
    {
        Move(Vector3.down);
    }

    public void MoveUp()
    {
        Move(Vector3.up);
    }

    public void RotateRight()
    {
        if (!canRotate) return;
        Body.Rotate(0f, 0f, -90f);
    }
    public void RotateLeft()
    {
        if (!canRotate) return;
        Body.Rotate(0f, 0f, 90f);
    }

    public void EnableTrail()
    {
        foreach (Cell cell in Cells)
        {
            if (cell) cell.FX.enabled = true;
        }
    }
    public void DisableTrail()
    {
        foreach (Cell cell in Cells)
        {
            if (cell) cell.FX.enabled = false;
        }
    }
}
