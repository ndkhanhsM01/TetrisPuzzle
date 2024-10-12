using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public abstract class GlobalData<T>: MonoBehaviour, IGlobalData where T : class
{
    [SerializeField] protected bool loadOnStart = true;
    [SerializeField] private float intervalLoad = 2f;

    protected bool isLoadSuccess = false;
    private bool isLoadContinuously = true;
    protected T data;
    protected GlobalDataManager manager;

    public bool IsLoadContinuously { get => isLoadContinuously; set => isLoadContinuously = value; }
    public T Data { get => data; set => data = value; }

    private UniTask taskDelayLoad;
    protected virtual void Awake()
    {
        UniTask taskDelayLoad = UniTask.WaitForSeconds(intervalLoad);

        RegisterWithManger();
    }
/*    protected virtual void Start()
    {
        if (loadOnStart)
        {
            LoadContinuously();
        }
    }*/

    #region IGlobalData
    public async void RegisterWithManger()
    {
        await UniTask.WaitUntil(() => GlobalDataManager.Instance != null);
        GlobalDataManager.Instance.Register(this);
        manager = GlobalDataManager.Instance;
    }
    public async void LoadContinuously()
    {
        while (IsLoadContinuously && !isLoadSuccess)
        {
            Load();
            await taskDelayLoad;
        }
    }
    public abstract void Load();
    #endregion
}
