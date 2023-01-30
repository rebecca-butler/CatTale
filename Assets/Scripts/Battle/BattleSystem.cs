using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Possible states of battle
public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy };

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogueBox dialogueBox;

    BattleState state;
    int currentAction;

    private void Start() {
        state = BattleState.Start;
        dialogueBox.EnableActionSelector(false);
        StartCoroutine(SetUpBattle());
    }

    // Set up battle elements
    public IEnumerator SetUpBattle() {
        // Set up player and enemy
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        // Start initial dialogue coroutine and wait 1 second
        yield return dialogueBox.TypeDialogue($"A wild {enemyUnit.Pokemon.Base.Name} appeared!");
        yield return new WaitForSeconds(1f);

        // Start player action phase
        PlayerAction();
    }

    // Update scene based on current battle state
    private void Update() {
        if (state == BattleState.PlayerAction) {
            HandleActionSelection();
        }
    }

    // Handle player action phase
    void PlayerAction() {
        state = BattleState.PlayerAction;
        Debug.Log("Player action");
        StartCoroutine(dialogueBox.TypeDialogue("Choose an action."));
        dialogueBox.EnableActionSelector(true);
    }

    // Handle player move phase
    IEnumerator PlayerMove() {
        state = BattleState.PlayerMove;
        Debug.Log("Player move");
        yield return dialogueBox.TypeDialogue($"{enemyUnit.Pokemon.Base.Name} says: don't kill me, I have a family :(");
        yield return new WaitForSeconds(3f);
        PlayerAction();
    }

    // Handle player keyboard input to select an action
    void HandleActionSelection() {
        // Increase action index when player presses down
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (currentAction < 1) {
                ++currentAction;
            }
        }
        // Decrease action index when player presses up
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (currentAction > 0) {
                --currentAction;
            }
        }

        // Update selected action text in UI
        dialogueBox.UpdateActionSelection(currentAction);

        // Execute action when player presses z
        if (Input.GetKeyDown(KeyCode.Z)) {
            state = BattleState.Busy;
            dialogueBox.EnableActionSelector(false);

            // Fight
            if (currentAction == 0) {
                StartCoroutine(PlayerMove());
            }
            // Run
            else if (currentAction == 1) {
                StartCoroutine(PlayerMove());
            }
        }
    }
}
