using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle, Dialogue, Menu, Bag, Paused }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    [SerializeField] InventoryUI inventoryUI;

    GameState state;
    GameState stateBeforePause;

    public SceneDetails CurrentScene { get; private set; }
    public SceneDetails PreviousScene { get; private set; }

    MenuController menuController;

    // Singleton to store instance of class
    public static GameController Instance { get; private set; }

    private void Awake() {
        Instance = this;

        menuController = GetComponent<MenuController>();
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

        menuController.onMenuExited += () => {
            state = GameState.FreeRoam;
        };
        menuController.onMenuSelected += OnMenuSelected;
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
            // If Enter key is pressed, open menu
            if (Input.GetKeyDown(KeyCode.Return)) {
                menuController.OpenMenu();
                state = GameState.Menu;
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
        else if (state == GameState.Menu) {
            menuController.HandleUpdate();
        }
        else if (state == GameState.Bag) {
            Action onBack = () => {
                inventoryUI.gameObject.SetActive(false);
                state = GameState.FreeRoam;
            };

            inventoryUI.HandleUpdate(onBack);
        }
    }

    public void SetCurrentScene(SceneDetails currScene) {
        PreviousScene = CurrentScene;
        CurrentScene = currScene;
    }

    void OnMenuSelected(int selectedItem) {
        if (selectedItem == 0) {
            // Bag
            inventoryUI.gameObject.SetActive(true);
            state = GameState.Bag;
        }
        else if (selectedItem == 1) {
            // Save
            SavingSystem.i.Save("save_slot_1");
            state = GameState.FreeRoam;
        }
        else if (selectedItem == 2) {
            // Load
            SavingSystem.i.Load("save_slot_1");
            state = GameState.FreeRoam;
        }
    }
}
