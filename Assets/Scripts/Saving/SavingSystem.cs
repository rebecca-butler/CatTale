using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavingSystem : MonoBehaviour
{
    public static SavingSystem i { get; private set; }

    private void Awake() {
        i = this;
    }

    // Hashmap to look up entity states by entity ID
    Dictionary<string, object> gameState = new Dictionary<string, object>();

    // Store input savable entities in the gameState
    // Used for storing state when unloading a scene
    public void CaptureEntityStates(List<SavableEntity> savableEntities) {
        foreach (SavableEntity savable in savableEntities) {
            gameState[savable.UniqueId] = savable.CaptureState();
        }
    }

    // Restore input savable entities from the gameState
    // Used for restoring state when loading a scene
    public void RestoreEntityStates(List<SavableEntity> savableEntities) {
        foreach (SavableEntity savable in savableEntities) {
            string id = savable.UniqueId;
            if (gameState.ContainsKey(id))
                savable.RestoreState(gameState[id]);
        }
    }

    // Capture states of all savable objects in the currently loaded scene
    private void CaptureState(Dictionary<string, object> state) {
        foreach (SavableEntity savable in FindObjectsOfType<SavableEntity>()) {
            state[savable.UniqueId] = savable.CaptureState();
        }
    }

    // Restore states of all savable objects in the currently loaded scene
    private void RestoreState(Dictionary<string, object> state) {
        foreach (SavableEntity savable in FindObjectsOfType<SavableEntity>()) {
            string id = savable.UniqueId;
            if (state.ContainsKey(id))
                savable.RestoreState(state[id]);
        }
    }

    public void Save(string saveFile) {
        // Update gameState by capturing current savable objects
        CaptureState(gameState);

        // Save gameState to file
        SaveFile(saveFile, gameState);
    }

    public void Load(string saveFile) {
        // Load gameState from file
        gameState = LoadFile(saveFile);
        
        // Update gameState by restoring current savable objects
        RestoreState(gameState);
    }

    public void Delete(string saveFile) {
        File.Delete(GetPath(saveFile));
    }

    void SaveFile(string saveFile, Dictionary<string, object> state) {
        string path = GetPath(saveFile);
        print($"saving to {path}");

        using (FileStream fs = File.Open(path, FileMode.Create))
        {
            // Serialize object
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fs, state);
        }
    }

    Dictionary<string, object> LoadFile(string saveFile) {
        string path = GetPath(saveFile);
        if (!File.Exists(path))
            return new Dictionary<string, object>();

        using (FileStream fs = File.Open(path, FileMode.Open))
        {
            // Deserialize object
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            return (Dictionary<string, object>)binaryFormatter.Deserialize(fs);
        }
    }

    private string GetPath(string saveFile)
    {
        return Path.Combine(Application.persistentDataPath, saveFile);
    }
}
