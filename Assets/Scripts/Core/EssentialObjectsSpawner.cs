using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialObjectsSpawner : MonoBehaviour
{
    [SerializeField] GameObject essentialObjectsPrefab;

    private void Awake() {
        // Spawn essential objects if they don't already exist
        var existingObjects = FindObjectsOfType<EssentialObjects>();
        if (existingObjects.Length == 0) {
            Instantiate(essentialObjectsPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
