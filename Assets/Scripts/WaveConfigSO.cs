using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfig", menuName = "New WaveConfig")]
public class WaveConfigSO : ScriptableObject
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform pathPrefab;
    [SerializeField] float enemyMoveSpeed = 5f;
    [SerializeField] float timeBetweenEnemySpawns = 1f;
    [SerializeField] float enemySpawnVarience = 0f;
    [SerializeField] float minimumSpawnVarience = 0.2f;

    public GameObject GetEnemyPrefab(int index)
    {
        return enemyPrefabs[index];
    }
    public int GetEnemyCount()
    {
        return enemyPrefabs.Length;
    }
    public float GetEnemyMoveSpeed()
    {
        return enemyMoveSpeed;
    }
    public Transform[] GetWaypoints()
    {
        Transform[] waypoints = new Transform[pathPrefab.childCount];
        for (int i = 0; i < pathPrefab.childCount; i++)
        {
            waypoints[i] = pathPrefab.GetChild(i);
        }
        return waypoints;
    }
    public Transform GetStartingWaypoint()
    {
        return pathPrefab.GetChild(0);
    }

    public float GetRandomEnemySpawnTime()
    {
        float spawnTime = Random.Range(
            timeBetweenEnemySpawns - minimumSpawnVarience,
            timeBetweenEnemySpawns + enemySpawnVarience);
        return Mathf.Clamp(spawnTime, minimumSpawnVarience, float.MaxValue);
    }
}
