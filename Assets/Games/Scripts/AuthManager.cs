using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum AuthMethod
{
    None,
    Token,
    Google,
    Facebook
}

public class AuthManager : MonoBehaviour
{
    public TextAsset jsonFile;
    public RectTransform loadingPanel;

    private void Start()
    {
        AuthMethod authMethod = Enum.Parse<AuthMethod>(PlayerPrefs.GetString("authMethod", AuthMethod.None.ToString()));
        LoginOnStart(authMethod);
    }

    private void LoginOnStart(AuthMethod method)
    {
        loadingPanel.gameObject.SetActive(true);
        switch (method)
        {
            case AuthMethod.Token:
                string token = PlayerPrefs.GetString("authToken");
                PlayfabAuth.LoginWithCustomID(token, OnLoginCallback, OnError);
                break;
            case AuthMethod.Google:
                PlayfabAuth.LoginWithGoogle(OnLoginCallback, OnError);
                break;
            default:
                loadingPanel.gameObject.SetActive(false);
                break;
        }
    }

    public void LoginAsGuest()
    {
        string token = System.Guid.NewGuid().ToString();

        PlayerPrefs.SetString("authToken", token);
        PlayerPrefs.SetString("authMethod", AuthMethod.Token.ToString());
        PlayfabAuth.LoginWithCustomID(token, OnLoginCallback, OnError);
    }

    private void OnError(PlayFabError obj)
    {
        
    }

    private string GetRandomName()
    {
        List<string> names = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonFile.text)["names"];
        int index = UnityEngine.Random.Range(0, names.Count);
        return names[index];
    }

    private void OnLoginCallback(LoginResult obj)
    {
        Debug.Log($"Suceessfully Logged in : {obj.PlayFabId}");

        PlayfabProfileManager.GetProfile(result =>
        {
            CheckAndGenerateDisplayName(result.PlayerProfile.DisplayName);
            string characterID = PlayerPrefs.GetString("Current Character");
            if (string.IsNullOrEmpty(characterID) )
            {
                GrantFirstCharacter();
            }
            else
            {
                GetCharacterStats(characterID, null);
            }
        }, error => {
            Debug.LogError(error.ErrorMessage);
        });
    }

    private void GetCharacterStats(string characterID, Action<StatsBlock> callback)
    {
        PlayfabProfileManager.GetCharacterStats(characterID, character =>
        {
            if (!character.Data.ContainsKey("StatsBlock"))
            {
                if (PlayableCharacterDatabase.Instance.characters.TryGetValue("paladin", out CharacterSO characterSO))
                {
                    PlayfabProfileManager.UpdateCharacterStats(characterID, characterSO.stats, result =>
                    {
                        Debug.Log("Successful Update");
                        callback?.Invoke(characterSO.stats);
                    }, error =>
                    {
                        Debug.LogError(error.ErrorMessage);
                    });
                }
            }
        }, error =>
        {
            Debug.LogError(error.ErrorMessage);
        });
    }

    private void GrantFirstCharacter()
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            ItemId = "paladin",
            CharacterName = "Paladin"
        }, character =>
        {
            PlayerPrefs.SetString("Current Character", character.CharacterId);
        }, error =>
        {
            Debug.LogError(error.ErrorMessage);
        });
    }

    private void CheckAndGenerateDisplayName(string displayName)
    {
        if (string.IsNullOrEmpty(displayName))
        {
            displayName = GetRandomName();
            PlayfabProfileManager.UpdateDisplayName(displayName, resultUpdate =>
            {
                Debug.Log($"Suceessfully Updated : {displayName}");
                SceneManager.LoadSceneAsync(1);
            }, error =>
            {
                Debug.LogError(error);
            });
        }
        else
        {
            Debug.Log($"Welcome {displayName}");
            SceneManager.LoadSceneAsync(1);
        }
    }
}
