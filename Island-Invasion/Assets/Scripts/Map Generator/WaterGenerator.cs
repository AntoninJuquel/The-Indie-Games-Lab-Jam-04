using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour
{
    public GameObject water;
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
        for (int x = 0; x < level.roomSizeWorldUnits.x; x++)
        {
            for (int z = 0; z < level.roomSizeWorldUnits.z; z++)
            {
                switch (level.grid[x, z])
                {
                    case LevelGenerator.gridSpace.empty:
                        level.Spawn(x, z, water, new Vector3(90,0,0),0);
                        break;
                    case LevelGenerator.gridSpace.floor:
                        break;
                    case LevelGenerator.gridSpace.wall:
                        break;
                }
            }
        }
    }
}
