using UnityEngine;
using TMPro;

namespace RPG.Combat {
public class EnemyHealthDisplay : MonoBehaviour {
    TextMeshProUGUI healthValue;
    Fighter playerFighter;
    string defaultValue = "N/A";

    private void Awake() {
        playerFighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        healthValue = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        if (playerFighter.GetTarget() != null) {
            healthValue.text = playerFighter.GetTarget().GetHealthPercentage() + "%"; 
        } else {
            healthValue.text = defaultValue; 
        }
    }
}}
