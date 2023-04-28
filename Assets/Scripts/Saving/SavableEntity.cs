using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Attach to game objects to make them savable

[ExecuteAlways]
public class SavableEntity : MonoBehaviour
{
    [SerializeField] string uniqueId = "";

    // Hashmap to look up savable entities by ID
    static Dictionary<string, SavableEntity> globalLookup = new Dictionary<string, SavableEntity>();

    public string UniqueId => uniqueId;

    // Capture state of game object on which the SavableEntity is attached
    public object CaptureState() {
        Dictionary<string, object> state = new Dictionary<string, object>();

        // Iterate over all savable components in the game object
        foreach (ISavable savable in GetComponents<ISavable>()) {
            // Capture state for each component in dictionary
            state[savable.GetType().ToString()] = savable.CaptureState();
        }
        return state;
    }

    // Restore state of the game object on which the SavableEntity is attached
    public void RestoreState(object state) {
        Dictionary<string, object> stateDict = (Dictionary<string, object>)state;

        // Iterate over all savable components in the game object
        foreach (ISavable savable in GetComponents<ISavable>()) {
            string id = savable.GetType().ToString();

            // If a state was previously stored for this component, restore it
            if (stateDict.ContainsKey(id)) {
                savable.RestoreState(stateDict[id]);
            }
        }
    }

#if UNITY_EDITOR
    // Update method used for generating UUID of the SavableEntity
    private void Update() {
        // Don't execute in playmode
        if (Application.IsPlaying(gameObject)) return;

        // Don't generate ID for prefabs (prefab scene will have path as null)
        if (String.IsNullOrEmpty(gameObject.scene.path)) return;

        // Serialize this game object
        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty property = serializedObject.FindProperty("uniqueId");

        // Generate unique ID if it doesn't already exist
        if (String.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue)) {
            property.stringValue = Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedProperties();
        }

        // Add this game object to the hashmap by its ID
        globalLookup[property.stringValue] = this;
    }
#endif

    private bool IsUnique(string candidate) {
        if (!globalLookup.ContainsKey(candidate)) return true;

        if (globalLookup[candidate] == this) return true;

        // Handle scene unloading cases
        if (globalLookup[candidate] == null) {
            globalLookup.Remove(candidate);
            return true;
        }

        // Handle edge cases like designer manually changing the UUID
        if (globalLookup[candidate].UniqueId != candidate) {
            globalLookup.Remove(candidate);
            return true;
        }

        return false;
    }
}