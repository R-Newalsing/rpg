using System.Collections;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement {
public class SavingWrapper : MonoBehaviour {
    const string defaultSaveFile = "file";
    public float fadeInTime = 0.5f;

    IEnumerator Start() {
        Fader fader = FindObjectOfType<Fader>();

        fader.FadeOutImmediate();
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
        yield return fader.FadeIn(fadeInTime);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            Load();
        }
    }

    public void Save() {
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }

    public void Load() {
        GetComponent<SavingSystem>().Load(defaultSaveFile);
    }
}}
