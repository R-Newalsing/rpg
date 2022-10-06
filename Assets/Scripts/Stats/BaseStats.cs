using UnityEngine;

namespace RPG.Stats {
public class BaseStats : MonoBehaviour
{
    [Range(1, 99)] 
    [SerializeField] int startingLevel = 1;
    [SerializeField] CharacterClass characterClass;
    [SerializeField] Progression progression = null;

    public float GetStat(Stat stat) {
        return progression.GetStat(stat, characterClass, GetLevel());
    }

    public int GetLevel() {
        Experience experience = GetComponent<Experience>();

        if (experience == null) return startingLevel;

        float currentExperience = experience.GetExperience();
        int secondLastLevel = SecondLastLevel();

        for (int level = 1; level <= secondLastLevel; level++) {
            if (currentExperience < ExperienceToLevelUp(level)) {
                return level;
            }
        }

        return secondLastLevel + 1;
    }

    int SecondLastLevel () {
        return progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
    }

    float ExperienceToLevelUp(int level) {
        return progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
    }
}}
