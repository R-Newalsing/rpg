using UnityEngine;

namespace RPG.Control {
public class PatrolPath : MonoBehaviour {
    const float gizmoRadius = 0.3f;

    private void OnDrawGizmos() {
        for (int i = 0; i < transform.childCount; i++) {
            Gizmos.DrawSphere(GetWaypoint(i), gizmoRadius);
            Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextPosition(i)));
        }
    }

    public Vector3 GetWaypoint(int i) {
        return transform.GetChild(i).position;
    }

    public int GetNextPosition(int i) {
        if (i + 1 == transform.childCount) {
            return 0;
        }

        return i + 1;
    }
}}
