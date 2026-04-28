using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarBlasterAssetPreloader : MonoBehaviour
{
    [SerializeField] private Button startButton;

    private void OnEnable()
    {
        if (StarBlasterConfigLoader.Instance != null)
        {
            StarBlasterConfigLoader.Instance.OnConfigLoaded += HandleConfigLoaded;
        }
    }

    private void OnDisable()
    {
        if (StarBlasterConfigLoader.Instance != null)
        {
            StarBlasterConfigLoader.Instance.OnConfigLoaded -= HandleConfigLoaded;
        }
    }

    private void Start()
    {
        Debug.Log("[AssetPreloader] Start");

        if (startButton != null)
            startButton.interactable = false;

        if (StarBlasterConfigLoader.Instance == null)
        {
            Debug.LogWarning("[AssetPreloader] No ConfigLoader instance");
            return;
        }

        if (StarBlasterConfigLoader.Instance.Options == null)
        {
            Debug.LogWarning("[AssetPreloader] Options are null");
            return;
        }

        Debug.Log("[AssetPreloader] Starting preload from Start");

        StartCoroutine(WaitForConfigAndPreload());
    }

    private IEnumerator WaitForConfigAndPreload()
    {
        yield return null;

        if (StarBlasterConfigLoader.Instance?.Options != null)
        {
            Debug.Log("[AssetPreloader] Starting preload after one frame");
            yield return PreloadAssets(StarBlasterConfigLoader.Instance.Options);
        }
        else
        {
            Debug.LogWarning("[AssetPreloader] Config not ready after one frame");
        }
    }

    private void HandleConfigLoaded(StarBlasterOptions options)
    {
        Debug.Log("[AssetPreloader] Config loaded event received");
        StartCoroutine(PreloadAssets(options));
    }

    private IEnumerator PreloadAssets(StarBlasterOptions options)
    {
        Debug.Log("[AssetPreloader] PreloadAssets called");

        if (options?.images == null)
        {
            Debug.LogWarning("[AssetPreloader] Images config is null");
            EnableStart();
            yield break;
        }

        Debug.Log($"[AssetPreloader] Player image URL: {options.images.playerImage}");
        Debug.Log($"[AssetPreloader] Basic enemy URL: {options.images.basicEnemyImage}");

        yield return SpriteLoader.LoadSprite(options.images.backgroundImage, null);
        yield return SpriteLoader.LoadSprite(options.images.backgroundTopLayerImage, null);
        yield return SpriteLoader.LoadSprite(options.images.playerImage, null);
        yield return SpriteLoader.LoadSprite(options.images.basicEnemyImage, null);
        yield return SpriteLoader.LoadSprite(options.images.fastEnemyImage, null);
        yield return SpriteLoader.LoadSprite(options.images.tankEnemyImage, null);
        yield return SpriteLoader.LoadSprite(options.images.playerProjectileImage, null);
        yield return SpriteLoader.LoadSprite(options.images.enemyProjectileImage, null);

        Debug.Log("[AssetPreloader] Preload completed");
        EnableStart();
    }

    private void EnableStart()
    {
        if (startButton != null)
            startButton.interactable = true;
    }
}