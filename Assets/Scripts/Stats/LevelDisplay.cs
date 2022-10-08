using TMPro;
using UnityEngine;

namespace RPG.Stats {
public class LevelDisplay : MonoBehaviour {
    TextMeshProUGUI level;
    BaseStats baseStats;

    private void Awake() {
        baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        level = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        level.text = baseStats.GetLevel().ToString();
    }
}}
