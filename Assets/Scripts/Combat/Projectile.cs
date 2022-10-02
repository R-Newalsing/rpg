using UnityEngine;
using RPG.Core;

namespace RPG.Combat {
public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] bool isHoming = false;
    [SerializeField] GameObject hitEffect = null;
    Health target = null;
    float damage = 0;

    void Update() {
        if (isHoming && !target.IsDead()) {
            transform.LookAt(GetAimLocation());
        }
        
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    public void SetTarget(Health target, float damage) {
        this.target = target;
        this.damage = damage;
        if (! isHoming) transform.LookAt(GetAimLocation());
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
        if (target.IsDead()) return;
        if (hitEffect != null) {
            Instantiate(hitEffect, GetAimLocation(), transform.rotation);
        } 

        target.TakeDamage(damage);
        Destroy(gameObject);
    }
}}
