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
            Instantiate(spawnedObject, transform.position, Quaternion.identity, papa);
            currentDeltaT = maxDeltaT;
        }
    }
}
