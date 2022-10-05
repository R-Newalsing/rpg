using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

namespace RPG.Attributes {
public class Health : MonoBehaviour, ISaveable {
    public float healthPoints = 100f;
    float maxHealthPoints = 100f;
    bool isAlive = true;
    BaseStats baseStats;
    Animator animator;
    ActionScheduler scheduler;

    private void Start() {
        baseStats = GetComponent<BaseStats>();
        healthPoints = baseStats.GetStat(Stat.Health);
        maxHealthPoints = healthPoints;
    }

    public bool IsDead() {
        return ! isAlive;
    }

    private void Awake() {
        animator = GetComponent<Animator>();
        scheduler = GetComponent<ActionScheduler>();
    }

    public void TakeDamage(GameObject instigator, float damage) {
        healthPoints = Mathf.Max(healthPoints - damage, 0);

        if (healthPoints == 0 && isAlive) {
            Die();
            AwardExperience(instigator);
        } 
    }

    public float GetHealthPercentage() {
        return Mathf.Abs(100 * (healthPoints / maxHealthPoints));
    }

    void Die() {
        isAlive = false;
        animator.SetTrigger("die");
        scheduler.CancelCurrentAction();
    }

    void AwardExperience(GameObject instigator) {
        Experience experience = instigator.GetComponent<Experience>();

        if (experience == null) return;

        experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
    }

    public object CaptureState() {
        return healthPoints;
    }

    public void RestoreState(object state) {
        float health = (float) state;
        healthPoints = health;

        if (healthPoints <= 0) {
            Die();
        }
    }
}}