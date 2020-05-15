using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public enum gridSpace { empty, floor, wall, occupied };
    public gridSpace[,] grid;
    int roomHeight, roomWidth;
    public Vector3 roomSizeWorldUnits = new Vector3(30, 0, 30);

    public float worldUnitsInOneGridCell = 1;
    struct walker
    {
        public Vector3 dir;
        public Vector3 pos;
    }
    List<walker> walkers;
    [Range(0, 1)]
    public float chanceWalkerChangeDir = 0.5f, chanceWalkerSpawn = 0.05f;
    [Range(0, 1)]
    public float chanceWalkerDestroy = 0.05f;
    public int maxWalker = 10;
    [Range(0, 1)]
    public float percentToFill = 0.2f;
    public GameObject wallObj, floorObj;

    public bool generationComplete = false;


    string holderName = "Generated Map";
    Transform mapHolder;




    private void Start()
    {
        // Create map holder object
        mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        // Resize objects
        wallObj.transform.localScale = new Vector3(worldUnitsInOneGridCell, 1, worldUnitsInOneGridCell);
        floorObj.transform.localScale = new Vector3(worldUnitsInOneGridCell, worldUnitsInOneGridCell, 1);

        Setup();
        CreateFloors();
        CreateWalls();
        SpawnLevel();

        // Repositioning the map
        mapHolder.transform.localPosition = Vector3.zero;

        generationComplete = true;
    }

    void Setup()
    {
        // Find grid size
        roomWidth = Mathf.RoundToInt(roomSizeWorldUnits.x / worldUnitsInOneGridCell);
        roomHeight = Mathf.RoundToInt(roomSizeWorldUnits.z / worldUnitsInOneGridCell);

        // Create grid
        grid = new gridSpace[roomWidth, roomHeight];
        // Set grid's default state
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int z = 0; z < roomHeight - 1; z++)
            {
                // Make every cell "empty"
                grid[x, z] = gridSpace.empty;
            }
        }
        // Set first walker
        // Init list
        walkers = new List<walker>();
        // Create a walker
        walker newWalker = new walker();
        newWalker.dir = RandomDirection();
        // Find center of grid
        Vector3 spawnPos = new Vector3(Mathf.RoundToInt(roomWidth / 2.0f), 0, Mathf.RoundToInt(roomHeight / 2.0f));
        newWalker.pos = spawnPos;
        // Add walker to list
        walkers.Add(newWalker);
    }

    Vector3 RandomDirection()
    {
        // Pick random int btw 0 and 3
        int choice = Mathf.FloorToInt(Random.value * 3.99f);
        // Use that int to chose a direction
        switch (choice)
        {
            case 0:
                return Vector3.back;
            case 1:
                return Vector3.left;
            case 2:
                return Vector3.forward;
            default:
                return Vector3.right;
        }
    }

    void CreateFloors()
    {
        int iteration = 0;
        do
        {
            // Create floor at position of every walker
            foreach (walker myWalker in walkers)
            {
                grid[(int)myWalker.pos.x, (int)myWalker.pos.z] = gridSpace.floor;
            }
            // Chance: destroy walker
            int numberChecks = walkers.Count;
            for (int i = 0; i < numberChecks; i++)
            {
                // Only if its not the only one and at a low chance
                if (Random.value < chanceWalkerDestroy && walkers.Count > 1)
                {
                    walkers.RemoveAt(i);
                    break; // Only destroy one per iteration
                }
            }
            // Chance: walker pick new direction
            for (int i = 0; i < walkers.Count; i++)
            {
                if (Random.value < chanceWalkerChangeDir)
                {
                    walker thisWalker = walkers[i];
                    thisWalker.dir = RandomDirection();
                    walkers[i] = thisWalker;
                }
            }
            // Chance: spawn new walker
            numberChecks = walkers.Count;
            for (int i = 0; i < numberChecks; i++)
            {
                // Only if # of walkers < max, and at a low chance
                if (Random.value < chanceWalkerSpawn && walkers.Count < maxWalker)
                {
                    // Create walker
                    walker newWalker = new walker();
                    newWalker.dir = RandomDirection();
                    newWalker.pos = walkers[i].pos;
                    walkers.Add(newWalker);
                }
            }
            // Move walkers
            for (int i = 0; i < walkers.Count; i++)
            {
                walker thisWalker = walkers[i];
                thisWalker.pos += thisWalker.dir;
                walkers[i] = thisWalker;
            }
            // Avoid border of grid
            for (int i = 0; i < walkers.Count; i++)
            {
                walker thisWalker = walkers[i];
                // Clamp x,z to leave a 1 space boarder: leave room for walls
                thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, 1, roomWidth - 2);
                thisWalker.pos.z = Mathf.Clamp(thisWalker.pos.z, 1, roomHeight - 2);
                walkers[i] = thisWalker;
            }
            // Check for exit loop
            if ((float)NumberOfFloors() / (float)grid.Length > percentToFill)
            {
                break;
            }
            iteration++;
        } while (iteration < 100000);
    }

    int NumberOfFloors()
    {
        int count = 0;
        foreach (gridSpace space in grid)
        {
            if (space == gridSpace.floor)
            {
                count++;
            }
        }
        return count;
    }
    void SpawnLevel()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int z = 0; z < roomHeight; z++)
            {
                switch (grid[x, z])
                {
                    case gridSpace.empty:
                        break;
                    case gridSpace.floor:
                        Spawn(x, z, floorObj, new Vector3(90, 0, 0),0.5f);
                        break;
                    case gridSpace.wall:
                        Spawn(x, z, wallObj, Vector3.zero,0.3f);
                        //Spawn(x, z, floorObj, new Vector3(90, 0, 0));
                        break;
                }
            }
        }
    }

    public GameObject Spawn(float x, float z, GameObject toSpawn, Vector3 rotation, float height)
    {
        // Find the position to spawn
        Vector3 offset = roomSizeWorldUnits / 2.0f;
        Vector3 spawnPos = new Vector3(x, height, z) * worldUnitsInOneGridCell - offset;

        // Spawn object
        GameObject gameObject = Instantiate(toSpawn, spawnPos, Quaternion.Euler(rotation));
        gameObject.transform.parent = mapHolder;

        return gameObject;
    }

    void CreateWalls()
    {
        // Loop though every grid space
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int z = 0; z < roomHeight - 1; z++)
            {
                if (grid[x, z] == gridSpace.floor)
                {
                    if (grid[x, z + 1] == gridSpace.empty)
                    {
                        grid[x, z + 1] = gridSpace.wall;
                    }
                    if (grid[x, z - 1] == gridSpace.empty)
                    {
                        grid[x, z - 1] = gridSpace.wall;
                    }
                    if (grid[x + 1, z] == gridSpace.empty)
                    {
                        grid[x + 1, z] = gridSpace.wall;
                    }
                    if (grid[x - 1, z] == gridSpace.empty)
                    {
                        grid[x - 1, z] = gridSpace.wall;
                    }
                }
            }
        }
    }
}
