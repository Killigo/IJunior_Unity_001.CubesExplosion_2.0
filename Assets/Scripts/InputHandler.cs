using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private float _maxDistance = 1000f;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance) && hit.collider.TryGetComponent(out Cube cube))
            {
                cube.Segment();
            }
        }
    }
}
