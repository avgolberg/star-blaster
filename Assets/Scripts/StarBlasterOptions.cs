using System;

[Serializable]
public class StarBlasterConfigRoot
{
    public StarBlasterOptions starBlasterOptions = new();
}

[Serializable]
public class StarBlasterOptions
{
    public string difficultyPreset = "medium";

    public ImagesConfig images = new();
    public ColorsConfig colors = new();
    public SoundUrlsConfig soundUrls = new();
    public TextsConfig texts = new();

    public BackgroundConfig background = new();
    public EffectsConfig effects = new();
    public GameplayConfig gameplay = new();
}

[Serializable]
public class ImagesConfig
{
    public string backgroundImage = "";
    public string backgroundTopLayerImage = "";

    public string playerImage = "";

    public string basicEnemyImage = "";
    public string fastEnemyImage = "";
    public string tankEnemyImage = "";

    public string playerProjectileImage = "";
    public string enemyProjectileImage = "";

    public string explosionImage = "";
    public string logoImage = "";
}

[Serializable]
public class ColorsConfig
{
    public string primaryColor = "#A855F7";
    public string secondaryColor = "#111827";
    public string buttonColor = "#E9C8FF";
    public string buttonTextColor = "#6B46C1";
    public string uiTextColor = "#FFFFFF";
    public string healthBarColor = "#8B5CF6";
}

[Serializable]
public class SoundUrlsConfig
{
    public string shoot = "";
    public string damage = "";

    public string backgroundMusic = ""; //??
}

[Serializable]
public class TextsConfig
{
    public string title = "STAR BLASTER";
    public string subtitle = "PROTECT THE EARTH";

    public string startButton = "START";
    public string quitButton = "QUIT";

    public string scoreLabel = "SCORE";
    public string healthLabel = "HEALTH";

    public string gameOverTitle = "GAME OVER";
    public string gameOverSubtitle = "BETTER LUCK NEXT TIME";
    public string finalScoreLabel = "FINAL SCORE";
    public string playAgainButton = "PLAY AGAIN";
    public string mainMenuButton = "MAIN MENU";
}

[Serializable]
public class BackgroundConfig
{
    public float baseScrollSpeedY = 0.2f;
    public float topScrollSpeedY = 0.5f;
}

[Serializable]
public class EffectsConfig
{
    public bool enableCameraShake = true;
    public float cameraShakeDuration = 0.3f;
    public float cameraShakeMagnitude = 0.2f;

    public bool enableHitParticles = true;
}

[Serializable]
public class GameplayConfig
{
    public int gameDuration = 60;

    public PlayerConfig player = new();
    public EnemyConfig enemies = new();
    public ProjectileConfig projectiles = new();
    public ScoreConfig score = new();
}

[Serializable]
public class PlayerConfig
{
    public int health = 50;
    public float moveSpeed = 10f;
    public float fireRate = 0.2f;
    public int damage = 10;
    public float projectileSpeed = 10f;
    public float projectileLifetime = 5f;
}

[Serializable]
public class EnemyConfig
{
    public int basicHealth = 50;
    public int fastHealth = 30;
    public int tankHealth = 100;

    public int damage = 10;

    public float basicMoveSpeed = 5f;
    public float fastMoveSpeed = 7f;
    public float tankMoveSpeed = 3f;

    public float fireRate = 1.2f;
    public float fireRateVariance = 0.3f;
}

[Serializable]
public class ProjectileConfig
{
    public float enemyProjectileSpeed = 7f;
    public float enemyProjectileLifetime = 5f;
}

[Serializable]
public class ScoreConfig
{
    public int basicEnemyScore = 50;
    public int fastEnemyScore = 75;
    public int tankEnemyScore = 150;
}