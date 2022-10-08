using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.AI;
using UnityEngine;

namespace RPG.SceneManagement {
public class Portal : MonoBehaviour {
    public int sceneToLoad = -1;
    public Transform SpawnPoint;
    public DestinationIdentifier destination;
    public float fadeOutTimer = 0.5f;
    public float fadeInTimer = 1f;

    SavingWrapper saving;

    public enum DestinationIdentifier {
        A, B,
    }

    private void Awake() {
        saving = FindObjectOfType<SavingWrapper>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") StartCoroutine(Transition());
    }

    private IEnumerator Transition() {
        DontDestroyOnLoad(gameObject);

        yield return GetFader().FadeOut(fadeOutTimer);
        saving.Save();
        yield return SceneManager.LoadSceneAsync(sceneToLoad);
        saving.Load();
        UpdatePlayer(GetOtherPortal());
        saving.Save();
        
        yield return new WaitForSeconds(1);
        yield return GetFader().FadeIn(fadeInTimer);
       
        Destroy(this.gameObject);
    }

    Fader GetFader() {
        return FindObjectOfType<Fader>();
    }

    Portal GetOtherPortal() {
        foreach (Portal portal in FindObjectsOfType<Portal>()) {
            if (portal == this) continue;
            if (portal.destination != destination) continue;

            return portal;
        }

        return null;
    }

    void UpdatePlayer(Portal portal) {
        if (portal == null) return;
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<NavMeshAgent>().Warp(portal.SpawnPoint.position);
        player.transform.rotation = portal.SpawnPoint.rotation;
    }
}}