using UnityEngine;

namespace RPG.Core {
public class FollowCamera : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void LateUpdate() {
        transform.position = target.position;
    }
}}
