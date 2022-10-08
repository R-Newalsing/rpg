using UnityEngine;
using RPG.Attributes;
using RPG.Movement;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace RPG.Combat {
public class Fighter : MonoBehaviour, IAction, ISaveable, IModefierProvider {
    [SerializeField] float timeBetweenAttacks = 1f;
    [SerializeField] Transform rightHandTransform = null;
    [SerializeField] Transform leftHandTransform = null;
    [SerializeField] Weapon defaultWeapon = null;

    float timeSinceLastAttack = 2f;
    LazyValue<Weapon> currentWeapon;
    ActionScheduler scheduler;
    BaseStats baseStats;
    Animator animator;
    Health target;
    Mover mover;

    private void Awake() {
        mover = GetComponent<Mover>();
        scheduler = GetComponent<ActionScheduler>();
        animator = GetComponent<Animator>();
        baseStats = GetComponent<BaseStats>();
        currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
    }

    private void Start() {
        currentWeapon.ForceInit();
    }

    private Weapon SetupDefaultWeapon() {
        AttachWeapon(defaultWeapon);
        return defaultWeapon;
    }

    private void Update() {
        timeSinceLastAttack += Time.deltaTime;

        // if there is no target, do nothing
        if (target == null) return;
        if (target.IsDead()) return;

        if (!IsInWeaponDistance()) {
            // if the fighter is not in weapon range, move towards the target
            mover.MoveTo(target.transform.position);
        } else {
            // cancel the movement because we are now in weapon range
            mover.Cancel();
            AttackBehaviour();
        }
    }

    public void EquipWeapon(Weapon weapon) {
        currentWeapon.value = weapon;
        AttachWeapon(weapon);
    }

    private void AttachWeapon(Weapon weapon) {
        weapon.Spawn(rightHandTransform, leftHandTransform, animator);
    }

    public Health GetTarget() {
        return target;
    }

    public IEnumerable<float> GetAdditiveModefiers(Stat stat) {
        if (stat == Stat.Damage) yield return currentWeapon.value.GetDamage();
    }

    public IEnumerable<float> GetPercentageModefiers(Stat stat) {
        if (stat == Stat.Damage) yield return currentWeapon.value.GetPercentageBonus();
    }

    private void AttackBehaviour() {
        transform.LookAt(target.transform.position);

        if (timeSinceLastAttack < timeBetweenAttacks) return;
        // this will trigger the Hit() event
        TriggerAttack();
        timeSinceLastAttack = 0;
    }

    void TriggerAttack() {
        animator.ResetTrigger("stopAttack");
        animator.SetTrigger("attack");
    }
    
    // animation event
    void Hit() {
        if (target == null) return;
        float damage = baseStats.GetStat(Stat.Damage);

        if (currentWeapon.value.HasProjectile()) {
            currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, GetOwner(), target, damage);
            return;
        }

        // do damage to the target
        target.TakeDamage(gameObject, damage);
    }

    Health GetOwner() {
        return GetComponent<Health>();
    }

    // animation event
    void Shoot() {
        Hit();
    }

    public void Attack(GameObject combatTarget) {
        // call start on scheduler so it can stop any ongoing actions
        scheduler.StartAction(this);
        target = combatTarget.GetComponent<Health>();
    }

    public void Cancel() {
        StopAttack();
        target = null;
    }

    void StopAttack() {
        animator.ResetTrigger("attack");
        animator.SetTrigger("stopAttack");
    }

    //  get the distance between the fighter and the target 
    bool IsInWeaponDistance() {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        return distance < currentWeapon.value.GetRange();
    }

    public bool CanAttack(GameObject targetCombat) {
        if (targetCombat == null) return false;

        Health targetToTest = targetCombat.GetComponent<Health>();
        return targetToTest != null && !targetToTest.IsDead();
    }

    public object CaptureState() {
        return currentWeapon.value.name;
    }

    public void RestoreState(object state) {
        string weaponName = (string) state;
        Weapon weapon = Resources.Load<Weapon>(weaponName);
        EquipWeapon(weapon);
    }
}}