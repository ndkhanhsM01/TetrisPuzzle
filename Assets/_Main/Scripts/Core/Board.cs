using Cysharp.Threading.Tasks;
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

    private ScoreSystem scoreSystem => ScoreSystem.Instance;
    private void Awake()
    {
        if(!mainCamera) mainCamera = Camera.main;

        
    }
    private void Start()
    {
        DrawCameraView();
        DrawEmptyCells();
    }

    private void OnDrawGizmos()
    {
        DrawVacantCells_Editor();
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

    private bool IsWithinBoard(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0);
    }

    public bool IsValidPosition(Shape shape)
    {
        foreach(Transform child in shape.Body)
        {
            Vector3Int intPosition = child.position.ToIntRound();
            if(!IsWithinBoard(intPosition.x, intPosition.y)) return false;
            else if (IsOccupied(intPosition.x, intPosition.y)) return false;
        }

        return true;
    }
    public void StoreShapeInGrid(Shape shape)
    {
        if (!shape) return;

        foreach(Transform child in shape.Body)
        {
            Vector3Int intPosition = child.position.ToIntRound();

            if(intPosition.x < width && intPosition.y < height)
                Grid[intPosition.x, intPosition.y] = child;
        }
    }

    private bool IsRowComplete(int y)
    {
        for(int x = 0; x< width; x++)
        {
            if (Grid[x, y] == null) return false;
        }

        return true;
    }

    private void ClearRow(int y)
    {
        for(int x = 0;x< width; x++)
        {
            if (Grid[x, y] == null) continue;

            Destroy(Grid[x, y].gameObject);
            Grid[x, y] = null;
        }
    }

    private void ShiftOneRowDown(int y)
    {
        for(int x = 0;x< width; x++)
        {
            if (Grid[x, y] == null) continue;
            Grid[x, y-1] = Grid[x, y];
            Grid[x, y] = null;
            Grid[x, y - 1].position += Vector3.down;
        }
    }

    private void ShiftRowsDown(int fromY)
    {
        for(int y = fromY; y< height; y++)
        {
            ShiftOneRowDown(y);
        }
    }

    public async UniTask CheckClearAllRows()
    {
        for(int y = 0; y< height; y++)
        {
            if (!IsRowComplete(y)) continue;

            if (!scoreSystem.InCombo) scoreSystem.StartNewCombo();
            ClearRow(y);
            scoreSystem.IncreaseComboCount();
            await UniTask.WaitForSeconds(0.1f);
            ShiftRowsDown(y + 1);
            await UniTask.WaitForSeconds(0.1f);
            y--;
        }

        if (scoreSystem.InCombo)
        {
            scoreSystem.UpdateScoreInCombo();
            scoreSystem.EndCurrentCombo();
        }
    }

    public bool IsOverLimit(Shape shape)
    {
        foreach(Transform child in shape.Body)
        {
            if (child.position.y > (height - 1)) return true;
        }

        return false;
    }

    private bool IsOccupied(int x, int y)
    {
        return y < height && Grid[x, y] != null;
    }

    private void DrawVacantCells_Editor()
    {
#if UNITY_EDITOR
        if(!Application.isPlaying) return;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if(!IsOccupied(x, y))
                {
                    Vector3 vacantPos = new Vector3(x, y, 0);
                    vacantPos.DrawBlankCell(Color.green);
                }
            }
        }
#endif
    }
}
