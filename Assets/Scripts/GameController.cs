using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle, Dialogue }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;

    private void Start() {
        playerController.OnEncountered += StartBattle;
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

    void StartBattle() {
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
            playerController.HandleUpdate();
        }
        else if (state == GameState.Battle) {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialogue) {
            DialogueManager.Instance.HandleUpdate();
        }
    }
}
