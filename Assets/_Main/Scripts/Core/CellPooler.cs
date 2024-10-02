using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cell Pooler", menuName = ("Tetris Setup/Cell Pooler"))]
public class CellPooler: ScriptableObject
{
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private int beginAmount = 30;

    private Dictionary<Cell, bool> allCells;
    private Queue<Cell> readyCells;

    private Transform holder;
    public void Initialize(Transform holder)
    {
        allCells = new Dictionary<Cell, bool>();
        readyCells = new Queue<Cell>();
        this.holder = holder;
        for(int i=0; i<beginAmount; i++)
        {
            CloneNewCell();
        }
    }

    public List<Cell> GetMany(int amount)
    {
        List<Cell> result = new List<Cell>();
        for(int i=0; i<amount; i++)
        {
            result.Add(GetReadyCell());

        }

        return result;
    }

    public Cell GetOne(Transform parent, Vector3 localPosition, bool active = true)
    {
        Cell cell = GetReadyCell();
        cell.Body.parent = parent;
        cell.Body.localPosition = localPosition;
        return cell;
    }

    public void ReturnCell(Cell cell)
    {
        if(!allCells.ContainsKey(cell))
        {
            Debug.LogError("Hey, this cell is not mine!!");
            return;
        }

        cell.SetActive(false);
        cell.Body.parent = holder;
        allCells[cell] = true;
        readyCells.Enqueue(cell);
    }

    public void ReturnCells(List<Cell> cells)
    {
        foreach(Cell cell in cells)
        {
            ReturnCell(cell);
        }
    }

    private void MakeSureEnoughCell()
    {
        if (readyCells.Count > 0) return;

        int moreValue = 10;

        for(int i=0; i<moreValue; i++)
        {
            CloneNewCell();
        }
    }
    private Cell GetReadyCell(bool active = true)
    {
        MakeSureEnoughCell();

        Cell cell = readyCells.Dequeue();
        allCells[cell] = false;
        cell.SetActive(active);
        return cell;
    }

    private void CloneNewCell()
    {
        Cell cellClone = Instantiate(cellPrefab, holder);
        allCells.Add(cellClone, true);
        readyCells.Enqueue(cellClone);
        cellClone.SetActive(false);
    }
}