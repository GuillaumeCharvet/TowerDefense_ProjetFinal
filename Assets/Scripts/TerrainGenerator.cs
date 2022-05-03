using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    const int gridHeight = 28, gridWidth = 28;
    [SerializeField]
    public int[,] grid = new int[gridHeight, gridWidth];

    [SerializeField]
    private GameObject blockChemin, blockPlaine;

    [SerializeField]
    private CSVReader csvReader;

    void Awake()
    {
        RandomizeGridValues();
        //grid = csvReader.grid;

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
                grid[i, j] = (int) Mathf.Floor(Random.Range(0f, 1.6f));
            }
        }
        grid[0, 0] = 0;
        grid[1, 2] = 0;
        grid[2, 1] = 0;
        grid[grid.GetLength(0) - 3, grid.GetLength(1) - 2] = 0;
        grid[grid.GetLength(0) - 2, grid.GetLength(1) - 3] = 0;
        grid[grid.GetLength(0) - 1, grid.GetLength(1) - 1] = 0;
    }
}
