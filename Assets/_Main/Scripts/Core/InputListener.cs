

using MLib;
using System;
using UnityEngine;

public class InputListener: MSingleton<InputListener>
{
    [Header("Repeat input rate")]
    [Min(0f)] [SerializeField] private float horizontalRate = 0.2f;
    [Min(0f)] [SerializeField] private float dropFastRate = 0.1f;
    [Min(0f)] [SerializeField] private float rotateRate = 0.2f;

    public Action OnMoveRight;
    public Action OnMoveLeft;
    public Action OnRotate;
    public Action<bool> OnStatusDropFastChanged;

    private float timerHorizontal = 0f;
    private float timerRotate = 0f;
    private void Update()
    {
        timerHorizontal -= Time.deltaTime;
        timerRotate -= Time.deltaTime;
#if UNITY_EDITOR
        HandleOnPC();
#elif PLATFORM_ANDROID
        HandleOnMobile();
#endif
    }
    private void HandleOnPC()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            OnStatusDropFastChanged?.Invoke(true);
        else if(Input.GetKeyUp(KeyCode.DownArrow))
            OnStatusDropFastChanged?.Invoke(false);


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
        else if (timerRotate <= 0f && (Input.GetKey(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            timerRotate = rotateRate;
            OnRotate?.Invoke();
        }
    }

    private void HandleOnMobile()
    {
        
    }
} 