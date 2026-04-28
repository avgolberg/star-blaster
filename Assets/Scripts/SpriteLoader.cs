using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public static class SpriteLoader
{
    private static readonly Dictionary<string, Sprite> spriteCache = new();

    public static IEnumerator LoadSprite(string url, Action<Sprite> onLoaded)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            onLoaded?.Invoke(null);
            yield break;
        }

        if (spriteCache.TryGetValue(url, out Sprite cachedSprite))
        {
            onLoaded?.Invoke(cachedSprite);
            yield break;
        }

        Debug.Log($"[SpriteLoader] Requesting sprite: {url}");

        using UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning($"[SpriteLoader] FAILED: {url} | Error: {request.error}");
            onLoaded?.Invoke(null);
            yield break;
        }

        Texture2D texture = DownloadHandlerTexture.GetContent(request);

        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            100f
        );

        spriteCache[url] = sprite;

        Debug.Log($"[SpriteLoader] Loaded successfully: {url}");

        onLoaded?.Invoke(sprite);
    }

    public static bool TryGetFromCache(string url, out Sprite sprite)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            sprite = null;
            return false;
        }

        return spriteCache.TryGetValue(url, out sprite);
    }

    public static void ClearCache()
    {
        spriteCache.Clear();
    }

    public static bool IsCached(string url)
    {
        return !string.IsNullOrWhiteSpace(url) && spriteCache.ContainsKey(url);
    }
}