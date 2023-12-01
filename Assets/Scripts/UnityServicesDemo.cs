using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudCode;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;

public class UnityServicesDemo : MonoBehaviour
{
    private async void Awake()
    {
        try
        {
            var options = new InitializationOptions();
            options.SetProfile("test_profiles");

            await UnityServices.InitializeAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        //身份验证匿名异步登录
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        /*try
        {
            string result = await CloudCodeService.Instance.CallEndpointAsync("ExampleModule",  new Dictionary<string, object> { {"name"," World"} });
        }
        catch(CloudCodeException exception)
        {
            Debug.LogException(exception);
        }*/
    }

    private void Start()
    {
        AddSignCallBack();
    }


    /// <summary>
    /// 添加登录响应事件
    /// </summary>
    void AddSignCallBack()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

            DoSomething();
        };

        AuthenticationService.Instance.SignInFailed += (err) =>
        {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };
    }

    void DoSomething()
    {
        Debug.Log(AuthenticationService.Instance.Profile);

        Debug.Log($"Is SignedIn: {AuthenticationService.Instance.IsSignedIn}");
        Debug.Log($"Is Authorized: {AuthenticationService.Instance.IsAuthorized}");
        Debug.Log($"Is Expired: {AuthenticationService.Instance.IsExpired}");
        Debug.Log("A");
        SaveDataExample();
        Debug.Log("B");
        LoadDataExample();
        Debug.Log("C");
        RetrieveKeys();
        Debug.Log("D");

    }

    /// <summary>
    /// 保存游戏数据示例
    /// </summary>
    private async void SaveDataExample()
    {
        var data = new Dictionary<string, object> { { "key", "someValue" },{ "key_01","test_value"} };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
    }
    /// <summary>
    /// 加载游戏数据示例
    /// </summary>
    private async void LoadDataExample()
    {
        Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "key" });
        Debug.Log("Done: " + savedData["key"]);
    }
    /// <summary>
    /// 删除数据
    /// </summary>
    private async void DeleteDataExample()
    {
        await CloudSaveService.Instance.Data.ForceDeleteAsync("key");
    }
    /// <summary>
    /// 获取密钥和元数据的列表
    /// </summary>
    private async void RetrieveKeys()
    {
        List<string> keys = await CloudSaveService.Instance.Data.RetrieveAllKeysAsync();

        foreach(string key in keys)
        {
            Debug.Log("key----:"+ key);
        }
    }

    /*public async void GetPlayerFileAsByteArray()
    {
        byte[] file = await CloudSaveService.Instance.Files.Player.LoadBytesAsync("fileName.csv");
    }*/
}
