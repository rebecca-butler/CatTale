using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle, Dialogue, Paused }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;
    GameState stateBeforePause;

    public SceneDetails CurrentScene { get; private set; }
    public SceneDetails PreviousScene { get; private set; }

    // Singleton to store instance of class
    public static GameController Instance { get; private set; }

    private void Awake() {
       Instance = this; 
    }

    private void Start() {
        battleSystem.OnBattleOver += EndBattle;

        DialogueManager.Instance.OnShowDialogue += () => {
            state = GameState.Dialogue;
        };
        DialogueManager.Instance.OnCloseDialogue += () => {
            if (state == GameState.Dialogue) {
                state = GameState.FreeRoam;
            }
        };
    }

    public void PauseGame(bool pause) {
        if (pause) {
            stateBeforePause = state;
            state = GameState.Paused;
        } else {
            state = stateBeforePause;
        }
    }

    public void StartBattle() {
        state = GameState.Battle;
        
        // Enable battle system and disble main camera
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        battleSystem.StartBattle();
    }

    void EndBattle(bool won) {
        state = GameState.FreeRoam;
        
        // Disable battle system and enable main camera
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
    }

    private void Update() {
        if (state == GameState.FreeRoam) {
            // If s key is pressed, save game
            if (Input.GetKeyDown(KeyCode.S)) {
                SavingSystem.i.Save("save_slot_1");
            }
            
            // If l key is pressed, load game
            if (Input.GetKeyDown(KeyCode.L)) {
                SavingSystem.i.Load("save_slot_1");
            }

            // Handle player updates
            playerController.HandleUpdate();
        }
        else if (state == GameState.Battle) {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialogue) {
            DialogueManager.Instance.HandleUpdate();
        }
    }

    public void SetCurrentScene(SceneDetails currScene) {
        PreviousScene = CurrentScene;
        CurrentScene = currScene;
    }
}
