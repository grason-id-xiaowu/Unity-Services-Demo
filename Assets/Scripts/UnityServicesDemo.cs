using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
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

        //身份验证登录
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
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
    }
}
