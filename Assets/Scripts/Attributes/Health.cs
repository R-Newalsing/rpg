using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

namespace RPG.Attributes {
public class Health : MonoBehaviour, ISaveable {
    float healthPoints = -1f;
    bool isAlive = true;
    BaseStats baseStats;
    Animator animator;
    ActionScheduler scheduler;

    private void Awake() {
        animator = GetComponent<Animator>();
        scheduler = GetComponent<ActionScheduler>();
        baseStats = GetComponent<BaseStats>();
    }

    private void Start() {
        baseStats.onLevelUp += RegenerateHealth;

        if (healthPoints < 0) {
            healthPoints = baseStats.GetStat(Stat.Health);
        }
    }

    public bool IsDead() {
        return ! isAlive;
    }

    private void RegenerateHealth() {
        healthPoints = baseStats.GetStat(Stat.Health);
    }

    public void TakeDamage(GameObject instigator, float damage) {
        print(gameObject.name  + " took damag: " + damage);
        healthPoints = Mathf.Max(healthPoints - damage, 0);

        if (healthPoints == 0 && isAlive) {
            Die();
            AwardExperience(instigator);
        } 
    }

    public float GetHealthpoints() {
        return healthPoints;
    }

    public float GetMaxHealthpoints() {
        return baseStats.GetStat(Stat.Health);
    }

    public float GetHealthPercentage() {
        return Mathf.Abs(100 * (healthPoints / baseStats.GetStat(Stat.Health)));
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