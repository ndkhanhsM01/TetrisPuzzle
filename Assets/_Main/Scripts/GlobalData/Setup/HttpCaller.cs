using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UnityEngine;
using MLib;

public class HttpCaller: MonoBehaviour
{
    private const string baseAddress = "http://ndkm01.somee.com/";

    #region Base
    private async Task<T> Get<T>(string api, Dictionary<string, object> queryParams = null, 
                            Action<T> onSuccess = null, Action onFailure = null) where T : class
    {
        try
        {
            using (var client = new HttpClient())
            {
                UriBuilder endPointBuilder = new UriBuilder(baseAddress + api);
                if (queryParams != null && queryParams.Count > 0)
                {
                    var query = HttpUtility.ParseQueryString(endPointBuilder.Query);
                    foreach (var param in queryParams)
                    {
                        query[param.Key] = param.Value.ToString();
                    }
                    endPointBuilder.Query = query.ToString();
                }

                Uri endPoint = endPointBuilder.Uri;
                HttpResponseMessage respone = await client.GetAsync(endPoint);

                if (respone.IsSuccessStatusCode)
                {
                    var json = await respone.Content.ReadAsStringAsync();

                    T result = JsonConvert.DeserializeObject<T>(json);
                    onSuccess?.Invoke(result);
                    Debug.Log($"Success: {api} \n{json}");
                    return result;
                }

            }

            Debug.LogError($"Failed to Get data from <{api}>");
            onFailure?.Invoke();
            return null;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception occurred while get data: {ex.Message}");
            onFailure?.Invoke();
            return null;
        }
    }
    private async Task Post<T>(string api, T data, Dictionary<string, object> queryParams = null, 
                            Action<T> onSuccess = null, Action onFailure = null) 
                            where T : class
    {
        try
        {
            using (var client = new HttpClient())
            {
                UriBuilder endPointBuilder = new UriBuilder(baseAddress + api);
                if (queryParams != null && queryParams.Count > 0)
                {
                    var query = HttpUtility.ParseQueryString(endPointBuilder.Query);
                    foreach (var param in queryParams)
                    {
                        query[param.Key] = param.Value.ToString();
                    }
                    endPointBuilder.Query = query.ToString();
                }

                Uri endPoint = endPointBuilder.Uri;
                StringContent req = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage respone = await client.PostAsync(endPoint, req);
                if (respone.IsSuccessStatusCode)
                {
                    var json = await respone.Content.ReadAsStringAsync();
                    T resData = JsonConvert.DeserializeObject<T>(json);
                    onSuccess?.Invoke(resData);
                    Debug.Log($"Success: {api} \n{json}");
                    return;
                }

            }

            Debug.LogError($"Failed to Post data from <{api}>");
            onFailure?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception occurred while posting data: {ex.Message}");
            onFailure?.Invoke();
        }
    }
    #endregion

    [MButton]
    private void TestCreateUser()
    {
        Post_CreateNewUser("hehe",
                            onSuccess: (res) => { Debug.LogWarning($"Success Regist new user <{res.SeqID}>!!"); });
    }
    public void Post_CreateNewUser(string userName, Action<User_Respone> onSuccess = null, Action onFailure = null)
    {
        string api = "api/TetrisUser/CreateNewUser";
        Dictionary<string, object> queryParams = new();
        queryParams.Add("username", userName);

        var result = Post<User_Respone>(api, null, queryParams, onSuccess, onFailure);
    }

    public void Post_UpdateScore(int userId, int newScore, Action<UserRanking_Respone> onSuccess = null, Action onFailure = null)
    {
        string api = "api/TetrisUser/UpdateScore";
        Dictionary<string, object> queryParams = new();
        queryParams.Add("userId", userId);
        queryParams.Add("newScore", newScore);

        var result = Post<UserRanking_Respone>(api, null, queryParams, onSuccess, onFailure);
    }
    public void Post_UpdateName(int userId, string newName, Action<User_Respone> onSuccess = null, Action onFailure = null)
    {
        string api = "api/TetrisUser/UpdateName";
        Dictionary<string, object> queryParams = new();
        queryParams.Add("userId", userId);
        queryParams.Add("newName", newName);

        var result = Post<User_Respone>(api, null, queryParams, onSuccess, onFailure);
    }


    public void Get_GetRankingResult(int userId, Action<UserRanking_Respone> onSuccess = null, Action onFailure = null)
    {
        string api = "api/TetrisUser/GetRankingResult";
        Dictionary<string, object> queryParams = new();
        queryParams.Add("userId", userId);

        var result = Get<UserRanking_Respone>(api, queryParams, onSuccess, onFailure);
    }
}