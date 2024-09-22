using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShapeLibrary;

public class GhostShape : MonoBehaviour
{
    [SerializeField] private ShapeLibrary shapeLibrary;
    [SerializeField] private Color color = Color.white;

    private Shape ghost;
    private bool hitBottom;

    private Dictionary<int, Shape> dictGhostsPrepare = new();
    private void OnEnable()
    {
        EventsCenter.OnSceneLoaded += PrepareShapes;
    }
    private void OnDisable()
    {
        EventsCenter.OnSceneLoaded -= PrepareShapes;
    }
    public void Draw(Shape originalShape)
    {
        if (!ghost)
        {
            EnableGhost(originalShape);
        }

        ghost.Body.position = originalShape.Body.position;
        ghost.Body.rotation = originalShape.Body.rotation;

        hitBottom = false;
        while (!hitBottom)
        {
            ghost.MoveDown();
            if (!GamePlayController.Instance.Board.IsValidPosition(ghost))
            {
                ghost.MoveUp();
                hitBottom = true;
            }
        }
    }

    public void EnableGhost(Shape originShape)
    {
        ghost = dictGhostsPrepare[originShape.ID];
        ghost.SetActive(true);
    }

    public void DisableGhost()
    {
        if (!ghost)
        {
            Debug.LogWarning("Dont have Ghost, but you try access it!");
            return;
        }
        ghost.SetActive(false);
        ghost = null;
    }

    private void PrepareShapes()
    {
        dictGhostsPrepare = new();
        foreach (ShapeConfig config in shapeLibrary.All)
        {
            var clone = SpawnNewGhostShape(config.Prefab);
            clone.SetActive(false);
            dictGhostsPrepare.TryAdd(config.Prefab.ID, clone);
        }
    }
    private Shape SpawnNewGhostShape(Shape originalShape)
    {
        Shape cloner = Instantiate(originalShape, originalShape.transform.position, Quaternion.identity);
        cloner.gameObject.name = $"GhostShape {originalShape.name}";
        cloner.transform.parent = transform;
        cloner.Initialize();
        foreach (var cell in cloner.Cells)
        {
            cell.Graphic.color = color;
        }

        return cloner;
    }
}
