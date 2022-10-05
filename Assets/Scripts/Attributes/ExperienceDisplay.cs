using TMPro;
using UnityEngine;

namespace RPG.Attributes {
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
