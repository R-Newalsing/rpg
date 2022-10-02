using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control {
public class AIController : MonoBehaviour {
    public float chaseDistance = 5f;
    public float suspicionTime = 5f;
    public float guardTime = 2f;
    public float waypointTolerance = 1f;
    [Range(0, 1)] public float patrolSpeed = 0.2f;
    [Range(0, 1)] public float chaseSpeed = 0.9f;
    public PatrolPath patrolPath;

    float timeSinceLastSawPlayer = Mathf.Infinity;
    float timeSinceLastWaypoint = Mathf.Infinity;
    int currentWaypoint = 0;
    Vector3 guardPosition;

    ActionScheduler scheduler;
    GameObject player;
    Fighter fighter;
    Health health;
    Mover mover;

    private void Start() {
        player = GameObject.FindWithTag("Player");
        fighter = GetComponent<Fighter>();
        health = GetComponent<Health>();
        mover = GetComponent<Mover>();
        scheduler = GetComponent<ActionScheduler>();
        guardPosition = transform.position;
    }

    private void Update() {
        if (health.IsDead()) return;
        if (player == null) return;

        if (InAttackRange() && fighter.CanAttack(player)) {
            AttackBehaviour();
        } else if (IsSuspicous()) {
            SuspicionBehaviour();
        } else {
            PatrolBehaviour();
        }

        UpdateTimers();
    }

    void UpdateTimers() {
        timeSinceLastSawPlayer += Time.deltaTime;
        timeSinceLastWaypoint += Time.deltaTime;
    }

    void AttackBehaviour() {
        timeSinceLastSawPlayer = 0;
        mover.ChangeSpeed(chaseSpeed);
        fighter.Attack(player);
    }

    bool IsSuspicous() {
        return timeSinceLastSawPlayer < suspicionTime
            || timeSinceLastWaypoint < guardTime;
    }

    void SuspicionBehaviour() {
        scheduler.CancelCurrentAction();
    }

    void PatrolBehaviour() {
        Vector3 nextPosition = guardPosition;

        if (patrolPath != null) {
            if (AtWaypoint()) CycleWaypoint();
            nextPosition = GetCurrentWaypoint();
        }

        mover.ChangeSpeed(patrolSpeed)
            .StartMoveAction(nextPosition);
    }

    bool AtWaypoint() {
        return GetDistanceToWaypoint() < waypointTolerance;
    }

    void CycleWaypoint() {
        timeSinceLastWaypoint = 0;
        currentWaypoint = patrolPath.GetNextPosition(currentWaypoint);
    }

    Vector3 GetCurrentWaypoint() {
        return patrolPath.GetWaypoint(currentWaypoint);
    }

    float GetDistanceToWaypoint() {
        return MeasureDistance(transform.position, GetCurrentWaypoint());
    }

    bool InAttackRange() {
        return DistanceToPlayer() < chaseDistance;
    }

    float DistanceToPlayer() {
        return MeasureDistance(player.transform.position, transform.position);
    }

    float MeasureDistance(Vector3 start, Vector3 end) {
        return Vector3.Distance(start, end);
    }

    // called by unity
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}}