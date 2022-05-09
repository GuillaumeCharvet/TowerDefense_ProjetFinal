using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Noeud
{
    public int x, y;
    public int cout;
    public float heuristique;
}

public class PathfindingAStar : MonoBehaviour
{
    private int[,] grid;
    public int departX, departY, arriveeX, arriveeY;
    public Noeud depart, arrivee;
    public List<Noeud> chemin;

    [SerializeField]
    private TerrainGenerator terrainGenerator;
    [SerializeField]
    private CSVReader csvReader;
    [SerializeField]
    private PlacementTour placementTour;
    [SerializeField]
    private List<Noeud> closedList = new List<Noeud>(), openList = new List<Noeud>();
    public int objectifX, objectifY;

    public void Awake()
    {
        grid = terrainGenerator.grid;
        depart = new Noeud();
        arrivee = new Noeud();
        depart.x = departX; depart.y = departY; arrivee.x = arriveeX; arrivee.y = arriveeY;
        depart.cout = 0; depart.heuristique = 0; arrivee.cout = 0; arrivee.heuristique = 0;
        NouveauChemin(depart, arrivee, null, out );
    }

    public int CompareParHeuristique(Noeud n1, Noeud n2)
    {
        if (n1.heuristique < n2.heuristique) return 1;
        else if (n1.heuristique == n2.heuristique) return 0;
        else return -1;
    }

    public Noeud TrouveNoeudMin(List<Noeud> list)
    {
        Noeud currentNoeudMin = list[0];
        foreach (Noeud noeud in list)
        {
            if(noeud.heuristique < currentNoeudMin.heuristique)
            {
                currentNoeudMin = noeud;
            }
        }
        return currentNoeudMin;
    }

    public bool SontVoisins(Noeud u, Noeud v)
    {
        if (u.x == v.x + 1 || u.x == v.x - 1)
        {
            if (u.y == v.y) return true;
        }
        else if (u.y == v.y + 1 || u.y == v.y - 1)
        {
            if (u.x == v.x) return true;
        }
        return false;
    }

    public Noeud TrouveMeilleurParent(List<Noeud> liste, Noeud fils) //#rateYourDaddy
    {
        Noeud currentBestNoeud = fils;
        foreach (Noeud v in liste)
        {
            if (SontVoisins(fils, v) && v.cout < currentBestNoeud.cout)
            {
                currentBestNoeud = v;
            }
        }
        return currentBestNoeud;
    }

    public List<Noeud> ReconstituerChemin(List<Noeud> closedList, Noeud u, Noeud start)
    {
        List<Noeud> finaleListe = new List<Noeud>();
        Noeud currentNoeud = u;
        finaleListe.Add(u);
        int i = 0; 
        while (currentNoeud.x != start.x || currentNoeud.y != start.y)
        {
            Noeud topParent = TrouveMeilleurParent(closedList, currentNoeud);
            finaleListe.Insert(0, topParent);
            closedList.Remove(topParent);
            currentNoeud = topParent;
            i++;
            if (i > 1000)
            {
                Debug.Break();
                break;
            }
        }
        return finaleListe;
    }

    public List<Noeud> TrouverVoisinsDeNoeud(Noeud u, int[,] _grid)
    {
        var listeRetour = new List<Noeud>();
        if (u.x > 0 && _grid[u.x - 1, u.y] == 0)
        {
            Noeud v = new Noeud();
            v.x = u.x - 1;
            v.y = u.y;
            v.cout = 0;
            v.heuristique = 0;
            listeRetour.Add(v);
        }
        if (u.x < grid.GetLength(0) - 1 && _grid[u.x + 1, u.y] == 0)
        {
            Noeud v = new Noeud();
            v.x = u.x + 1;
            v.y = u.y;
            v.cout = 0;
            v.heuristique = 0;
            listeRetour.Add(v);
        }
        if (u.y > 0 && _grid[u.x, u.y - 1] == 0)
        {
            Noeud v = new Noeud();
            v.x = u.x;
            v.y = u.y - 1;
            v.cout = 0;
            v.heuristique = 0;
            listeRetour.Add(v);
        }
        if (u.y < grid.GetLength(1) - 1 && _grid[u.x, u.y + 1] == 0)
        {
            Noeud v = new Noeud();
            v.x = u.x;
            v.y = u.y + 1;
            v.cout = 0;
            v.heuristique = 0;
            listeRetour.Add(v);
        }
        return listeRetour;            
    }

