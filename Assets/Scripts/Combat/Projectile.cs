using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat {
public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] bool isHoming = false;
    [SerializeField] GameObject hitEffect = null;
    [SerializeField] float maxLifeTime = 10f;
    [SerializeField] GameObject[] destroyOnHit = null;
    [SerializeField] float lifeAfterImpact = 0.5f;
    Health target = null;
    Health owner = null;
    float damage = 0;

    void Update() {
        if (target == null) return;
        if (isHoming && !target.IsDead()) {
            transform.LookAt(GetAimLocation());
        }
        
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    public void SetOwnerAndTarget(Health owner, Health target, float damage) {
        this.target = target;
        this.damage = damage;
        this.owner = owner;
        
        if (! isHoming) transform.LookAt(GetAimLocation());
        Destroy(gameObject, maxLifeTime);
    }

    private Vector3 GetAimLocation() {
        CapsuleCollider collider = target.GetComponent<CapsuleCollider>();

        if (collider == null) {
            return target.transform.position;
        }

        return target.transform.position + Vector3.up * collider.height / 2;
    }

    private void OnTriggerEnter(Collider other) {
        Health foundTarget = other.GetComponent<Health>();

        if (foundTarget == null) return;
        if (foundTarget == owner) return;
        if (target.IsDead()) return;
        target.TakeDamage(owner.gameObject, damage);

        if (hitEffect != null) {
            GameObject impact = Instantiate(hitEffect, GetAimLocation(), transform.rotation);
        }

        foreach (GameObject toDestroy in destroyOnHit) {
            Destroy(toDestroy);
        }

        Destroy(gameObject, lifeAfterImpact);
    }
}}
