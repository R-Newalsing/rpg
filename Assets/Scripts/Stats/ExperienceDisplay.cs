using TMPro;
using UnityEngine;

namespace RPG.Stats {
public class ExperienceDisplay : MonoBehaviour {
    TextMeshProUGUI experienceValue;
    Experience experience;

    private void Awake() {
        experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        experienceValue = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        experienceValue.text = experience.GetExperience().ToString();
    }
}}
