using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[GlobalConfig("Assets/Games/Resources/Config")]
public class PlayableCharacterDatabase : GlobalConfig<PlayableCharacterDatabase>
{
    public PlayableCharacterDatabaseDictionary characters;
}

[Serializable]
public class PlayableCharacterDatabaseDictionary : UnitySerializedDictionary<string, CharacterSO> { }
