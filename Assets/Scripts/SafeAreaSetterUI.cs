using UnityEngine;

[DisallowMultipleComponent]
public class SafeAreaSetterUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private RectTransform _panelSafeArea;

    private Rect _currentSafeArea;
    private ScreenOrientation _currentOrientation = ScreenOrientation.AutoRotation;
    
    private void Start()
    {
        _panelSafeArea = GetComponent<RectTransform>();
        
        _currentOrientation = Screen.orientation;
        _currentSafeArea = Screen.safeArea;
        
        UpdateSafeArea();
    }

    private void UpdateSafeArea()
    {
        if (_panelSafeArea == null) return;
        
        var safeArea = Screen.safeArea;

        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= canvas.pixelRect.width;
        anchorMin.y /= canvas.pixelRect.height;
        
        anchorMax.x /= canvas.pixelRect.width;
        anchorMax.y /= canvas.pixelRect.height;

        _panelSafeArea.anchorMin = anchorMin;
        _panelSafeArea.anchorMax = anchorMax;

        _currentOrientation = Screen.orientation;
        _currentSafeArea = Screen.safeArea;
    }

    private void FixedUpdate()
    {
        if ((_currentOrientation != Screen.orientation) || (_currentSafeArea != Screen.safeArea))
        {
            UpdateSafeArea();
        }
    }
}