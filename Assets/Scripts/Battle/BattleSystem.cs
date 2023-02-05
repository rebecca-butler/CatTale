using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Possible states of battle
public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove };

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogueBox dialogueBox;

    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;

    public void StartBattle() {
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
        yield return dialogueBox.TypeDialogue($"{enemyUnit.Pokemon.Base.Name} says: don't kill me, I have a family :(");
        yield return new WaitForSeconds(1f);

        // Start player action phase
        PlayerAction();
    }

    // Update scene based on current battle state
    public void HandleUpdate() {
        if (state == BattleState.PlayerAction) {
            HandleActionSelection();
        }
    }

    // Handle player action phase
    void PlayerAction() {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogueBox.TypeDialogue("Choose an action."));
        dialogueBox.EnableActionSelector(true);
    }

    // Handle player move phase
    void PlayerMove() {
        state = BattleState.PlayerMove;
        StartCoroutine(EnemyMove());
    }

    // Handle enemy move phase
    IEnumerator EnemyMove() {
        state = BattleState.EnemyMove;

        float r = UnityEngine.Random.value;
        if (r < 0.3f) {
            yield return dialogueBox.TypeDialogue($"{enemyUnit.Pokemon.Base.Name} cried and took emotional damage!");
        } else if (r >= 0.3f && r < 0.6f) {
            yield return dialogueBox.TypeDialogue($"{enemyUnit.Pokemon.Base.Name} is having a panic attack!");
        } else {
            yield return dialogueBox.TypeDialogue($"{enemyUnit.Pokemon.Base.Name} stubbed his pinky toe!");
        }
        yield return new WaitForSeconds(1f);

        // Apply damage and update UI
        bool isFainted = enemyUnit.Pokemon.TakeDamage(enemyUnit.Pokemon);
        yield return enemyHud.UpdateHP();

        if (isFainted) {
            yield return dialogueBox.TypeDialogue($"{enemyUnit.Pokemon.Base.Name} fainted :(");
            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        } else {
            PlayerAction();
        }
    }

    // Handle player attempt to run away
    IEnumerator PlayerRun() {
        state = BattleState.PlayerMove;
        float r = UnityEngine.Random.value;

        // 90% chance of successfully escaping
        if (r < 0.01f) {
            yield return dialogueBox.TypeDialogue($"{playerUnit.Pokemon.Base.Name} ran away!");
            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        } 
        // 10% chance of failure to escape
        else {
            yield return dialogueBox.TypeDialogue($"{playerUnit.Pokemon.Base.Name} tried to run away, but got distracted by {enemyUnit.Pokemon.Base.Name} pleading for his son's life!");
            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyMove());
        }
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
            dialogueBox.EnableActionSelector(false);

            // Fight
            if (currentAction == 0) {
                PlayerMove();
            }
            // Run
            else if (currentAction == 1) {
                StartCoroutine(PlayerRun());
            }
        }
    }
}
