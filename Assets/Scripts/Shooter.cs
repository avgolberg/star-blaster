using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Base Variables")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime = 5f;
    [SerializeField] float baseFireRate = 0.2f;

    [Header("AI Variables")]
    [SerializeField] bool useAI;
    [SerializeField] float minimumFireRate = 0.2f;
    [SerializeField] float fireRateVarience = 0f;


    [HideInInspector] public bool isFiring;

    Coroutine fireCoroutine;
    AudioManager audioManager;

    void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
        if (useAI)
        {
            isFiring = true;
        }
    }

    void Update()
    {
        Firing();
    }

    void Firing()
    {
        if (isFiring && fireCoroutine == null)
        {
            fireCoroutine = StartCoroutine(FireContinuously());
        }
        else if (!isFiring && fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject projectile = Instantiate(projectilePrefab,
                          transform.position,
                          transform.rotation);

            Rigidbody2D projectileRB = projectile.GetComponent<Rigidbody2D>();
            projectileRB.linearVelocity = transform.up * projectileSpeed;

            Destroy(projectile, projectileLifetime);

            float waitTime = Random.Range(baseFireRate - fireRateVarience, baseFireRate + fireRateVarience);
            waitTime = Mathf.Clamp(waitTime, minimumFireRate, float.MaxValue);

            audioManager.PlayShootingSFX();

            yield return new WaitForSecondsRealtime(waitTime);
        }
    }

    public void ApplyConfig(float newProjectileSpeed, float newProjectileLifetime, float newBaseFireRate)
    {
        projectileSpeed = newProjectileSpeed;
        projectileLifetime = newProjectileLifetime;
        baseFireRate = newBaseFireRate;
    }
}
