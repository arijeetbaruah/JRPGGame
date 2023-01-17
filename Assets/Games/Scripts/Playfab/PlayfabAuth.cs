using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EconomyModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayfabAuth
{
    public static void LoginWithEmail(string email, string password, Action<LoginResult> callback, Action<PlayFabError> errorCallback)
    {
        PlayFabClientAPI.LoginWithEmailAddress(new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password
        }, callback, errorCallback);
    }

    public static void LoginWithCustomID(string customID, Action<LoginResult> callback, Action<PlayFabError> errorCallback)
    {
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
        {
            CreateAccount = true,
            CustomId = customID
        }, callback, errorCallback);
    }

    public static void LoginWithGoogle(Action<LoginResult> callback, Action<PlayFabError> errorCallback)
    {
        PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest
        {
            CreateAccount = true
        }, callback, errorCallback);
    }

    public static void LoginWithFacebook(Action<LoginResult> callback, Action<PlayFabError> errorCallback)
    {
        PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest
        {
            CreateAccount = true
        }, callback, errorCallback);
    }
}
