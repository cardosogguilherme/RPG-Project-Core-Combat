using System;
using RPG.Control;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] Transform target = null;

    void Update()
    {
        if (target == null) { return; }

        transform.LookAt(target);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null) { return target.position; }

        return target.position + Vector3.up * targetCapsule.height / 2;
    }
}
