using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube[] _prefabs;
    [SerializeField] private int _minSpawnCount = 2;
    [SerializeField] private int _maxSpawnCount = 6;

    private float _minExplodeChance = 0f;
    private float _maxExplodeChance = 100f;
    private int _chanceDivisor = 2;
    private int _scaleDivisor = 2;

    private ObjectPool<Cube> _pool;

    private void Start()
    {
        _pool = new ObjectPool<Cube>(_prefabs, transform, 10);

        Cube cube = SpawnCube();
        cube.transform.position = new Vector3(0, 5, 0);
    }

    private void OnDestroyed(Cube cube)
    {
        List<Cube> explodableCubes = new();

        float randomValue = Random.Range(_minExplodeChance, _maxExplodeChance);

        if (randomValue <= cube.SplitChance)
        {
            int spawnCount = Random.Range(_minSpawnCount, _maxSpawnCount);

            for (int i = 0; i < spawnCount; i++)
            {
                Cube newCube = SpawnCube();
                newCube.transform.position = cube.transform.localPosition;
                newCube.transform.localScale = cube.transform.localScale / _scaleDivisor;
                newCube.SplitChance = cube.SplitChance / _chanceDivisor;

                explodableCubes.Add(newCube);
            }

            foreach (Cube explodableCube in explodableCubes)
                explodableCube.Explode();
        }

        cube.Destroyed -= OnDestroyed;
        _pool.PutObject(cube);
    }

    private Cube SpawnCube()
    {
        Cube cube = _pool.GetObject();
        cube.Destroyed += OnDestroyed;

        return cube;
    }
}
