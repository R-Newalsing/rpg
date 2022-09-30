using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat {
public class Fighter : MonoBehaviour, IAction {
    [SerializeField] float timeBetweenAttacks = 1f;
    [SerializeField] Transform handTransform = null;
    [SerializeField] Weapon defaultWeapon = null;
    float timeSinceLastAttack = 2f;

    Weapon currentWeapon = null;
    Health target;
    ActionScheduler scheduler;
    Animator animator;
    Mover mover;

    private void Start() {
        mover = GetComponent<Mover>();
        scheduler = GetComponent<ActionScheduler>();
        animator = GetComponent<Animator>();
        EquipWeapon(defaultWeapon);
    }

    private void Update() {
        timeSinceLastAttack += Time.deltaTime;

        // if there is no target, do nothing
        if (target == null) return;
        if (target.isDead()) return;

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
        currentWeapon = weapon;
        weapon.Spawn(handTransform, animator);
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

        // do damage to the target
        target.TakeDamage(currentWeapon.getDamage());
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
        return distance < currentWeapon.getRange();
    }

    public bool CanAttack(GameObject targetCombat) {
        if (targetCombat == null) return false;

        Health targetToTest = targetCombat.GetComponent<Health>();
        return targetToTest != null && !targetToTest.isDead();
    }
}}