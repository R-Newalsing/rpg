using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using GameDevTV.Utils;

namespace RPG.Attributes {
public class Health : MonoBehaviour, ISaveable {
    LazyValue<float> healthPoints;
    bool isAlive = true;
    BaseStats baseStats;
    Animator animator;
    ActionScheduler scheduler;

    private void Awake() {
        animator = GetComponent<Animator>();
        scheduler = GetComponent<ActionScheduler>();
        baseStats = GetComponent<BaseStats>();
        healthPoints = new LazyValue<float>(GetInitialHealth);
    }

    private void Start() {
        healthPoints.ForceInit();
    }

    private float GetInitialHealth() {
        return baseStats.GetStat(Stat.Health);
    }

    private void OnEnable() {
        baseStats.onLevelUp += RegenerateHealth;
    }

    public bool IsDead() {
        return ! isAlive;
    }

    private void RegenerateHealth() {
        healthPoints.value = baseStats.GetStat(Stat.Health);
    }

    public void TakeDamage(GameObject instigator, float damage) {
        print(gameObject.name  + " took damag: " + damage);
        healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

        if (healthPoints.value == 0 && isAlive) {
            Die();
            AwardExperience(instigator);
        } 
    }

    public float GetHealthpoints() {
        return healthPoints.value;
    }

    public float GetMaxHealthpoints() {
        return baseStats.GetStat(Stat.Health);
    }

    public float GetHealthPercentage() {
        return Mathf.Abs(100 * (healthPoints.value / baseStats.GetStat(Stat.Health)));
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
        healthPoints.value = health;

        if (healthPoints.value <= 0) {
            Die();
        }
    }
}}