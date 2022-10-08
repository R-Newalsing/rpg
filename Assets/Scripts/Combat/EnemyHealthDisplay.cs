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
            healthValue.text = string.Format(
                "{0:0}/{1:0}",
                playerFighter.GetTarget().GetHealthpoints(),
                playerFighter.GetTarget().GetMaxHealthpoints()
            );
            return;
        }

        healthValue.text = defaultValue; 
    }
}}