    public bool EstDansListe(List<Noeud> liste, Noeud u, int i)
    {
        foreach (Noeud v in liste)
        {
            if (u.x == v.x && u.y == v.y)
            {
                if (i == 0)
                {
                    return true;
                }
                else if (v.cout >= u.cout)
                {
                    return true;
                }
                else return false;
            }
        }
        return false;
    }
    public List<Noeud> CheminPlusCourt(int[,] _grid, Noeud objectif, Noeud depart, GameObject enemy, out bool aggro, out int objX, out int objY)
    {
        openList.Clear();
        closedList.Clear();
        openList.Add(depart);
        int k = 0;
        while (openList.Count > 0)
        {
            var u = TrouveNoeudMin(openList);
            openList.Remove(u);
            if (u.x == objectif.x && u.y == objectif.y)
            {
                aggro = false;
                objX = 0;
                objY = 0;
                return ReconstituerChemin(closedList, u, depart);
            }
            var tvdn = TrouverVoisinsDeNoeud(u, _grid);
            for (int i = 0; i < tvdn.Count; i++)
            {
                var v = tvdn[i];
                if (!EstDansListe(closedList, v, 0) && !EstDansListe(openList, v, 1))
                {
                    v.cout = u.cout + 1;
                    v.heuristique = v.cout + Mathf.Sqrt(Mathf.Pow(v.x - objectif.x, 2) + Mathf.Pow(v.y - objectif.y, 2));
                    openList.Add(v);
                }
            }
            closedList.Add(u);
            k++;
            if (k > 10000)
            {
                Debug.Log("CheminPlusCourt échoue après" + k + "itérations");
                Debug.Break();
                break;
            }
        }
        bool aggro2; int objX2, objY2;
        var listeCheminSansTours = CheminPlusCourt(csvReader.gridWithoutTowers, objectif, depart, enemy, out aggro2, out objX2, out objY2);
        var tourelleTrouveeSurChemin = false;
        int posTourelleDansListe = 0;
        int initialLength = listeCheminSansTours.Count;
        EnemyMoving eM = enemy.GetComponent<EnemyMoving>();
        for (int i = 0; i < initialLength; i++)
        {
            if (!tourelleTrouveeSurChemin && _grid[listeCheminSansTours[i].x, listeCheminSansTours[i].y] == 1)
            {
                int objectifX = listeCheminSansTours[i].x;
                int objectifY = listeCheminSansTours[i].y;                
                Debug.Log("Tourelle plus proche à virer : " + objectifX + ", " + objectifY);
                tourelleTrouveeSurChemin = true;
                posTourelleDansListe = i;
                listeCheminSansTours.RemoveAt(posTourelleDansListe);
            }
            else if (tourelleTrouveeSurChemin)
            {
                listeCheminSansTours.RemoveAt(posTourelleDansListe);
            }
        }
        aggro = true;
        objX = objectifX;
        objY = objectifX;
        return listeCheminSansTours;
    }

    /*public List<Noeud> CheminPlusCourt(int[,] _grid, Noeud objectif, Noeud depart, GameObject enemy, out bool aggro, out int objX, out int objY)
    {
        openList.Clear();
        closedList.Clear();
        openList.Add(depart);
        int k = 0;
        while (openList.Count > 0)
        {            
            var u = TrouveNoeudMin(openList);
            openList.Remove(u);
            if (u.x == objectif.x && u.y == objectif.y)
            {
                aggro = false;
                return ReconstituerChemin(closedList, u, depart);
            }
            var tvdn = TrouverVoisinsDeNoeud(u, _grid);
            for (int i = 0; i < tvdn.Count; i++)
            {
                var v = tvdn[i];
                if (!EstDansListe(closedList, v, 0) && !EstDansListe(openList, v, 1))
                {                    
                    v.cout = u.cout + 1;
                    v.heuristique = v.cout + Mathf.Sqrt(Mathf.Pow(v.x - objectif.x, 2) + Mathf.Pow(v.y - objectif.y, 2));
                    openList.Add(v);
                }
            }
            closedList.Add(u);
            k++;
            if (k > 10000)
            {
                Debug.Log("CheminPlusCourt échoue après" + k + "itérations");
                Debug.Break();
                break;
            }
        }
        bool balec;
        var listeCheminSansTours = CheminPlusCourt(csvReader.gridWithoutTowers, objectif, depart, enemy);
        var tourelleTrouveeSurChemin = false;
        int posTourelleDansListe = 0;
        int initialLength = listeCheminSansTours.Count;
        EnemyMoving eM = enemy.GetComponent<EnemyMoving>();
        for (int i = 0; i < initialLength; i++)
        {
            if (!tourelleTrouveeSurChemin && _grid[listeCheminSansTours[i].x, listeCheminSansTours[i].y] == 1)
            {
                int objectifX = listeCheminSansTours[i].x;
                int objectifY = listeCheminSansTours[i].y;
                eM.objectifX = objectifX;
                eM.objectifY = objectifX;
                Debug.Log("Tourelle plus proche à virer : " + objectifX + ", " + objectifY);
                tourelleTrouveeSurChemin = true;
                posTourelleDansListe = i;
                listeCheminSansTours.RemoveAt(posTourelleDansListe);
            }
            else if (tourelleTrouveeSurChemin)
            {
                listeCheminSansTours.RemoveAt(posTourelleDansListe);
            }
        }
        aggro = true;
        return listeCheminSansTours;
    }*/

    public void NouveauChemin(Noeud _depart, Noeud _arrivee, GameObject enemy, out bool aggro, out int objX, out int objY)
    {
        grid = terrainGenerator.grid;
        //grid = csvReader.grid;
        //placementTour.DebugLogGrid(grid);
        chemin = CheminPlusCourt(grid, _arrivee, _depart, enemy, out aggro, out objX, out objY);
    }
}
