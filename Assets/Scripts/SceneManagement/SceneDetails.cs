using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneDetails : MonoBehaviour
{
    [SerializeField] List<SceneDetails> connectedScenes;

    List<SavableEntity> savableEntities;

    public bool IsLoaded { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision) {
        // Check if player has entered trigger box
        if (collision.tag == "Player") {
            Debug.Log($"Entered {gameObject.name} scene");

            LoadScene();
            GameController.Instance.SetCurrentScene(this);

            // Load all connected scenes
            foreach (var scene in connectedScenes) {
                scene.LoadScene();
            }

            // Unload all scenes that are no longer connected
            var previousScene = GameController.Instance.PreviousScene;
            if (previousScene != null) {
                var previouslyConnectedScenes = previousScene.connectedScenes;
                foreach (var scene in previouslyConnectedScenes) {
                    // If previously connected scene is not currently connected and it's not this scene, unload
                    if (!connectedScenes.Contains(scene) && scene != this) {
                        scene.UnloadScene();
                    }
                }

                // If the previous scene itself is no longer connected, unload it too
                if (!connectedScenes.Contains(previousScene)) {
                    previousScene.UnloadScene();
                }
            }
        }
    }

    public void LoadScene() {
        if (!IsLoaded) {
            // Load in additive mode (don't destroy other open scenes)
            var operation = SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            IsLoaded = true;

            // Wait until scene loading is complete
            operation.completed += (AsyncOperation op) => {
                // Restore state of savable entities in scene
                savableEntities = GetSavableEntitiesInScene();
                SavingSystem.i.RestoreEntityStates(savableEntities);
            };
        }
    }

    public void UnloadScene() {
        Debug.Log($"UnloadScene: {gameObject.name}");
        if (IsLoaded) {
            // Store state of savable entities in scene
            SavingSystem.i.CaptureEntityStates(savableEntities);

            SceneManager.UnloadSceneAsync(gameObject.name);
            IsLoaded = false;
        }
    }

    List<SavableEntity> GetSavableEntitiesInScene() {
        var currentScene = SceneManager.GetSceneByName(gameObject.name);
        var savableEntities = FindObjectsOfType<SavableEntity>().Where(x => x.gameObject.scene == currentScene).ToList();
        return savableEntities;
    }
}
