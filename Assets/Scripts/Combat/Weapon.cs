using UnityEngine;

namespace RPG.Combat {
[CreateAssetMenu(fileName = "Weapon", menuName = "weapons/Make New Weapon", order = 0)]
public class Weapon : ScriptableObject {
    [SerializeField] AnimatorOverrideController animatorOverride;
    [SerializeField] GameObject equippedPrefab = null;
    [SerializeField] float weaponDamage = 5f;
    [SerializeField] float weaponRange = 2f;

    public void Spawn(Transform handTransform, Animator animator) {
        if (equippedPrefab != null) {
            Instantiate(equippedPrefab, handTransform);
        }

        if (animatorOverride != null) {
            animator.runtimeAnimatorController = animatorOverride;
        }

    }

    public float getDamage() {
        return weaponDamage;
    }

    public float getRange() {
        return weaponRange;
    }
}}