using RPG.Core;
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

        if (animatorOverride != null) {
            animator.runtimeAnimatorController = animatorOverride;
        }

    }

    void DestroyOldWeapon(Transform rightHand, Transform leftHand) {
        Transform oldWeapon = rightHand.Find(weaponName) ?? rightHand.Find(weaponName);
        Debug.WriteLine(oldWeapon.name);
        if (oldWeapon == null) return;
        oldWeapon.name = "DESTROYING";
        Destroy(oldWeapon.gameObject); 
    }

    public bool HasProjectile() {
        return projectile != null;
    }

    public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target) {
        Projectile projectileInstance = Instantiate(
            projectile,
            GetHandTransform(rightHand, leftHand).position,
            Quaternion.identity
        );
        projectileInstance.SetTarget(target, weaponDamage);
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