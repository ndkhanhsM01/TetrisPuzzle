using System;

public interface IGlobalData
{
    void RegisterWithManger();
    public void Load();
    public void LoadContinuously();
}