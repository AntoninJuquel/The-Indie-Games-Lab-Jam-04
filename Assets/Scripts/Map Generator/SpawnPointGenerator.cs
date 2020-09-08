using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointGenerator : MonoBehaviour
{
    public GameObject spawner;
    public int spawnPointNb;
    public int spawned = 0;
    public float distanceBtwSpawn;
    //[Range(0, 1)]
    //public float chanceOfSpawning;

    LevelGenerator level;


    private void Awake()
    {
        level = GetComponent<LevelGenerator>();
    }

    private void Update()
    {
        if (level.generationComplete)
        {
            Gen();
            this.enabled = false;
        }
    }

    public void Gen()
    {
        while (spawned < spawnPointNb)
        {
            int x = Random.Range(0, (int)level.roomSizeWorldUnits.x);
            int z = Random.Range(0, (int)level.roomSizeWorldUnits.z);

            if (level.grid[x, z] == LevelGenerator.gridSpace.floor)
            {
                Transform spawnPoint;
                if (spawned > 0)
                {
                    if (Vector3.Distance(new Vector3(x, 0, z), FindObjectOfType<WaveSpawner>().spawnPoints[spawned - 1].position) > distanceBtwSpawn)
                    {
                        spawnPoint = level.Spawn(x, z, spawner, new Vector3(0, Random.Range(0f, 360f), 0), 0.5f).transform;
                        spawnPoint.LookAt(Vector3.zero);
                        FindObjectOfType<WaveSpawner>().spawnPoints.Add(spawnPoint);
                        spawned++;
                    }
                }
                else
                {
                    spawnPoint = level.Spawn(x, z, spawner, new Vector3(0, Random.Range(0f, 360f), 0), 0.5f).transform;
                    spawnPoint.LookAt(Vector3.zero);
                    FindObjectOfType<WaveSpawner>().spawnPoints.Add(spawnPoint);
                    spawned++;
                }
            }
        }

        /*for (int x = 0; x < level.roomSizeWorldUnits.x; x++)
        {
            for (int z = 0; z < level.roomSizeWorldUnits.z; z++)
            {
                switch (level.grid[x, z])
                {
                    case LevelGenerator.gridSpace.empty:
                        break;
                    case LevelGenerator.gridSpace.floor:
                        if (Random.value < chanceOfSpawning && spawnPointNb > 0)
                        {
                            spawnPointNb--;
                            Transform spawnPoint = level.Spawn(x, z, spawner, new Vector3(0, 0, 0),0.5f).transform;
                            FindObjectOfType<WaveSpawner>().spawnPoints.Add(spawnPoint);
                        }
                        break;
                    case LevelGenerator.gridSpace.wall:
                        break;
                }
            }
        }*/
    }
}
