using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/* Teleports player to a new position without switching scenes */
public class LocationPortal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] DestinationIdentifier destinationPortal;
    [SerializeField] Transform spawnPoint;

    PlayerController player;
    Fader fader;

    public void OnPlayerTriggered(PlayerController player) {
        player.Character.Animator.IsMoving = false;
        this.player = player;
        StartCoroutine(Teleport());
    }

    private void Start() {
        fader = FindObjectOfType<Fader>();
    }

    IEnumerator Teleport() {
        // Pause game and fade in
        GameController.Instance.PauseGame(true);
        yield return fader.FadeIn(0.5f);

        // Get destination portal and update player position
        var destPortal = FindObjectsOfType<LocationPortal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        player.Character.SetPositionAndSnapToTile(destPortal.spawnPoint.position);

        // When updates are done, fade out and unpause game
        yield return fader.FadeOut(0.5f);
        GameController.Instance.PauseGame(false);
    }

    public Transform SpawnPoint => spawnPoint;
}
