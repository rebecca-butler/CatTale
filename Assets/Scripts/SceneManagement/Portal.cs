using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] int sceneToLoad = -1;
    [SerializeField] DestinationIdentifier destinationPortal;
    [SerializeField] Transform spawnPoint;

    PlayerController player;

    public void OnPlayerTriggered(PlayerController player) {
        this.player = player;
       StartCoroutine(SwitchScene());
    }

    IEnumerator SwitchScene() {
        DontDestroyOnLoad(gameObject);

        // Pause game and load new scene
        GameController.Instance.PauseGame(true);
        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        // Get destination portal in new scene and update player position
        var destPortal = FindObjectsOfType<Portal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        player.Character.SetPositionAndSnapToTile(destPortal.spawnPoint.position);

        // When updates are done, unpause and destroy this portal
        GameController.Instance.PauseGame(false);
        Destroy(gameObject);
    }

    public Transform SpawnPoint => spawnPoint;
}

public enum DestinationIdentifier { A, B, C, D, E };
