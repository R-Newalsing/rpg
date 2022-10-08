using UnityEngine;
using System;
using System.Linq;

namespace RPG.Stats {
public class BaseStats : MonoBehaviour {
    [Range(1, 99)] 
    [SerializeField] int startingLevel = 1;
    [SerializeField] CharacterClass characterClass;
    [SerializeField] Progression progression = null;
    [SerializeField] GameObject levelUpEffect = null;
    [SerializeField] bool shouldUseModefiers = false;

    int currentLevel = 0;
    public event Action onLevelUp;

    private void Start() {
        currentLevel = CalculateLevel();
        Experience experience = GetComponent<Experience>();

        if (experience != null) {
            experience.onExperienceGained += UpdateLevel;
        }
    }

    public int GetLevel() {
        if (currentLevel < 1) {
            currentLevel = CalculateLevel();
        }

        return currentLevel;
    }

    public float GetStat(Stat stat) {
        return (GetBaseStat(stat) + GetAdditiveModefier(stat)) * (1 + GetPercentageModefier(stat) / 100);
    }

    private float GetBaseStat(Stat stat) {
        return progression.GetStat(stat, characterClass, GetLevel());
    }

    private int CalculateLevel() {
        Experience experience = GetComponent<Experience>();

        if (experience == null) return startingLevel;
        if (progression == null) return startingLevel;

        float currentExperience = experience.GetExperience();
        int secondLastLevel = SecondLastLevel();

        for (int level = 1; level <= secondLastLevel; level++) {
            if (currentExperience < ExperienceToLevelUp(level)) {
                return level;
            }
        }

        return secondLastLevel + 1;
    }

    private void UpdateLevel() {
        int newLevel = CalculateLevel();

        if (newLevel > currentLevel) {
            currentLevel = newLevel;
            LevelUpEffect();
            onLevelUp();
        }
    }

    private float GetAdditiveModefier(Stat stat) {
        if (!shouldUseModefiers) return 0;

        return GetComponents<IModefierProvider>().Aggregate(0f, (sum, provider) => 
            sum + provider.GetAdditiveModefiers(stat).Sum()
        );
    }

    private float GetPercentageModefier(Stat stat) {
        if (!shouldUseModefiers) return 0;
        
        return GetComponents<IModefierProvider>().Aggregate(0f, (sum, provider) => 
            sum + provider.GetPercentageModefiers(stat).Sum()
        );
    }

    private int SecondLastLevel () {
        return progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
    }

    private float ExperienceToLevelUp(int level) {
        return progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
    }

    private void LevelUpEffect() {
        Instantiate(levelUpEffect, transform);
    }
}}
