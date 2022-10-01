using UnityEngine;
using RPG.Core;

namespace RPG.Combat {
public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    Health target = null;
    float damage = 0;

    void Update() {
        if (target == null) return;

        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    public void SetTarget(Health target, float damage) {
        this.target = target;
        this.damage = damage;
    }

    private Vector3 GetAimLocation() {
        CapsuleCollider collider = target.GetComponent<CapsuleCollider>();

        if (collider == null) {
            return target.transform.position;
        }

        return target.transform.position + Vector3.up * collider.height / 2;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Health>() == null) return;

        target.TakeDamage(damage);
        Destroy(gameObject);
    }
}}
