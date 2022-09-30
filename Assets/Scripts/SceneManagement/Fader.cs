using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement {
public class Fader : MonoBehaviour {
    CanvasGroup group;

    private void Awake() {
        group = GetComponent<CanvasGroup>();
    }

    public void FadeOutImmediate() {
        group.alpha = 1f;
    }

    public IEnumerator FadeOut(float time) {
        while (group.alpha < 1) {
            group.alpha += Time.deltaTime / time;
            yield return null;
        }
    }

    public IEnumerator FadeIn(float time) {
        while (group.alpha > 0) {
            group.alpha -= Time.deltaTime / time;
            yield return null;
        }
    }
}}
