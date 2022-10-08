using UnityEngine.AI;
using UnityEngine;
using RPG.Attributes;
using RPG.Saving;
using RPG.Core;

namespace RPG.Movement {
public class Mover : MonoBehaviour, IAction, ISaveable {
    private NavMeshAgent agent;
    private Animator animator;
    private float maxSpeed = 6f;
    ActionScheduler scheduler;
    Health health;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        scheduler = GetComponent<ActionScheduler>();
        health = GetComponent<Health>();
    }

    void Update() {
        agent.enabled = ! health.IsDead();
        UpdateAnimator();
    }

    public void StartMoveAction(Vector3 destination) {
        scheduler.StartAction(this);
        MoveTo(destination);
    }

    public void MoveTo(Vector3 destination) {
        agent.isStopped = false;
        agent.SetDestination(destination);
    }

    public Mover ChangeSpeed(float speed) {
        agent.speed = maxSpeed * Mathf.Clamp01(speed);
        return this;
    }

    // set the animators forward speed
    void UpdateAnimator() {
        animator.SetFloat("forwardSpeed", GetVelocity().z);
    }

    // return the velocity of the local direction
    Vector3 GetVelocity() {
        return transform.InverseTransformDirection(agent.velocity);
    }

    public void Cancel() {
        agent.isStopped = true;
    }

    [System.Serializable]
    struct MoverSaveData {
        public SerializableVector3 position;
        public SerializableVector3 rotation;
    }

    public object CaptureState() {
        MoverSaveData data = new MoverSaveData();
        data.position = new SerializableVector3(transform.position);
        data.rotation = new SerializableVector3(transform.eulerAngles); 
        return data;
    }

    public void RestoreState(object state) {
        MoverSaveData data = (MoverSaveData) state;
        transform.position = data.position.ToVector();
        transform.eulerAngles = data.rotation.ToVector();
    }
}}
