using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementTour : MonoBehaviour
{
    const int gridHeight = 28, gridWidth = 28;
    [SerializeField]
    public int[,] towerGrid = new int[gridHeight, gridWidth];

    public GameObject[,] towerMatrix = new GameObject[gridHeight, gridWidth];

    public bool isPlacingTower = false;
    private int layerMask = 1<<6;
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private TerrainGenerator terrainGenerator;
    [SerializeField]
    private PathfindingAStar pathfindingAStar;
    [SerializeField]
    private SpawnCube spawnCube;
    [SerializeField]
    private DestructionTour destructionTour;

    void Start()
    {
        
    }

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
                //Debug.Log("xGrid: " + xGrid + " yGrid: " + yGrid);
                if (towerGrid[xGrid, yGrid] == 0)
                {
                    towerGrid[xGrid, yGrid] = 1;
                    terrainGenerator.grid[xGrid, yGrid] = 1;
                    foreach (var enemy in spawnCube.listeEnnemis)
                    {
                        if (enemy.transform.position != Vector3.zero)
                        {
                            var enemyMoving = enemy.GetComponent<EnemyMoving>();
                            var intX = (int)enemy.transform.position.x;
                            var intY = (int)enemy.transform.position.z;
                            if (IsTowerCoordoneesInList(xGrid, yGrid, enemyMoving.listOfPositions))
                            {
                                var departEnemy = new Noeud();
                                departEnemy.x = intX; departEnemy.y = intY; departEnemy.cout = 0; departEnemy.heuristique = 0f;
                                bool doitAttaque;
                                int potentielX, potentielY;
                                pathfindingAStar.NouveauChemin(departEnemy, pathfindingAStar.arrivee, enemy, out doitAttaque, out potentielX, out potentielY);
                                var cheminDeVector3 = enemyMoving.NoeudToVector3(pathfindingAStar.chemin);
                                enemyMoving.vaDetruireUneTour = doitAttaque;
                                enemyMoving.objectifX = potentielX;
                                enemyMoving.objectifY = potentielY;
                                enemyMoving.listOfPositions = cheminDeVector3;
                                enemyMoving.listOfPositions.Insert(0, enemyMoving.transform.position);
                                enemyMoving.currentIndex = 0;
                                enemyMoving.currentLength = enemyMoving.GetNewCurrentLength();
                            }
                        }
                    }
                    pathfindingAStar.NouveauChemin(pathfindingAStar.depart, pathfindingAStar.arrivee, null);
                    var newTower = Instantiate(towerPrefab, hit.transform.position + Vector3.up, hit.transform.rotation);
                    newTower.name = "Tower" + xGrid + ":" + yGrid;
                    towerMatrix[xGrid, yGrid] = newTower;
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

    public bool IsTowerCoordoneesInList(int i, int j, List<Vector3> liste)
    {
        for (int k = 0; k < liste.Count; k++)
        {
            if ((int)liste[k].x == i && (int)liste[k].z == j) return true;
        }
        return false;
    }

    public void startPlacingTowers()
    {
        isPlacingTower = true;
        destructionTour.isDestructingTower = false;
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
