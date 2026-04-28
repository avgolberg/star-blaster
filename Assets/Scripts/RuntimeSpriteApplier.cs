using UnityEngine;

public enum StarBlasterSpriteType
{
    Player, BasicEnemy, FastEnemy, TankEnemy, PlayerProjectile, EnemyProjectile
}

public class RuntimeSpriteApplier : MonoBehaviour
{
    [SerializeField] private StarBlasterSpriteType spriteType;
    private SpriteRenderer targetRenderer;
    private bool spriteApplied;

    private void Awake()
    {
        targetRenderer = GetComponentInChildren<SpriteRenderer>();
    }

     private void OnEnable()
    {
        if (StarBlasterConfigLoader.Instance != null)
        {
            StarBlasterConfigLoader.Instance.OnConfigLoaded += OnConfigLoaded;
        }

        TryApplySprite();
    }


    private void Start()
    {
        TryApplySprite();
    }

     private void OnDisable()
    {
        if (StarBlasterConfigLoader.Instance != null)
        {
            StarBlasterConfigLoader.Instance.OnConfigLoaded -= OnConfigLoaded;
        }
    }

     private void OnConfigLoaded(StarBlasterOptions options)
    {
        spriteApplied = false;
        TryApplySprite();
    }

    private void TryApplySprite()
    {
        if (spriteApplied)
            return;

        if (StarBlasterConfigLoader.Instance == null)
        {
            Debug.LogWarning($"[RuntimeSpriteApplier] No loader on {gameObject.name}");
            return;
        }

        StarBlasterOptions options = StarBlasterConfigLoader.Instance.Options;

        if (options?.images == null)
        {
            Debug.LogWarning($"[RuntimeSpriteApplier] No images config on {gameObject.name}");
            return;
        }

        if (targetRenderer == null)
        {
            targetRenderer = GetComponentInChildren<SpriteRenderer>(true);
        }

        if (targetRenderer == null)
        {
            Debug.LogWarning($"[RuntimeSpriteApplier] No SpriteRenderer on {gameObject.name}");
            return;
        }

        string url = GetUrl(options.images);

        if (string.IsNullOrWhiteSpace(url))
            return;
        else
            Debug.Log($"[RuntimeSpriteApplier] {gameObject.name} type={spriteType}, url={url}");

        spriteApplied = true;

        if (SpriteLoader.TryGetFromCache(url, out var sprite))
        {
            ApplySpriteWithScale(sprite);
        }
        else
        {
            Debug.LogWarning($"[RuntimeSpriteApplier] Sprite not in cache, loading fallback: {url}");

            StartCoroutine(SpriteLoader.LoadSprite(url, loadedSprite =>
            {
                if (loadedSprite != null)
                    ApplySpriteWithScale(loadedSprite);
            }));
        }
    }

    private void ApplySpriteWithScale(Sprite newSprite)
    {
        if (targetRenderer == null || newSprite == null)
            return;

        Vector2 oldSize = targetRenderer.bounds.size;

        targetRenderer.sprite = newSprite;

        Vector2 newSize = targetRenderer.bounds.size;

        if (oldSize.x <= 0 || oldSize.y <= 0 || newSize.x <= 0 || newSize.y <= 0)
            return;

        float scaleFactor = Mathf.Min(
            oldSize.x / newSize.x,
            oldSize.y / newSize.y
        );

        targetRenderer.transform.localScale *= scaleFactor;
    }

    private string GetUrl(ImagesConfig images)
    {
        return spriteType switch
        {
            StarBlasterSpriteType.Player => images.playerImage,
            StarBlasterSpriteType.BasicEnemy => images.basicEnemyImage,
            StarBlasterSpriteType.FastEnemy => images.fastEnemyImage,
            StarBlasterSpriteType.TankEnemy => images.tankEnemyImage,
            StarBlasterSpriteType.PlayerProjectile => images.playerProjectileImage,
            StarBlasterSpriteType.EnemyProjectile => images.enemyProjectileImage,
            _ => ""
        };
    }
}
