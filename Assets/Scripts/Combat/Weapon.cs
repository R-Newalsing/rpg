using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat {
[CreateAssetMenu(fileName = "Weapon", menuName = "weapons/Make New Weapon", order = 0)]
public class Weapon : ScriptableObject {
    [SerializeField] AnimatorOverrideController animatorOverride;
    [SerializeField] GameObject equippedPrefab = null;
    [SerializeField] Projectile projectile = null;
    [SerializeField] float weaponDamage = 5f;
    [SerializeField] float weaponRange = 2f;
    [SerializeField] bool isRightHanded = true;

    const string weaponName = "Weapon";

    public void Spawn(Transform rightHand, Transform leftHand, Animator animator) {
        DestroyOldWeapon(rightHand, leftHand);

        if (equippedPrefab != null) {
            GameObject weapon = Instantiate(equippedPrefab, GetHandTransform(rightHand, leftHand));
            weapon.name = weaponName;
        }

        var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

        if (animatorOverride != null) {
            animator.runtimeAnimatorController = animatorOverride;
        } else if(overrideController != null) {
            animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        }
    }

    void DestroyOldWeapon(Transform rightHand, Transform leftHand) {
        Transform oldWeapon = rightHand.Find(weaponName) ?? leftHand.Find(weaponName);
        
        if (oldWeapon == null) return;

        oldWeapon.name = "DESTROYING";
        Destroy(oldWeapon.gameObject); 
    }

    public bool HasProjectile() {
        return projectile != null;
    }

    public void LaunchProjectile(Transform rightHand, Transform leftHand, Health owner, Health target) {
        Projectile projectileInstance = Instantiate(
            projectile,
            GetHandTransform(rightHand, leftHand).position,
            Quaternion.identity
        );
        projectileInstance.SetOwnerAndTarget(owner, target, weaponDamage);
    }

    Transform GetHandTransform(Transform rightHand, Transform leftHand) {
        return isRightHanded ? rightHand : leftHand;
    }

    public float getDamage() {
        return weaponDamage;
    }

    public float getRange() {
        return weaponRange;
    }
}}