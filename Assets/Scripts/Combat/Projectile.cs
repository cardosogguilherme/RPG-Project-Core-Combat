using System;
using RPG.Control;
using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    Health target = null;
    float damage = 0;

    void Update()
    {
        if (target == null) { return; }

        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != target) { return; }

        target.TakeDamage(damage);
        Destroy(gameObject);
    }

    public void SetTarget(Health target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null) { return target.transform.position; }

        return target.transform.position + Vector3.up * targetCapsule.height / 2;
    }
}
