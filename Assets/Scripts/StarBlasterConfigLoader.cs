using UnityEngine;

public class StarBlasterConfigLoader : MonoBehaviour
{
    public static StarBlasterConfigLoader Instance { get; private set; }

    [Header("Local testing")]
    [SerializeField] private TextAsset testJson;
    [SerializeField] private bool loadTestJsonOnStart = true;

    public StarBlasterOptions Options { get; private set; } = new();

    public event System.Action<StarBlasterOptions> OnConfigLoaded;

    [SerializeField] private bool debugConfigLogs = true;

    public bool DebugConfigLogs => debugConfigLogs; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (loadTestJsonOnStart && testJson != null)
        {
            LoadFromJson(testJson.text);
        }
        else
        {
            ApplyDefaultConfig();
        }
    }

    public void LoadFromJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            Debug.LogWarning("StarBlasterConfigLoader: JSON is empty. Using default config.");
            ApplyDefaultConfig();
            return;
        }

        try
        {
            StarBlasterConfigRoot root = JsonUtility.FromJson<StarBlasterConfigRoot>(json);

            if (root == null || root.starBlasterOptions == null)
            {
                Debug.LogWarning("StarBlasterConfigLoader: Invalid JSON structure. Using default config.");
                ApplyDefaultConfig();
                return;
            }

            Options = root.starBlasterOptions;
            EnsureDefaults();

            Debug.Log("StarBlasterConfigLoader: Config loaded successfully.");
            OnConfigLoaded?.Invoke(Options);
        }
        catch (System.Exception exception)
        {
            Debug.LogError($"StarBlasterConfigLoader: Failed to parse JSON. {exception.Message}");
            ApplyDefaultConfig();
        }
    }

    public void ApplyDefaultConfig()
    {
        Options = new StarBlasterOptions();
        SpriteLoader.ClearCache();
        OnConfigLoaded?.Invoke(Options);
    }

    private void EnsureDefaults()
    {
        Options.images ??= new ImagesConfig();
        Options.colors ??= new ColorsConfig();
        Options.soundUrls ??= new SoundUrlsConfig();
        Options.texts ??= new TextsConfig();
        Options.background ??= new BackgroundConfig();
        Options.effects ??= new EffectsConfig();
        Options.gameplay ??= new GameplayConfig();

        Options.gameplay.player ??= new PlayerConfig();
        Options.gameplay.enemies ??= new EnemyConfig();
        Options.gameplay.projectiles ??= new ProjectileConfig();
        Options.gameplay.score ??= new ScoreConfig();

        if (string.IsNullOrWhiteSpace(Options.difficultyPreset))
            Options.difficultyPreset = "medium";
    }

    public void ReceiveConfigFromWiply(string json)
    {
        LoadFromJson(json);
    }
}