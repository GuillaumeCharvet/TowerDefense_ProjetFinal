using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CSVReader : MonoBehaviour
{
    public TextAsset textAssetData;

    const int gridHeight = 28, gridWidth = 28;
    [SerializeField]
    public int[,] grid = new int[gridHeight, gridWidth];
    public int[,] gridWithoutTowers = new int[gridHeight, gridWidth];

    void Awake()
    {
        ReadCSV();
    }

    public void ReadCSV()
    {
        string[] data = textAssetData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                //Debug.Log("i:"+i+"j:"+j+"grid:"+int.Parse(data[i + j * 28]));
                grid[i, j] = int.Parse(data[i + j * 28]);
                gridWithoutTowers[i, j] = int.Parse(data[i + j * 28]);
            }
        }        
    }
}
