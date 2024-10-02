using MLib;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AudioButton: MonoBehaviour
{
    [SerializeField] private Button button;


    private void Reset()
    {
        button = GetComponent<Button>();
    }

    private void Awake()
    {
        if(!button) button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.AddListener(OnClick);
    }
    private void OnDisable()
    {
        button.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        if (MAudioManager.Instance)
        {
            MAudioManager.Instance.PlaySFX(MSoundType.ClickButton);
        }
    }
}
