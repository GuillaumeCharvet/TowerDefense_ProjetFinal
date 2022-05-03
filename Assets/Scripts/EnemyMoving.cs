using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    public Transform cube_transform;
    [SerializeField]
    private float lerpValue = 0;
    [SerializeField]
    private List<Vector3> listOfPositions;
    private float height = 1f;
    private int currentIndex = 0;
    private float currentLength;
    private float speed = 10f;

    [SerializeField]
    private PathfindingAStar pathfindingAStar;

    private void Start()
    {
        pathfindingAStar = FindObjectOfType<PathfindingAStar>();
        listOfPositions = NoeudToVector3(pathfindingAStar.chemin);
        if (listOfPositions != new List<Vector3>())
        {
            currentLength = Vector3.Distance(listOfPositions[0], listOfPositions[1]);
        }
    }

    void Update()
    {
        //lerpValue += 0.002f;
        //transform.position = Vector3.Lerp(listOfPositions[currentIndex], listOfPositions[currentIndex + 1], lerpValue);
        lerpValue += speed * Time.deltaTime;

        cube_transform.position = Vector3.MoveTowards(listOfPositions[currentIndex], listOfPositions[currentIndex + 1], lerpValue);
        if (lerpValue >= currentLength)
        {
            lerpValue = 0f;
            currentIndex = Mathf.Min(currentIndex + 1, listOfPositions.Count - 2);
            if (currentIndex == listOfPositions.Count - 2)
            {
                Destroy(cube_transform.gameObject);
            }
            currentLength = Vector3.Distance(listOfPositions[currentIndex], listOfPositions[currentIndex + 1]);
        }
    }

    public List<Vector3> NoeudToVector3(List<Noeud> chemin)
    {
        var newList = new List<Vector3>();
        for (int i = 0; i < chemin.Count; i++)
        {
            var newElement = new Vector3(chemin[i].x, height, chemin[i].y);
            newList.Add(newElement);
        }
        return newList;
    }
}
