using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraConstantWidth : MonoBehaviour
{
    public float minOrthographicSize = 10;
    public Vector2 defaultResolution = new (720, 1280);
    [Range(0f, 1f)] public float widthOrHeight;

    private Camera _camera;

    private float _initialSize;
    private float _targetAspect;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _initialSize = _camera.orthographicSize;

        _targetAspect = defaultResolution.x / defaultResolution.y;
    }

    private void Update()
    {
        var constantWidthSize = _initialSize * (_targetAspect / _camera.aspect);
        var size = Mathf.Lerp(constantWidthSize, _initialSize, widthOrHeight);
        _camera.orthographicSize = size < minOrthographicSize ? minOrthographicSize : size;
    }
}
