using MLib;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDataManager: MSingleton<GlobalDataManager>
{
    [SerializeField] private HttpCaller httpCaller;

    private Dictionary<string, IGlobalData> dictDatas;

    public HttpCaller HttpCaller => httpCaller;
    protected override void Awake()
    {
        base.Awake();

    }

    public void Register<T>(GlobalData<T> member) where T : class
    {
        if (dictDatas == null) dictDatas = new();

        bool adddSuccess = dictDatas.TryAdd(nameof(T), member);
        if (!adddSuccess)
        {
            Debug.LogError($"Can not have same type {nameof(T)} in global data");
        }
    }

    public T GetData<T>() where T : GlobalData<T>
    {
        if(dictDatas.TryGetValue(nameof(T), out IGlobalData data))
        {
            return data as T;
        }
        else
        {
            Debug.LogError($"Not found data type of <{nameof(T)}> in dictionary");
            return null;
        }
    }
}
