using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    const int gridHeight = 28, gridWidth = 28;
    [SerializeField]
    private int[,] grid = new int[gridHeight, gridWidth];

    [SerializeField]
    private GameObject blockChemin, blockPlaine;

    void Start()
    {
        RandomizeGridValues();

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j] == 0)
                {
                    Instantiate(blockChemin, new Vector3(i, 0, j), Quaternion.identity, transform);
                }
                else if (grid[i, j] == 1)
                {
                    Instantiate(blockPlaine, new Vector3(i, 0.5f, j), Quaternion.identity, transform);
                }
            }
        }
    }

    private void RandomizeGridValues()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j] = Random.Range(0, 2);
            }
        }
    }
}
