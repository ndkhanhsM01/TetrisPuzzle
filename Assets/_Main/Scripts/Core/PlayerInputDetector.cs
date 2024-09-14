

using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInputDetector: MonoBehaviour
{
    [Header("Repeat input rate")]
    [Min(0f)][SerializeField] private float horizontalRate = 0.2f;
    [Min(0f)][SerializeField] private float rotateRate = 0.2f;

    public Action OnDrop;
    public Action OnMoveRight;
    public Action OnMoveLeft;
    public Action<bool> OnDetectFallingFast;
    public Action OnRotate;

    private float timerHorizontal = 0f;
    private float timerRotate = 0f;
    private GamePlayController gamePlayController => GamePlayController.Instance;

    public bool IsActive { get; set; } = true;

    private void Update()
    {
        if (gamePlayController.IsGamePause || gamePlayController.IsEndGame) return;

        if (IsActive)
        {
            timerHorizontal -= Time.deltaTime;
            timerRotate -= Time.deltaTime;
#if UNITY_EDITOR
            HandleInputOnPC();
#elif PLATFORM_ANDROID
        HandleInputOnMobile();
#endif
        }
    }

    private void HandleInputOnPC()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            OnDetectFallingFast?.Invoke(true);
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            OnDetectFallingFast?.Invoke(false);


        if (timerHorizontal <= 0f && (Input.GetKey(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            timerHorizontal = horizontalRate;
            OnMoveRight?.Invoke();
        }
        else if (timerHorizontal <= 0f && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            timerHorizontal = horizontalRate;
            OnMoveLeft?.Invoke();
        }
        else if (timerRotate <= 0f && (Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space)))
        {
            timerRotate = horizontalRate;
            OnRotate?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnDrop?.Invoke();
        }
    }

    private void HandleInputOnMobile()
    {
        if (Input.touchCount <= 0) return;


    }
}