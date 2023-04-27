using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneDetails : MonoBehaviour
{
    [SerializeField] List<SceneDetails> connectedScenes;

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
            if (GameController.Instance.PreviousScene != null) {
                var previouslyConnectedScenes = GameController.Instance.PreviousScene.connectedScenes;
                foreach (var scene in previouslyConnectedScenes) {
                    // If previous scene is not currently connected and it's not this scene, unload
                    if (!connectedScenes.Contains(scene) && scene != this) {
                        scene.UnloadScene();
                    }
                }
            }
        }
    }

    public void LoadScene() {
        if (!IsLoaded) {
            // Load in additive mode (don't destroy other open scenes)
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            IsLoaded = true;
        }
    }

    public void UnloadScene() {
        if (IsLoaded) {
            SceneManager.UnloadSceneAsync(gameObject.name);
            IsLoaded = false;
        }
    }
}
