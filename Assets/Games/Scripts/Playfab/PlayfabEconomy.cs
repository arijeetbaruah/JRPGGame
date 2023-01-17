using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.EconomyModels;
using PlayFab;
using System;

public static class PlayfabEconomy
{
    public static void GetItemInfo(string itemID, Action<GetItemResponse> callback, Action<PlayFabError> errorCallback)
    {
        GetItemRequest getItemRequest = new GetItemRequest
        {
            Id = itemID
        };
        PlayFabEconomyAPI.GetItem(getItemRequest, itemResponse =>
        {
            Debug.Log(itemResponse);
            callback?.Invoke(itemResponse);
        }, error =>
        {
            Debug.LogError(error.ErrorMessage);
            errorCallback?.Invoke(error);
        });
    }
}
