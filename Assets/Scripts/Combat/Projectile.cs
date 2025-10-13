using System;
using RPG.Control;
using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] bool isHoming = false;

    Health target = null;
    float damage = 0;

    private void Start()
    {
        transform.LookAt(GetAimLocation());
    }

    void Update()
    {
        if (target == null) { return; }

        if (isHoming)
        {
            transform.LookAt(GetAimLocation());
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth != target) { return; }
        if (targetHealth.IsDead) { return; }

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
