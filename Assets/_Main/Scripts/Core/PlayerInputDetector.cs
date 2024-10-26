

using MLib;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInputDetector: MonoBehaviour
{
    #region General input setting
    [Header("General input setting")]
    [Min(0f)][SerializeField] private float rotateRate = 0.2f;
    #endregion

    #region Mobile input setting
    [Space(20)]
    [Header("Mobile input setting")]
    [SerializeField] private bool testInputMobile = false;
    [SerializeField] private bool debugTouch = true;
    [SerializeField] private float thresholdTouch = 1f;
    [SerializeField] private float tapThreshold = 0.15f;
    [SerializeField] private float dragThresholdHorizontal = 2f;
    [SerializeField] private float dragThreshollVertical = 3f;
    [SerializeField] private float timeThresholdVertical = 0.2f;
    [SerializeField] private float speedTouchForDrop = 10f;
    private float addPercentSensitivity => DataManager.Instance.LocalData.touchSensitivity;

    [Tooltip("min distance to enable horizontal swipe")]
    [Min(0f)][SerializeField] private float horizontalStep = 0.2f;

    private Vector2 lastPointInvokeHorizontal;
    private float touchingTime = 0f;
    private bool isFallingFast = false;
    #endregion

    #region PC input setting
    [Space(20)]
    [Header("PC input setting")]
    [Min(0f)][SerializeField] private float timeHorizontalRate = 0.15f;
    #endregion

    public Action OnDrop;
    public Action OnMoveRight;
    public Action OnMoveLeft;
    public Action<bool> OnDetectFallingFast;
    public Action OnRotate;

    private float timerHorizontalPC = 0f;
    private float timerRotate = 0f;
    private GamePlayController gamePlayController => GamePlayController.Instance;

    public bool IsActive { get; set; } = true;
    private void OnEnable()
    {
        OnMoveRight += MoveRight;
        OnMoveLeft += MoveLeft;
        OnDrop += DropImmidiate;
        OnRotate += RotateShape;
        OnDetectFallingFast += DetectHoldFallingFast;
    }
    private void OnDisable()
    {
        OnMoveRight -= MoveRight;
        OnMoveLeft -= MoveLeft;
        OnDrop -= DropImmidiate;
        OnRotate -= RotateShape;
        OnDetectFallingFast -= DetectHoldFallingFast;
    }
    #region Debug events
    private void DetectHoldFallingFast(bool obj)
    {
        //Debug.Log("Falling: " + obj.ToString());
    }

    private void RotateShape()
    {
        //Debug.Log("Rotate");
    }

    private void DropImmidiate()
    {
        //Debug.Log("Drop");
    }

    private void MoveLeft()
    {
        //Debug.Log("Move Left");
    }

    private void MoveRight()
    {
        //Debug.Log("Move Right");
    }
    #endregion

    private void Update()
    {
        if (gamePlayController.IsGamePause || gamePlayController.IsEndGame) return;

        if (IsActive)
        {
            timerRotate -= Time.deltaTime;

#if UNITY_EDITOR
            timerHorizontalPC -= Time.deltaTime;
            if (testInputMobile)
            {
                HandleInputOnMobile();
                return;
            }
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


        if (timerHorizontalPC <= 0f && (Input.GetKey(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            timerHorizontalPC = timeHorizontalRate;
            OnMoveRight?.Invoke();
        }
        else if (timerHorizontalPC <= 0f && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            timerHorizontalPC = timeHorizontalRate;
            OnMoveLeft?.Invoke();
        }
        else if (timerRotate <= 0f && (Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space)))
        {
            timerRotate = rotateRate;
            OnRotate?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnDrop?.Invoke();
        }
    }
    
    public void ResetTouch()
    {
        touchingTime = 0;
        StopFallingFast();
    }

    private void HandleInputOnMobile()
    {
        if (Input.touchCount <= 0)
        {
            return;
        }

        // get touch
        Touch newTouch = Input.GetTouch(0);
        Vector2 step = newTouch.deltaPosition;
        step.x += step.x * addPercentSensitivity;
        step.y += step.y * addPercentSensitivity;

        // handle on touch began
        if (newTouch.phase == TouchPhase.Began)
        {
            lastPointInvokeHorizontal = newTouch.position;
            touchingTime = 0f;
        }

        // handle touch to invoke input
        SwipeAxis swipeAxis = DetectSwipeAxis(step);
        touchingTime += Time.deltaTime;
        if (swipeAxis == SwipeAxis.None)
        {
/*            if (newTouch.phase == TouchPhase.Ended)
            {
                Debug.Log(touchingTime);
                Debug.Log(newTouch.deltaPosition);
            }*/

            if (touchingTime < tapThreshold && newTouch.phase == TouchPhase.Ended && newTouch.deltaPosition == Vector2.zero)
            {
                //Debug.Log("Try rotate");
                timerRotate = rotateRate;
                OnRotate?.Invoke();
            }
        }
        else if(swipeAxis == SwipeAxis.Horizontal)
        {
            StopFallingFast();
            float offsetLastHorizontal = Mathf.Abs(newTouch.position.x - lastPointInvokeHorizontal.x);
            offsetLastHorizontal += offsetLastHorizontal * addPercentSensitivity;
            if (step.x > 0 && offsetLastHorizontal >= horizontalStep)
            {
                lastPointInvokeHorizontal = newTouch.position;
                OnMoveRight?.Invoke();
            }
            else if (step.x < 0 && offsetLastHorizontal >= horizontalStep)
            {
                lastPointInvokeHorizontal = newTouch.position;
                OnMoveLeft?.Invoke();
            }
        }
        else if(swipeAxis == SwipeAxis.Vertical && touchingTime > timeThresholdVertical)
        {
            float speedTouch = step.magnitude / newTouch.deltaTime;
            //if(debugTouch) Debug.Log("Speed touch: " + speedTouch);
            if (step.y < 0f && speedTouch > speedTouchForDrop && !isFallingFast)
            {
                OnDrop?.Invoke();
                newTouch.phase = TouchPhase.Ended;
            }
            else if(step.y < 0f && !isFallingFast)
            {
                StartFallingFast();
            }
        }

        // handle on touch end
        if (newTouch.phase == TouchPhase.Ended)
        {
            if (isFallingFast)
            {
                StopFallingFast();
            }
        }
    }
    private SwipeAxis DetectSwipeAxis(Vector2 step)
    {
        float absX = Mathf.Abs(step.x);
        float absY = Mathf.Abs(step.y);
        if (absX < thresholdTouch || step == Vector2.zero) return SwipeAxis.None;

        if(absX > absY && absX > dragThresholdHorizontal) return SwipeAxis.Horizontal;
        else if (absX < absY && absY > dragThreshollVertical) return SwipeAxis.Vertical;
        else return SwipeAxis.None;
    }

    private void StopFallingFast()
    {
        OnDetectFallingFast?.Invoke(false);
        isFallingFast = false;
    }
    private void StartFallingFast()
    {
        OnDetectFallingFast?.Invoke(true);
        isFallingFast = true;
    }
    public enum SwipeAxis { None, Horizontal, Vertical } 
}