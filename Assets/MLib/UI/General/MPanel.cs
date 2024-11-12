using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLib
{
    public abstract class MPanel : MonoBehaviour
    {
        [SerializeField] private bool disableOnAwake = true;
        [SerializeField] private bool disableOnHide = true;

        protected Canvas canvas;
        protected CanvasGroup canvasGroup;

        public bool IsShowing => canvas.enabled;
        protected virtual void Awake()
        {
            canvas = GetComponent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();

            if (disableOnAwake)
                gameObject.SetActive(false);
        }

        [System.Serializable]
        public class Setting
        {
            public float introDuration = 0f;
            public float outroDuration = 0f;
        }
        [SerializeField] private Setting setting = new();

        public void Show()
        {
            Show(null);
        }

        public void Hide()
        {
            Hide(null);
        }

        public virtual void Show(Action onFinish)
        {
            gameObject.SetActive(true);
            if (!canvas)
            {
                Debug.LogError("My panel has to have canvas, right?");
                return;
            }
            canvas.enabled = true;
            if(canvasGroup)
            {
                canvasGroup.interactable = false;
                onFinish += () => canvasGroup.interactable = true;
            }
            this.DelayRealtimeCall(setting.introDuration, onFinish);
        }
        public virtual void Hide(Action onFinish)
        {
            if (canvasGroup) canvasGroup.interactable = false;
            this.DelayRealtimeCall(setting.outroDuration, () =>
            {
                onFinish?.Invoke();
                canvas.enabled = false;

                if(disableOnHide) 
                    gameObject.SetActive(false);
            });
        }
    }
}
