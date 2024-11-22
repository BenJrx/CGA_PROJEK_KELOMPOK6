using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    public float minionMoveSpeed;
    public float superMinionMoveSpeed;

    public GameObject minionPrefab;
    public GameObject superMinionPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 20.0f;
    public int minionsPerWave = 6;
    public int wavesUntilSuperMinion = 3;
    private int waveCount = 0;

    public float delayBetweenMinions;

    private void Start()
    {
        StartCoroutine(SpawnMinions());
    }

    private IEnumerator SpawnMinions()
    {
        while (true)
        {
            waveCount++;
            // Check if it's time to spawn a super minion.
            if (waveCount % wavesUntilSuperMinion == 0)
            {
                for (int i = 0; i < minionsPerWave - 1; i++)
                {
                    SpawnRegularMinion();
                    yield return new WaitForSeconds(delayBetweenMinions);
                }

                SpawnRegularMinion();
                yield return new WaitForSeconds(delayBetweenMinions);

                SpawnSuperMinion();
                yield return new WaitForSeconds(spawnInterval - delayBetweenMinions * (minionsPerWave - 1) - delayBetweenMinions);
            }
            else
            {
                // Spawn regular minions in a non-super minion wave.
                for (int i = 0; i < minionsPerWave; i++)
                {
                    SpawnRegularMinion();
                    yield return new WaitForSeconds(delayBetweenMinions);
                }

                // Wait for the remaining time in the spawn interval.
                yield return new WaitForSeconds(spawnInterval - delayBetweenMinions * minionsPerWave);
            }
        }
    }

    private void SpawnRegularMinion()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject minion = Instantiate(minionPrefab, spawnPoint.position, spawnPoint.rotation);

        // Adjust the minion's move speed (NavMeshAgent's speed property).
        UnityEngine.AI.NavMeshAgent minionAgent = minion.GetComponent<UnityEngine.AI.NavMeshAgent>();
        minionAgent.speed = minionMoveSpeed; // Set the desired move speed.
    }

    private void SpawnSuperMinion()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject minion = Instantiate(superMinionPrefab, spawnPoint.position, spawnPoint.rotation);

        // Adjust the super minion's move speed (NavMeshAgent's speed property).
        UnityEngine.AI.NavMeshAgent superMinionAgent = minion.GetComponent<UnityEngine.AI.NavMeshAgent>();
        superMinionAgent.speed = superMinionMoveSpeed; // Set the desired move speed.
    }
}
