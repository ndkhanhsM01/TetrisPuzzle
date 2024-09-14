using MLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public int ID;
    [SerializeField] private bool canRotate = true;

    public Transform Body { get; private set; }

    [field: SerializeField] public SpriteRenderer[] AllRenderers { get; private set; }

    private bool activeTrail = false;
    private TrailRenderer[] allTrails;
    private void Awake()
    {
        Body = transform;
        allTrails = GetComponentsInChildren<TrailRenderer>();
        DisableTrail();
    }
    private void Move(Vector3 direction)
    {
        Body.position += direction;
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
        foreach (TrailRenderer trail in allTrails)
        {
            if (trail) trail.enabled = true;
        }
    }
    public void DisableTrail()
    {
        foreach (TrailRenderer trail in allTrails)
        {
            if (trail) trail.enabled = false;
        }
    }
}
