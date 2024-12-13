using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    public GameObject minionPrefab; // Prefab minion
    public Transform statue; // Referensi ke statue
    public Transform player; // Referensi ke player
    public float spawnInterval = 3f; // Interval spawn
    public Transform[] spawnPoints; // Titik spawn minion

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnMinion();
            timer = 0f;
        }
    }

    private void SpawnMinion()
    {
        if (spawnPoints.Length == 0) return;

        // Pilih titik spawn secara acak
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Spawn minion
        GameObject minion = Instantiate(minionPrefab, spawnPoint.position, Quaternion.identity);

        // Set referensi target
        MinionAI minionAI = minion.GetComponent<MinionAI>();
        if (minionAI != null)
        {
            minionAI.statue = statue;
            minionAI.player = player;
        }
    }
}
