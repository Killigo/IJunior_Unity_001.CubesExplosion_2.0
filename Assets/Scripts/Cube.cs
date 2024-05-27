using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;

    [NonSerialized] public float SplitChance = 100f;

    private Renderer _renderer;
    private Rigidbody _rigidbody;

    public event Action<Cube> Destroyed;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        SetRandomColor();
    }

    public void Segmentation()
    {
        Destroyed?.Invoke(this);
    }

    public void Explode()
    {
        _rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
    }

    public void ExplodeGlobal()
    {
        float scaleMultiplier = 1 / transform.localScale.magnitude;
        float explosionRadius = _explosionRadius * scaleMultiplier;
        float explosionForce = _explosionForce * scaleMultiplier;

        foreach (Rigidbody explodableObject in GetExplodableObjects(explosionRadius))
        {
            Vector3 direction = explodableObject.position - transform.position;
            float distance = direction.magnitude;
            float adjustedForce = explosionForce * (1 - (distance / explosionRadius));

            explodableObject.AddExplosionForce(adjustedForce, transform.position, explosionRadius);
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

    private void SetRandomColor()
    {
        float channelRed = UnityEngine.Random.Range(0f, 1f);
        float channelGreen = UnityEngine.Random.Range(0f, 1f);
        float channelBlue = UnityEngine.Random.Range(0f, 1f);

        _renderer.material.color = new Color(channelRed, channelGreen, channelBlue);
    }
}
