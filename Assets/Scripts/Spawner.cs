// Holden Ernest - 10/11/2023
// a spawner object, I just made this to test placing objects in the editor

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Spawner : MonoBehaviour
{
    public GameObject thingToSpawn;
    public float spawnTimer;
    // float ammountToSpawn; // maybe if groups should be spawned instead of singles
    void Start()
    {
        if (thingToSpawn)
            InvokeRepeating("spawnClock", 2.0f, spawnTimer);
    }

    private void spawnClock() {
        Instantiate(thingToSpawn);
    }
}
