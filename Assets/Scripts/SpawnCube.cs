using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCube : MonoBehaviour
{
    private float time = 0f;
    private bool spawning = true;
    [SerializeField]
    private float maxDeltaT, currentDeltaT;

    [SerializeField]
    private GameObject spawnedObject;
    [SerializeField]
    private Transform papa;
    [SerializeField]
    public List<GameObject> listeEnnemis;

    private void Start()
    {
        maxDeltaT = 0.3f;
        currentDeltaT = 0f;
    }

    void Update()
    {
        time += Time.deltaTime;
        currentDeltaT -= Time.deltaTime;
        if (spawning && currentDeltaT <= 0f)
        {
            var newEnemy = Instantiate(spawnedObject, transform.position, Quaternion.identity, papa);
            listeEnnemis.Add(newEnemy);
            currentDeltaT = maxDeltaT;
        }
    }
}
