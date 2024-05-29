using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _explosionForce = 500f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void PushLocal()
    {
        _rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
    }

    public void PushGlobal()
    {
        float scaleMultiplier = 1 / transform.localScale.magnitude;
        float newExplosionRadius = _explosionRadius * scaleMultiplier;
        float newExplosionForce = _explosionForce * scaleMultiplier;

        foreach (Rigidbody explodableObject in GetExplodableObjects(newExplosionRadius))
        {
            Vector3 direction = explodableObject.position - transform.position;
            float distance = direction.magnitude;
            float adjustedForce = newExplosionForce * (1 - (distance / newExplosionRadius));

            explodableObject.AddExplosionForce(adjustedForce, transform.position, newExplosionRadius);
        }
    }

    private List<Rigidbody> GetExplodableObjects(float explosionRadius)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        List<Rigidbody> cubes = new();

        foreach (Collider hit in hits)
            if (hit.attachedRigidbody != null)
                cubes.Add(hit.attachedRigidbody);

        return cubes;
    }
}
