using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EconomyModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayfabAuth
{
    public static void Login(string email, string password, Action<GetItemResponse> callback, Action<PlayFabError> errorCallback)
    {
        PlayFabClientAPI.LoginWithEmailAddress(new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password
        }, itemResponse =>
        {
            Debug.Log(itemResponse);
            callback?.Invoke(itemResponse);
        }, error =>
        {
            Debug.LogError(error.ErrorMessage);
            errorCallback?.Invoke(error);
        }});
    }
}
