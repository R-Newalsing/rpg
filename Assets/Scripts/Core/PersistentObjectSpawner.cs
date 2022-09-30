using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core {
public class PersistentObjectSpawner : MonoBehaviour {
    public GameObject persistentObjectPrefab;
    static bool hasSpawned = false;

    private void Awake() {
        if (hasSpawned) return;

        SpawnPersistentObjects();
        hasSpawned = true;
    }

    void SpawnPersistentObjects () {
        GameObject persistentObject = Instantiate(persistentObjectPrefab);
        DontDestroyOnLoad(persistentObject);
    }
}}

