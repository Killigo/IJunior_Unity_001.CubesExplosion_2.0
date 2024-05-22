using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;

    public float SplitChance = 100f;

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

    private void SetRandomColor()
    {
        float channelRed = UnityEngine.Random.Range(0f, 1f);
        float channelGreen = UnityEngine.Random.Range(0f, 1f);
        float channelBlue = UnityEngine.Random.Range(0f, 1f);

        _renderer.material.color = new Color(channelRed, channelGreen, channelBlue);
    }
}
