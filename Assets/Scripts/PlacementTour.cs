using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementTour : MonoBehaviour
{
    const int gridHeight = 28, gridWidth = 28;
    [SerializeField]
    public int[,] towerGrid = new int[gridHeight, gridWidth];

    private bool isPlacingTower = false;
    private int layerMask = 1<<6;
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private TerrainGenerator terrainGenerator;
    [SerializeField]
    private PathfindingAStar pathfindingAStar;
    [SerializeField]
    private SpawnCube spawnCube;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isPlacingTower)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, layerMask))
            {
                //Debug.Log(hit.transform.name);
                //Debug.Log("hit");
                int xGrid = (int) hit.transform.position.x;
                int yGrid = (int) hit.transform.position.z;
                Debug.Log("xGrid: " + xGrid + " yGrid: " + yGrid);
                if (towerGrid[xGrid, yGrid] == 0)
                {
                    towerGrid[xGrid, yGrid] = 1;
                    terrainGenerator.grid[xGrid, yGrid] = 1;
                    foreach (var enemy in spawnCube.listeEnnemis)
                    {
                        var intX = (int) enemy.transform.position.x;
                        var intY = (int) enemy.transform.position.z;
                        var departEnemy = new Noeud();
                        departEnemy.x = intX; departEnemy.y = intY; departEnemy.cout = 0; departEnemy.heuristique = 0f;
                        pathfindingAStar.NouveauChemin(departEnemy, pathfindingAStar.arrivee);
                        var enemyMoving = enemy.GetComponent<EnemyMoving>();
                        var cheminDeVector3 = enemyMoving.NoeudToVector3(pathfindingAStar.chemin);
                        enemyMoving.listOfPositions = cheminDeVector3;
                        enemyMoving.currentIndex = 0;
                        enemyMoving.currentLength = enemyMoving.GetNewCurrentLength();
                    }
                    pathfindingAStar.NouveauChemin(pathfindingAStar.depart, pathfindingAStar.arrivee);
                    Instantiate(towerPrefab, hit.transform.position + Vector3.up, hit.transform.rotation);
                }
            }
        }
    }

    public void DebugLogGrid(int[,] _grid)
    {
        for (int i = 0; i < 28; i++)
        {            
            Debug.Log("" + _grid[i, 0]+_grid[i, 1]+_grid[i, 2]+_grid[i, 3]+_grid[i, 4]+_grid[i, 5]+_grid[i, 6]+_grid[i, 7]+_grid[i, 8]+_grid[i, 9]+_grid[i, 10]+_grid[i, 11]+_grid[i, 12]+_grid[i, 13]+_grid[i, 14]+_grid[i, 15]+_grid[i, 16]+_grid[i, 17]+_grid[i, 18]+_grid[i, 19]+_grid[i, 20]+_grid[i, 21]+_grid[i, 22]+_grid[i, 23]+_grid[i, 24]+_grid[i, 25]+_grid[i, 26]+_grid[i, 27]);
        }
    }

    public void startPlacingTowers()
    {
        isPlacingTower = true;
    }

    public void InitializeTowerGrid()
    {
        for (int i = 0; i < towerGrid.GetLength(0); i++)
        {
            for (int j = 0; j < towerGrid.GetLength(1); j++)
            {
                towerGrid[i, j] = 0;
            }
        }
    }
}
