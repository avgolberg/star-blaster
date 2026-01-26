using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health = 50;
    [SerializeField] ParticleSystem hitParticles;

    [SerializeField] bool isPlayer;
    [SerializeField] int scoreValue = 50;
    ScoreKeeper scoreKeeper;

    [SerializeField] bool applyCameraShake;
    CameraShake cameraShake;
    AudioManager audioManager;

    void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            TakeDamage(damageDealer.GetDamage());
            PlayHitParticles();
            damageDealer.Hit();
            audioManager.PlayDamageSFX();

            if (applyCameraShake)
            {
                cameraShake.Play();
            }
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
         if (!isPlayer)
            {
                scoreKeeper.ModifyScore(scoreValue);    
            }            
            Destroy(gameObject);
    }

    void PlayHitParticles()
    {
        if (hitParticles != null)
        {
            ParticleSystem particles = Instantiate(hitParticles, transform.position, Quaternion.identity);
            Destroy(particles, particles.main.duration + particles.main.startLifetime.constantMax);
        }
    }
}
