using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongGrass : MonoBehaviour, IPlayerTriggerable
{
    public void OnPlayerTriggered(PlayerController player) {
        // Encounter enemy 10% of the time
        if (UnityEngine.Random.Range(1, 101)  <= 10) {
            GameController.Instance.StartBattle();
        }
    }
}
