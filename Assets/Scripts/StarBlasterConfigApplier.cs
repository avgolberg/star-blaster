using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StarBlasterConfigApplier : MonoBehaviour
{
    [Header("Core")]
    [SerializeField] private StarBlasterConfigLoader configLoader;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer backgroundRenderer;
    [SerializeField] private SpriteRenderer backgroundTopLayerRenderer;

    [Header("Gameplay Components")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Shooter playerShooter;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private AudioManager audioManager;

    [Header("UI Texts")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI subtitleText;
    [SerializeField] private TextMeshProUGUI scoreLabelText;

    [SerializeField] private TextMeshProUGUI gameOverTitleText;
    [SerializeField] private TextMeshProUGUI gameOverSubtitleText;
    [SerializeField] private TextMeshProUGUI finalScoreLabelText;

    [Header("Buttons")]
    [SerializeField] private Button[] buttons;

    [Header("Health")]
    [SerializeField] private Slider healthSlider;


    private StarBlasterOptions options;

    private void Awake()
    {
        if (configLoader == null)
            configLoader = StarBlasterConfigLoader.Instance;
    }

    private void OnEnable()
    {
        if (StarBlasterConfigLoader.Instance != null)
        {
            StarBlasterConfigLoader.Instance.OnConfigLoaded += ApplyConfig;

            if (StarBlasterConfigLoader.Instance.Options != null)
            {
                ApplyConfig(StarBlasterConfigLoader.Instance.Options);
            }
        }
    }

    private void OnDisable()
    {
        if (StarBlasterConfigLoader.Instance != null)
        {
            StarBlasterConfigLoader.Instance.OnConfigLoaded -= ApplyConfig;
        }
    }

    private void Start()
    {
        if (configLoader != null && configLoader.Options != null)
            ApplyConfig(configLoader.Options);
    }

    public void ApplyConfig(StarBlasterOptions loadedOptions)
    {
        if (loadedOptions == null)
        {
            Debug.LogWarning("StarBlasterConfigApplier: Options are null.");
            return;
        }

        options = loadedOptions;

        ApplyTexts();
        ApplyColors();
        ApplyBackground();
        ApplyGameplay();
        ApplyEffects();

        StartCoroutine(ApplyImages());
        StartCoroutine(ApplySounds());
    }

    private void ApplyTexts()
    {
        TextsConfig texts = options.texts;

        SetText(titleText, texts.title);
        SetText(subtitleText, texts.subtitle);
        SetText(scoreLabelText, texts.scoreLabel);
        SetText(gameOverTitleText, texts.gameOverTitle);
        SetText(gameOverSubtitleText, texts.gameOverSubtitle);

        SetButtonText("Start", texts.startButton);
        SetButtonText("Quit", texts.quitButton);
        SetButtonText("PlayAgain", texts.playAgainButton);
        SetButtonText("MainMenu", texts.mainMenuButton);
    }

    private void SetButtonText(string buttonNamePart, string value)
    {
        if (buttons == null || string.IsNullOrWhiteSpace(value))
            return;

        foreach (Button button in buttons)
        {
            if (button == null)
                continue;

            if (!button.name.Contains(buttonNamePart, System.StringComparison.OrdinalIgnoreCase))
                continue;

            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
                buttonText.text = value;
        }
    }

    private void ApplyColors()
    {
        ColorsConfig colors = options.colors;

        if (TryParseColor(colors.primaryColor, out Color primary))
        {
            if (titleText != null) titleText.color = primary;
            if (gameOverTitleText != null) gameOverTitleText.color = primary;
        }

        if (TryParseColor(colors.secondaryColor, out Color secondary))
        {
            if (subtitleText != null) subtitleText.color = secondary;
            if (gameOverSubtitleText != null) gameOverSubtitleText.color = secondary;
        }

        if (TryParseColor(colors.uiTextColor, out Color uiTextColor))
        {
            if (scoreLabelText != null) scoreLabelText.color = uiTextColor;
            if (finalScoreLabelText != null) finalScoreLabelText.color = uiTextColor;
        }

        if (TryParseColor(colors.buttonColor, out Color buttonColor))
        {
            if (buttons != null)
            {
                 foreach (Button button in buttons)
                {
                    if (button == null) continue;

                    Image buttonImage = button.GetComponent<Image>();
                    if (buttonImage != null)
                        buttonImage.color = buttonColor;
                }
            }
        }

        if (TryParseColor(colors.buttonTextColor, out Color buttonTextColor))
        {
            if (buttons != null)
            {
                foreach (Button button in buttons)
                {
                    if (button == null) continue;

                    TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                    if (buttonText != null)
                        buttonText.color = buttonTextColor;
                }
            }
        }

        if (healthSlider != null && TryParseColor(colors.healthBarColor, out Color healthColor))
        {
            Image fill = healthSlider.fillRect != null
                ? healthSlider.fillRect.GetComponent<Image>()
                : null;

            if (fill != null)
                fill.color = healthColor;
        }
    }

    private void ApplyBackground()
    {
        BackgroundScroller[] scrollers = FindObjectsByType<BackgroundScroller>(FindObjectsSortMode.None);

        foreach (var scroller in scrollers)
        {
            if (scroller.CompareTag("BackgroundBase"))
            {
                scroller.ApplyConfig(
                    options.background.baseScrollSpeedY
                );
            }
            else if (scroller.CompareTag("BackgroundTop"))
            {
                scroller.ApplyConfig(
                    options.background.topScrollSpeedY
                );
            }
        }
    }

    private void ApplyGameplay()
    {
        GameplayConfig gameplay = options.gameplay;

        if (playerController != null)
            playerController.ApplyConfig(gameplay.player.moveSpeed);

        if (playerShooter != null)
        {
            playerShooter.ApplyConfig(
                gameplay.player.projectileSpeed,
                gameplay.player.projectileLifetime,
                gameplay.player.fireRate
            );
        }
    }

    private void ApplyEffects()
    {
        if (cameraShake != null)
        {
            cameraShake.ApplyConfig(
                options.effects.enableCameraShake,
                options.effects.cameraShakeDuration,
                options.effects.cameraShakeMagnitude
            );
        }
    }

    private IEnumerator ApplyImages()
    {
        ImagesConfig images = options.images;

        yield return LoadSpriteToRenderer(images.backgroundImage, backgroundRenderer);
        yield return LoadSpriteToRenderer(images.backgroundTopLayerImage, backgroundTopLayerRenderer);
    }

    private IEnumerator ApplySounds()
    {
        SoundUrlsConfig sounds = options.soundUrls;

        AudioClip shootClip = null;
        AudioClip damageClip = null;

        yield return LoadAudioClip(sounds.shoot, clip => shootClip = clip);
        yield return LoadAudioClip(sounds.damage, clip => damageClip = clip);
        
        if (audioManager != null)
            audioManager.ApplyConfig(shootClip, damageClip);
    }

    private IEnumerator LoadSpriteToRenderer(string url, SpriteRenderer targetRenderer)
    {
        if (targetRenderer == null || string.IsNullOrWhiteSpace(url))
            yield break;

        using UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning($"Failed to load sprite: {url}. Error: {request.error}");
            yield break;
        }

        Texture2D texture = DownloadHandlerTexture.GetContent(request);

        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            100f
        );

        targetRenderer.sprite = sprite;
    }

    private IEnumerator LoadAudioClip(string url, System.Action<AudioClip> onLoaded)
    {
        if (string.IsNullOrWhiteSpace(url))
            yield break;

        using UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning($"Failed to load audio: {url}. Error: {request.error}");
            yield break;
        }

        onLoaded?.Invoke(DownloadHandlerAudioClip.GetContent(request));
    }

    private void SetText(TextMeshProUGUI textElement, string value)
    {
        if (textElement != null && !string.IsNullOrWhiteSpace(value))
            textElement.text = value;
    }

    private bool TryParseColor(string hex, out Color color)
    {
        color = Color.white;

        if (string.IsNullOrWhiteSpace(hex))
            return false;

        return ColorUtility.TryParseHtmlString(hex, out color);
    }
}