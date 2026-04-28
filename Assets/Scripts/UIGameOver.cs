using TMPro;
using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    ScoreKeeper scoreKeeper;

    void Awake()
    {
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
    }
    
    void Start()
    {
        string label = "FINAL SCORE";

        StarBlasterOptions options = StarBlasterConfigLoader.Instance?.Options;

        if (options?.texts != null && !string.IsNullOrWhiteSpace(options.texts.finalScoreLabel))
        {
            label = options.texts.finalScoreLabel;
        }

        int score = scoreKeeper != null ? scoreKeeper.GetScore() : 0;

        scoreText.text = $"{label}\n{scoreKeeper.GetScore()}";
    }
}
