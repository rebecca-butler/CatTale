using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] int sceneToLoad = -1;
    [SerializeField] Transform spawnPoint;

    PlayerController player;

    public void OnPlayerTriggered(PlayerController player) {
        this.player = player;
       StartCoroutine(SwitchScene());
    }

    IEnumerator SwitchScene() {
        DontDestroyOnLoad(gameObject);

        // Load new scene
        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        // Get destination portal in new scene and update player position
        var destPortal = FindObjectsOfType<Portal>().First(x => x != this);
        player.Character.SetPositionAndSnapToTile(destPortal.spawnPoint.position);

        // When updates are done, destroy current portal
        Destroy(gameObject);
    }

    public Transform SpawnPoint => spawnPoint;
}
