using System.Collections;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement {
public class SavingWrapper : MonoBehaviour {
    const string defaultSaveFile = "file";
    public float fadeInTime = 0.5f;

    private void Awake() {
        StartCoroutine(LoadLastScene());
    }

    IEnumerator LoadLastScene() {
        yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
        Fader fader = FindObjectOfType<Fader>();
        fader.FadeOutImmediate();
        yield return fader.FadeIn(fadeInTime);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.S)) Save();
        if (Input.GetKeyDown(KeyCode.L)) Load();
        if (Input.GetKeyDown(KeyCode.Backspace)) Delete();
    }

    public void Save() {
        GetComponent<SavingSystem>().Save(defaultSaveFile);
    }

    public void Load() {
        GetComponent<SavingSystem>().Load(defaultSaveFile);
    }

    public void Delete() {
        GetComponent<SavingSystem>().Delete(defaultSaveFile );
    }
}}
