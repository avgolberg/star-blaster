using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] WaveConfigSO[] waveConfigs;
    [SerializeField] float timeBetweenWaves = 1f;
    [SerializeField] bool isLooping;
    WaveConfigSO currentWave;
    void Start()
    {
       StartCoroutine(SpawnEnemies());
    }

    public WaveConfigSO GetCurrentWave()
    {
        return currentWave;
    }

    IEnumerator SpawnEnemies()
    {
        do
        {
           foreach (var wave in waveConfigs)
           {
            currentWave = wave;
                for (int i = 0; i < currentWave.GetEnemyCount(); i++)
                {
                    Instantiate(currentWave.GetEnemyPrefab(i),
                        currentWave.GetStartingWaypoint().position,
                        Quaternion.identity,
                        transform);
                    yield return new WaitForSecondsRealtime(currentWave.GetRandomEnemySpawnTime());
                }
            yield return new WaitForSecondsRealtime(timeBetweenWaves);
            }
        }
        while (isLooping);
        
    }
}
