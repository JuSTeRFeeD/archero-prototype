using System;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Canvas))]
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _mainCameraTransform;

        private void Start()
        {
            if (Camera.main != null)
            {
                GetComponent<Canvas>().worldCamera = Camera.main; 
                _mainCameraTransform = Camera.main.transform;
            }
#if DEBUG
            else
            {
                throw new Exception("Camera not found!");
            } 
#endif
        }

        private void LateUpdate()
        {
            transform.forward = _mainCameraTransform.forward;
        }
    }
}
