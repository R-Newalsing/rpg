using UnityEngine;
using RPG.Attributes;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Control {
public class PlayerController : MonoBehaviour {
    Mover mover;
    Health health;
    Fighter fighter;

    void Start() {
        mover = GetComponent<Mover>();
        fighter = GetComponent<Fighter>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update() {
        if (health.IsDead()) return;
        if (InteractWithCombat()) return;
        if (InteractWithMovement()) return;
    }

    bool InteractWithCombat() {
        RaycastHit[] hits = Physics.RaycastAll(GetRayPoint());

        foreach (RaycastHit hit in hits) {
            TargetCombat target = hit.transform.GetComponent<TargetCombat>();
    
            if (target == null) continue;
            if (! fighter.CanAttack(target.gameObject)) continue;

            if (Input.GetMouseButton(0)) {
                fighter.Attack(target.gameObject);
            }
            return true;
        }
        return false;
    }

    bool InteractWithMovement() {
        if (Physics.Raycast(GetRayPoint(), out RaycastHit hit)) {
            if (Input.GetMouseButton(0)) {
                mover.StartMoveAction(hit.point);
            }
            return true;
        }
        return false;
    }

    // get the screen point ray on the mouse position
    Ray GetRayPoint() {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}}

