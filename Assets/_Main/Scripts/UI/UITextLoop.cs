using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;


public class UITextLoop: MonoBehaviour
{
    [SerializeField] private TMP_Text target;
    [SerializeField] private float interval = 0.2f;
    [SerializeField] private string[] contents;

    private int index;

    private void OnEnable()
    {
        Task_LoopContents().Forget();
    }

    private async UniTask Task_LoopContents()
    {
        while (gameObject.activeInHierarchy)
        {
            index = (index + 1) % contents.Length;
            target.text = contents[index];

            await UniTask.WaitForSeconds(interval);
        }
    }
}