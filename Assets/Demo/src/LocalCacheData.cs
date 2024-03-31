using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class LocalUserIdToken
{
    public string UserId;
    public string Token;
}


[Serializable]
public class LocalCacheData
{
    public List<LocalUserIdToken> LocalUserTokens;
    public LocalCacheData()
    {
        LocalUserTokens = new List<LocalUserIdToken>();
    }
}
