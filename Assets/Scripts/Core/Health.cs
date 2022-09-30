using UnityEngine;
using RPG.Saving;

namespace RPG.Core {
public class Health : MonoBehaviour, ISaveable {
    public float healthPoints = 100f;
    bool isAlive = true;
    Animator animator;
    ActionScheduler scheduler;

    public bool isDead() {
        return ! isAlive;
    }

    private void Awake() {
        animator = GetComponent<Animator>();
        scheduler = GetComponent<ActionScheduler>();
    }

    public void TakeDamage(float damage) {
        healthPoints = Mathf.Max(healthPoints - damage, 0);

        if (healthPoints == 0 && isAlive) {
            Die();
        } 
    }

    void Die() {
        isAlive = false;
        animator.SetTrigger("die");
        scheduler.CancelCurrentAction();
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