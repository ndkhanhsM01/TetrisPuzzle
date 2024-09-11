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
    private void Awake()
    {
        dictGhostsPrepare = new();
        foreach(ShapeConfig config in shapeLibrary.All)
        {
            var clone = SpawnNewGhostShape(config.Prefab);
            clone.SetActive(false);
            dictGhostsPrepare.TryAdd(config.Prefab.ID, clone);
        }
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
        ghost.SetActive(false);
        ghost = null;
    }

    private Shape SpawnNewGhostShape(Shape originalShape)
    {
        Shape cloner = Instantiate(originalShape, originalShape.transform.position, Quaternion.identity);
        cloner.gameObject.name = $"GhostShape {originalShape.name}";
        cloner.transform.parent = transform;

        foreach (var renderer in cloner.AllRenderers)
        {
            renderer.color = color;
        }

        return cloner;
    }
}
