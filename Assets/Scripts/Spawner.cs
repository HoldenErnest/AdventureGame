using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject thingToSpawn;
    public float spawnTimer;
    // float ammountToSpawn; // maybe if groups should be spawned instead of singles
    void Start()
    {
        InvokeRepeating("spawnClock", 2.0f, spawnTimer);
    }

    private void spawnClock() {
        Instantiate(thingToSpawn);
    }
}
