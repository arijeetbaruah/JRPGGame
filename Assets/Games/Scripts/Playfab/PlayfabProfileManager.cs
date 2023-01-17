using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using UnityEngine.TextCore.Text;

public static class PlayfabProfileManager
{
    public static void GetProfile(Action<GetPlayerProfileResult> callback, Action<PlayFabError> error)
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest(), callback, error);
    }

    public static void UpdateDisplayName(string displayName, Action<UpdateUserTitleDisplayNameResult> callback, Action<PlayFabError> error)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = displayName
        }, callback, error);
    }

    public static void GetCharacterStats(string characterID, Action<GetCharacterDataResult> callback, Action<PlayFabError> error)
    {
        PlayFabClientAPI.GetCharacterData(new GetCharacterDataRequest
        {
            CharacterId = characterID,
            Keys = new List<string>
            {
                "StatsBlock"
            }
        }, callback, error);
    }

    public static void UpdateCharacterStats(string characterID, StatsBlock stats, Action<UpdateCharacterDataResult> callback, Action<PlayFabError> error)
    {
        PlayFabClientAPI.UpdateCharacterData(new UpdateCharacterDataRequest
        {
            CharacterId = characterID,
            Data = new Dictionary<string, string>
            {
                { "StatsBlock", JsonConvert.SerializeObject(stats) }
            }
        }, callback, error);
    }
}
