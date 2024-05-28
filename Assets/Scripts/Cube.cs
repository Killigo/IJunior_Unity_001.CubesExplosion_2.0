using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Explosion))]

public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private Explosion _explosion;

    public event Action<Cube> Destroyed;

    public float SplitChance { get; private set; }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _explosion = GetComponent<Explosion>();

        SplitChance = 100f;
    }

    private void OnEnable()
    {
        SetRandomColor();
    }

    public void SetSplitChance(float value)
    {
        if (value > 0)
            SplitChance = value;
    }

    public void Segmentation()
    {
        Destroyed?.Invoke(this);
    }

    public void Explode()
    {
        _explosion.LocalBurst();
    }

    public void ExplodeGlobal()
    {
        _explosion.GlobalBurst();
    }

    private void SetRandomColor()
    {
        float channelRed = UnityEngine.Random.Range(0f, 1f);
        float channelGreen = UnityEngine.Random.Range(0f, 1f);
        float channelBlue = UnityEngine.Random.Range(0f, 1f);

        _renderer.material.color = new Color(channelRed, channelGreen, channelBlue);
    }
}
