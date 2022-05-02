using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Noeud
{
    public int x, y;
    public int cout;
    public float heuristique;
}

public class PathfindingAStar : MonoBehaviour
{
    private int[,] grid;

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

    public List<Noeud> ReconstituerChemin(List<Noeud> closedList, Noeud u)
    {
        List<Noeud> finaleListe = new List<Noeud>();
        Noeud currentNoeud = u;
        while (!(currentNoeud.x == currentNoeud.y))
        {

        }
        return finaleListe;
    }

    public List<Noeud> TrouverVoisinsDeNoeud(Noeud u)
    {
        var listeRetour = new List<Noeud>();
        if (u.x > 0)
        {
            Noeud v = new Noeud();
            v.x = u.x - 1;
            v.y = u.y;
            v.cout = 0;
            v.heuristique = 0;
            listeRetour.Add(v);
        }
        if (u.x < grid.GetLength(0) - 1)
        {
            Noeud v = new Noeud();
            v.x = u.x + 1;
            v.y = u.y;
            v.cout = 0;
            v.heuristique = 0;
            listeRetour.Add(v);
        }
        if (u.y > 0)
        {
            Noeud v = new Noeud();
            v.x = u.x;
            v.y = u.y - 1;
            v.cout = 0;
            v.heuristique = 0;
            listeRetour.Add(v);
        }
        if (u.y < grid.GetLength(1) - 1)
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
                else if (CompareParHeuristique(u, v) == -1)
                {
                    return true;
                }
                else return false;
            }
        }
        return false;
    }

    public List<Noeud> CheminPlusCourt(int[,] grid, Noeud objectif, Noeud depart)
    {
        var closedList = new List<Noeud>();
        var openList = new List<Noeud>();
        openList.Add(depart);
        while (openList.Count > 0)
        {
            var u = TrouveNoeudMin(openList);
            openList.Remove(u);
            if (u.x == objectif.x && u.y == objectif.y)
            {
                return ReconstituerChemin(closedList, u);
            }
            var tvdn = TrouverVoisinsDeNoeud(u);
            for (int i = 0; i < tvdn.Count; i++)
            {
                var v = tvdn[i];

                if (!(EstDansListe(closedList, v, 0) || EstDansListe(openList, v, 1)))
                {
                    v.cout = u.cout + 1;
                    v.heuristique = v.cout + Mathf.Sqrt(Mathf.Pow(v.x - objectif.x, 2) + Mathf.Pow(v.y - objectif.y, 2));
                    openList.Add(v);
                }
            }
            closedList.Add(u);
        }
        return new List<Noeud>();
        Debug.Log("Pas de chemin trouvé");
    }
}
