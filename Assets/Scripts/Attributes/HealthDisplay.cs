using UnityEngine;
using TMPro;

namespace RPG.Attributes {
public class HealthDisplay : MonoBehaviour {
    TextMeshProUGUI healthValue;
    Health health;

    private void Awake() {
        health = GameObject.FindWithTag("Player").GetComponent<Health>();
        healthValue = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        healthValue.text = health.GetHealthPercentage() + "%"; 
    }
}}
