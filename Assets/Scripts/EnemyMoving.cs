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
    private int currentIndex = 0;
    private float currentLength;
    private float speed = 1f;

    private void Start()
    {
        currentLength = Vector3.Distance(listOfPositions[0], listOfPositions[1]);
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
}
