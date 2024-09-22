

using UnityEngine;

public class Cell : MonoBehaviour
{
    [field: SerializeField] public SpriteRenderer Graphic { get; private set; }
    [field: SerializeField] public TrailRenderer FX { get; private set; }

    public Transform Body { get; private set; }

    private void Awake()
    {
        Body = transform;
    }

}